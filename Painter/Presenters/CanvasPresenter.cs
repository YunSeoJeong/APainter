using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Painter.Interfaces;
using Painter.Models;
using Painter.Strategies;

namespace Painter.Presenters
{
    public unsafe class CanvasPresenter
    {
        private readonly ICanvasView _view;
        private readonly IBitmapModel _bitmapModel;
        private readonly IPainterSettingsModel _settingsModel;
        private readonly IToolStrategyFactory _toolStrategyFactory;
        private Point? _lastPoint;
        private Bitmap? _tempBitmap; // 임시 비트맵 (완전 투명 초기값)
        private Bitmap? _maskBitmap; // 마스크 비트맵 (완전 불투명 초기값)
        private Rectangle _dirtyRect; // Dirty 영역 추적

        /// <summary>
        /// CanvasPresenter 생성자. 뷰, 모델, 설정, 도구 전략 팩토리를 초기화하고 이벤트를 구독합니다.
        /// </summary>
        public CanvasPresenter(ICanvasView view, IBitmapModel bitmapModel,
                              IPainterSettingsModel settingsModel,
                              IToolStrategyFactory toolStrategyFactory)
        {
            _view = view;
            _bitmapModel = bitmapModel;
            _settingsModel = settingsModel;
            _toolStrategyFactory = toolStrategyFactory;

            _view.MouseDownEvent += OnMouseDown;
            _view.MouseMoveEvent += OnMouseMove;
            _view.MouseUpEvent += OnMouseUp;
            
            // 도구 변경 이벤트 구독 추가
            _settingsModel.ToolChanged += OnToolChanged;
            
            // 초기 비트맵 설정
            InitializeBitmaps();
            _view.SetBitmap(_bitmapModel.GetBitmap());
        }

        /// <summary>
        /// 마우스 버튼이 눌렸을 때 호출되는 이벤트 핸들러.
        /// </summary>
        public void OnMouseDown(object? sender, MouseEventArgs e)
        {
            _lastPoint = e.Location;
            
            // 단일 클릭 시점에 점 그리기
            if (e.Button == MouseButtons.Left)
            {
                DrawLine(e.Location, e.Location);
            }
        }

        /// <summary>
        /// 마우스가 움직일 때 호출되는 이벤트 핸들러. 드래그 시 선을 그립니다.
        /// </summary>
        public void OnMouseMove(object? sender, MouseEventArgs e)
        {
            if (_lastPoint.HasValue && e.Button == MouseButtons.Left)
            {
                DrawLine(_lastPoint.Value, e.Location);
                _lastPoint = e.Location; // 마지막 위치 업데이트 (연속적인 드로잉을 위해)
            }
        }

        /// <summary>
        /// 마우스 버튼이 떼어졌을 때 호출되는 이벤트 핸들러. 변경 사항을 최종 적용합니다.
        /// </summary>
        public void OnMouseUp(object? sender, MouseEventArgs e)
        {
            _lastPoint = null;
            FlushChanges();
        }
        
        /// <summary>
        /// 임시 비트맵 변경사항을 메인 비트맵에 병합하고 마스크를 초기화합니다.
        /// </summary>
        private void FlushChanges()
        {
            if (_dirtyRect != Rectangle.Empty)
            {
                MergeTempBitmaps();
                _dirtyRect = Rectangle.Empty;
                _view.SetBitmap(_bitmapModel.GetBitmap());
                
                // 마스크 비트맵 초기화 (흰색으로 재설정)
                if (_maskBitmap != null)
                {
                    using (var g = Graphics.FromImage(_maskBitmap))
                    {
                        g.Clear(Color.White);
                    }
                }
            }
        }
        
        /// <summary>
        /// 임시 비트맵과 마스크 비트맵을 초기화합니다.
        /// </summary>
        private void InitializeBitmaps()
        {
            var mainBitmap = _bitmapModel.GetBitmap();
            
            // 임시 비트맵 초기화 (완전 투명)
            _tempBitmap = new Bitmap(mainBitmap.Width, mainBitmap.Height, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(_tempBitmap))
            {
                g.Clear(Color.Transparent);
            }

            // 마스크 비트맵 초기화 (완전 불투명)
            _maskBitmap = new Bitmap(mainBitmap.Width, mainBitmap.Height, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(_maskBitmap))
            {
                g.Clear(Color.White);
            }
        }

