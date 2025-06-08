using System;
using System.Drawing;
using System.Windows.Forms;
using Painter.Interfaces;
using Painter.Models;
using Painter.Strategies;

namespace Painter.Presenters
{
    public class CanvasPresenter
    {
        private readonly ICanvasView _view;
        private readonly IBitmapModel _bitmapModel;
        private readonly IPainterSettingsModel _settingsModel;
        private readonly IToolStrategyFactory _toolStrategyFactory;
        private Point? _lastPoint;
        private Bitmap? _tempBitmap; // 임시 비트맵 (완전 투명 초기값)
        private Bitmap? _maskBitmap; // 마스크 비트맵 (완전 불투명 초기값)
        private Rectangle _dirtyRect; // Dirty 영역 추적

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

        public void OnMouseDown(object? sender, MouseEventArgs e)
        {
            _lastPoint = e.Location;
            
            // 단일 클릭 시점에 점 그리기
            if (e.Button == MouseButtons.Left)
            {
                DrawLine(e.Location, e.Location);
            }
        }

        public void OnMouseMove(object? sender, MouseEventArgs e)
        {
            Console.WriteLine($"MouseMove: Button={e.Button}, Location={e.Location}, LastPoint={_lastPoint}");
            if (_lastPoint.HasValue && e.Button == MouseButtons.Left)
            {
                DrawLine(_lastPoint.Value, e.Location);
                _lastPoint = e.Location; // 마지막 위치 업데이트 (연속적인 드로잉을 위해)
            }
        }

        public void OnMouseUp(object? sender, MouseEventArgs e)
        {
            _lastPoint = null;
            FlushChanges();
        }
        
        private void FlushChanges()
        {
            if (_dirtyRect != Rectangle.Empty)
            {
                MergeTempBitmaps();
                _dirtyRect = Rectangle.Empty;
                _view.SetBitmap(_bitmapModel.GetBitmap());
            }
        }
        
        private void InitializeBitmaps()
        {
            var mainBitmap = _bitmapModel.GetBitmap();
            
            // 임시 비트맵 초기화 (완전 투명)
            _tempBitmap = new Bitmap(mainBitmap.Width, mainBitmap.Height);
            using (var g = Graphics.FromImage(_tempBitmap))
            {
                g.Clear(Color.Transparent);
            }

            // 마스크 비트맵 초기화 (완전 불투명)
            _maskBitmap = new Bitmap(mainBitmap.Width, mainBitmap.Height);
            using (var g = Graphics.FromImage(_maskBitmap))
            {
                g.Clear(Color.White);
            }
        }

        private void DrawLine(Point start, Point end)
        {
            Console.WriteLine($"Using tool: {_settingsModel.CurrentTool}");
            var toolStrategy = _toolStrategyFactory.CreateToolStrategy(_settingsModel.CurrentTool);
            
            // 현재 도구에 따라 적절한 비트맵 선택
            Action<int, int, Color> setPixel;
            Func<int, int, Color> getPixel;
            
            if (_settingsModel.CurrentTool == ToolType.Eraser)
            {
                setPixel = (x, y, color) => {
                    if (_maskBitmap != null)
                        _maskBitmap.SetPixel(x, y, Color.FromArgb(
                            (int)(color.A * 0.2f), // 지우개 강도 조절
                            color.R, color.G, color.B));
                    _dirtyRect = UpdateDirtyRect(_dirtyRect, new Rectangle(x, y, 1, 1));
                };
                getPixel = (x, y) => _maskBitmap?.GetPixel(x, y) ?? Color.White;
            }
            else
            {
                setPixel = (x, y, color) => {
                    if (_tempBitmap != null)
                        _tempBitmap.SetPixel(x, y, color);
                    _dirtyRect = UpdateDirtyRect(_dirtyRect, new Rectangle(x, y, 1, 1));
                };
                getPixel = (x, y) => _tempBitmap?.GetPixel(x, y) ?? Color.Transparent;
            }

            var context = new DrawingContext(
                start,
                end,
                _settingsModel.PrimaryColor,
                _settingsModel.BrushSize,
                setPixel,
                getPixel,
                _settingsModel.CurrentTool == ToolType.Eraser ? 0.2f : 0.8f // 도구별 기본 투명도
            );
            toolStrategy.Draw(context);
            
            // 실시간 합성 렌더링
            _view.SetCompositeBitmap(
                _bitmapModel.GetBitmap(),
                _tempBitmap,
                _settingsModel.CurrentTool == ToolType.Eraser ? _maskBitmap : null
            );
        }

        // 도구 변경 핸들러 추가
        private void OnToolChanged()
        {
            System.Diagnostics.Debug.WriteLine($"Tool changed to: {_settingsModel.CurrentTool}");
        }


        private void MergeTempBitmaps()
        {
            _bitmapModel.Lock();
            try
            {
                if (_tempBitmap != null)
                {
                    // 변경: 픽셀 단위 복사 방식으로 변경 (미리보기 병합 로직과 동일)
                    for (int y = 0; y < _tempBitmap.Height; y++)
                    {
                        for (int x = 0; x < _tempBitmap.Width; x++)
                        {
                            var srcColor = _tempBitmap.GetPixel(x, y);
                            if (srcColor.A > 0) // 투명하지 않은 픽셀만 복사
                            {
                                _bitmapModel.SetPixel(x, y, srcColor);
                            }
                        }
                    }
                    
                    // 임시 비트맵 초기화
                    using (var g = Graphics.FromImage(_tempBitmap))
                    {
                        g.Clear(Color.Transparent);
                    }
                }

                if (_maskBitmap != null)
                {
                    // 마스크 비트맵 병합 (곱연산)
                    for (int y = 0; y < _maskBitmap.Height; y++)
                    {
                        for (int x = 0; x < _maskBitmap.Width; x++)
                        {
                            var maskColor = _maskBitmap.GetPixel(x, y);
                            if (maskColor.A < 255)
                            {
                                var dstColor = _bitmapModel.GetPixel(x, y);
                                var blended = DrawingContext.MultiplyColors(maskColor, dstColor);
                                _bitmapModel.SetPixel(x, y, blended);
                            }
                        }
                    }
                    // 마스크 비트맵 초기화
                    using (var g = Graphics.FromImage(_maskBitmap))
                    {
                        g.Clear(Color.White);
                    }
                }
            }
            finally
            {
                _bitmapModel.Unlock();
            }
        }

        private Rectangle UpdateDirtyRect(Rectangle current, Rectangle newArea)
        {
            return current == Rectangle.Empty ? newArea : Rectangle.Union(current, newArea);
        }

    }
}