        /// <summary>
        /// 현재 도구를 사용하여 시작점과 끝점 사이에 선을 그립니다.
        /// </summary>
        private void DrawLine(Point start, Point end)
        {
            var toolStrategy = _toolStrategyFactory.CreateToolStrategy(_settingsModel.CurrentTool);
            toolStrategy.SetBrushSize(_settingsModel.BrushSize); // 브러시 크기 설정
            
            // 현재 도구에 따라 적절한 비트맵 선택
            Bitmap targetBitmap = _settingsModel.CurrentTool == ToolType.Eraser ? _maskBitmap : _tempBitmap;
            if (targetBitmap == null) return;

            float opacity = _settingsModel.CurrentTool == ToolType.Eraser ? 0.2f : 0.8f;

            // DrawingContext를 사용한 고속 픽셀 처리
            using (var context = new DrawingContext(
                start,
                end,
                _settingsModel.PrimaryColor,
                _settingsModel.BrushSize,
                targetBitmap,
                opacity))
            {
                toolStrategy.Draw(context);
            }
            
            // Dirty Rect 업데이트 (도구 반지름 활용)
            int radiusInt = (int)Math.Ceiling(toolStrategy.Radius);
            int diameter = radiusInt * 2;
            Rectangle lineRect = new Rectangle(
                Math.Min(start.X, end.X) - radiusInt,
                Math.Min(start.Y, end.Y) - radiusInt,
                Math.Abs(end.X - start.X) + diameter,
                Math.Abs(end.Y - start.Y) + diameter
            );
            _dirtyRect = UpdateDirtyRect(_dirtyRect, lineRect);
            
            // 실시간 합성 렌더링 (Dirty Rect만 전달)
            _view.SetCompositeBitmap(
                _bitmapModel.GetBitmap(),
                _tempBitmap,
                _maskBitmap, // 항상 전달 (도구에 따라 사용 여부 결정)
                _settingsModel.CurrentTool, // 현재 도구 전달
                _dirtyRect // 변경된 영역 전달
            );
        }

        // 도구 변경 핸들러
        /// <summary>
        /// 현재 사용 도구가 변경될 때 호출되는 이벤트 핸들러.
        /// </summary>
        private void OnToolChanged()
        {
            System.Diagnostics.Debug.WriteLine($"Tool changed to: {_settingsModel.CurrentTool}");
        }

        // 알파 블렌딩 메서드 (표준 오버 연산)
        /// <summary>
        /// 두 색상을 알파 블렌딩하여 혼합된 색상을 반환합니다.
        /// </summary>
        public static Color AlphaBlend(Color src, Color dst)
        {
            if (src.A == 0) return dst; // 소스가 완전 투명하면 대상 픽셀 반환
            if (src.A == 255) return src; // 소스가 완전 불투명하면 소스 픽셀 반환

            int srcA = src.A;
            int invSrcA = 255 - srcA;

            int r = (src.R * srcA + dst.R * invSrcA) / 255;
            int g = (src.G * srcA + dst.G * invSrcA) / 255;
            int b = (src.B * srcA + dst.B * invSrcA) / 255;
            int a = srcA + (dst.A * invSrcA) / 255;

            r = Math.Clamp(r, 0, 255);
            g = Math.Clamp(g, 0, 255);
            b = Math.Clamp(b, 0, 255);
            a = Math.Clamp(a, 0, 255);

            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// 임시 비트맵과 마스크 비트맵을 메인 비트맵에 병합합니다.
        /// </summary>
        private void MergeTempBitmaps()
        {
            _bitmapModel.Lock();
            try
            {
                MergeTempBitmapLayer();
                MergeMaskBitmapLayer();
            }
            finally
            {
                _bitmapModel.Unlock();
            }
        }

        /// <summary>
        /// 임시 비트맵 레이어의 내용을 메인 비트맵에 병합합니다.
        /// </summary>
        private void MergeTempBitmapLayer()
        {
            if (_tempBitmap == null) return;

            var srcData = _tempBitmap.LockBits(
                new Rectangle(0, 0, _tempBitmap.Width, _tempBitmap.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb
            );
            
            var dstData = _bitmapModel.LockBits(
                new Rectangle(0, 0, _bitmapModel.Width, _bitmapModel.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb
            );
            
            try
            {
                byte* srcPtr = (byte*)srcData.Scan0;
                byte* dstPtr = (byte*)dstData.Scan0;
                int srcStride = srcData.Stride;
                int dstStride = dstData.Stride;
                
                for (int y = 0; y < _tempBitmap.Height; y++)
                {
                    for (int x = 0; x < _tempBitmap.Width; x++)
                    {
                        int srcIndex = y * srcStride + x * 4;
                        int dstIndex = y * dstStride + x * 4;
                        
                        byte srcA = srcPtr[srcIndex + 3];
                        if (srcA > 0)
                        {
                            Color srcColor = ReadPixel(srcPtr, srcIndex);
                            Color dstColor = ReadPixel(dstPtr, dstIndex);
                            
                            Color blended = AlphaBlend(srcColor, dstColor);
                            
                            dstPtr[dstIndex] = blended.B;
                            dstPtr[dstIndex + 1] = blended.G;
                            dstPtr[dstIndex + 2] = blended.R;
                            dstPtr[dstIndex + 3] = blended.A;
                        }
                    }
                }
            }
            finally
            {
                _tempBitmap.UnlockBits(srcData);
                _bitmapModel.UnlockBits(dstData);
            }
            
            using (var g = Graphics.FromImage(_tempBitmap))
            {
                g.Clear(Color.Transparent);
            }
        }
        
        /// <summary>
        /// 마스크 비트맵 레이어의 내용을 메인 비트맵에 병합합니다.
        /// </summary>
        private void MergeMaskBitmapLayer()
        {
            if (_maskBitmap == null) return;

            var maskData = _maskBitmap.LockBits(
                new Rectangle(0, 0, _maskBitmap.Width, _maskBitmap.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb
            );
            
            var dstData = _bitmapModel.LockBits(
                new Rectangle(0, 0, _bitmapModel.Width, _bitmapModel.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb
            );
            
            try
            {
                byte* maskPtr = (byte*)maskData.Scan0;
                byte* dstPtr = (byte*)dstData.Scan0;
                int maskStride = maskData.Stride;
                int dstStride = dstData.Stride;
                
                for (int y = 0; y < _maskBitmap.Height; y++)
                {
                    for (int x = 0; x < _maskBitmap.Width; x++)
                    {
                        int maskIndex = y * maskStride + x * 4;
                        int dstIndex = y * dstStride + x * 4;
                        
                        byte maskA = maskPtr[maskIndex + 3];
                        if (maskA < 255)
                        {
                            byte currentAlpha = dstPtr[dstIndex + 3];
                            byte newAlpha = Math.Min(currentAlpha, maskA);
                            dstPtr[dstIndex + 3] = newAlpha;
                        }
                    }
                }
            }
            finally
            {
                _maskBitmap.UnlockBits(maskData);
                _bitmapModel.UnlockBits(dstData);
            }
        }
        
        /// <summary>
        /// 메모리 포인터에서 픽셀 색상 값을 읽습니다.
        /// </summary>
        private static Color ReadPixel(byte* ptr, int index)
        {
            return Color.FromArgb(
                ptr[index + 3],
                ptr[index + 2],
                ptr[index + 1],
                ptr[index]
            );
        }

        /// <summary>
        /// Dirty 영역을 업데이트하여 변경이 필요한 영역을 추적합니다.
        /// </summary>
        private Rectangle UpdateDirtyRect(Rectangle current, Rectangle newArea)
        {
            return current == Rectangle.Empty ? newArea : Rectangle.Union(current, newArea);
        }

    }
}
