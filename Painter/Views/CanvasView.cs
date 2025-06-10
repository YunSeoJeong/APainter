using System;
using System.Drawing;
using System.Drawing.Drawing2D; // Matrix 클래스를 위해 추가
using System.Drawing.Imaging; // PixelFormat, ImageLockMode 추가
using System.Windows.Forms;
using Painter.Interfaces;
using Painter.Presenters;
using Painter.Utils; // GridBackground 사용을 위한 네임스페이스 추가

namespace Painter.Views
{
    /// <summary>CanvasView 클래스</summary>
    public class CanvasView : UserControl, ICanvasView
    {
        public PictureBox? PictureBox { get; private set; }
        private Bitmap? _currentBitmap;
        private Bitmap? _tempBitmap; // 임시 비트맵 (완전 투명 초기값)
        private Bitmap? _maskBitmap; // 마스크 비트맵 (완전 불투명 초기값)
        private Bitmap? _gridBackground; // 그리드 배경 비트맵
        private Size _lastGridSize; // 마지막으로 생성된 그리드 크기
        private float zoom = 1.0f; // 확대/축소 배율 (1.0 = 100%)
        private PointF pan = new PointF(0, 0); // pan 벡터 (이동 벡터)
        private Matrix _transform; // 변환 행렬
        private bool _isPanning = false;
        private Point _lastPanPoint;
        private Rectangle _dirtyRect = Rectangle.Empty; // 변경된 영역 추적
        private readonly object _dirtyLock = new object(); // _dirtyRect 스레드 동기화 객체

        /// <summary>현재 확대/축소 배율</summary>
        public float Zoom => zoom;

        /// <summary>현재 이동 벡터</summary>
        public PointF Pan => pan;

        public event MouseEventHandler? MouseDownEvent;
        public event MouseEventHandler? MouseMoveEvent;
        public event MouseEventHandler? MouseUpEvent;

        /// <summary>CanvasView 생성자</summary>
        public CanvasView()
        {
            _transform = new Matrix(); // 생성자에서 초기화
            Initialize();
        }

        /// <summary>UI 컴포넌트 초기화</summary>
        public void Initialize()
        {
            PictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 240, 240), // 연한 회색 배경으로 변경
                SizeMode = PictureBoxSizeMode.Normal // Zoom 모드에서 Normal로 변경
            };

            // 마우스 이벤트 핸들러 연결 (변경: 람다식 대신 메서드 사용)
            PictureBox.MouseDown += PictureBox_MouseDown;
            PictureBox.MouseMove += PictureBox_MouseMove;
            PictureBox.MouseUp += PictureBox_MouseUp;
            PictureBox.MouseWheel += PictureBox_MouseWheel; // 마우스 휠 이벤트 추가
            PictureBox.Paint += PictureBox_Paint; // Paint 이벤트 추가

            Controls.Add(PictureBox);
        }

        /// <summary>비트맵을 UI 스레드에서 안전하게 설정</summary>
        public void SetBitmap(Bitmap bitmap)
        {
            _currentBitmap = bitmap; // 현재 비트맵 저장
            _tempBitmap = null;
            _maskBitmap = null;
            
            // 크기가 변경된 경우에만 그리드 배경 재생성
            if (_gridBackground == null || _lastGridSize != bitmap.Size)
            {
                if (_gridBackground != null)
                {
                    _gridBackground.Dispose();
                }
                _gridBackground = GridBackground.GenerateGrid(bitmap.Width, bitmap.Height);
                _lastGridSize = bitmap.Size;
            }
            
            if (PictureBox != null)
            {
                if (PictureBox.InvokeRequired)
                {
                    PictureBox.Invoke(new Action(() => PictureBox.Invalidate()));
                }
                else
                {
                    PictureBox.Invalidate();
                }
            }
        }

        /// <summary>합성 비트맵 설정</summary>
        private ToolType _currentTool; // 현재 도구 상태 저장

        public void SetCompositeBitmap(Bitmap mainBitmap, Bitmap? tempBitmap, Bitmap? maskBitmap, ToolType tool, Rectangle dirtyRect)
        {
            _currentBitmap = mainBitmap;
            _tempBitmap = tempBitmap;
            _maskBitmap = maskBitmap;
            _currentTool = tool; // 도구 상태 업데이트
            lock (_dirtyLock)
            {
                _dirtyRect = dirtyRect; // 변경 영역 저장
            }
            
            if (PictureBox != null)
            {
                if (PictureBox.InvokeRequired)
                {
                    PictureBox.Invoke(new Action(() => PictureBox.Invalidate(_dirtyRect)));
                }
                else
                {
                    PictureBox.Invalidate(_dirtyRect);
                }
            }
            lock (_dirtyLock)
            {
                _dirtyRect = Rectangle.Empty; // 변경 영역 사용 후 즉시 초기화
            }
        }

        // 뷰 좌표를 비트맵 좌표로 변환 (변환 행렬 적용)
        private Point ViewToBitmap(Point viewPoint)
        {
            if (PictureBox == null || _currentBitmap == null)
                return viewPoint;

            // 경계 검사 추가
            if (viewPoint.X < 0 || viewPoint.Y < 0 ||
                viewPoint.X >= PictureBox.Width || viewPoint.Y >= PictureBox.Height)
            {
                return Point.Empty;
            }

            // 변환 행렬의 역행렬을 사용해 뷰 좌표를 비트맵 좌표로 변환
            Matrix inverse = _transform.Clone();
            inverse.Invert();
            PointF[] points = { new PointF(viewPoint.X, viewPoint.Y) };
            inverse.TransformPoints(points);
            return new Point((int)points[0].X, (int)points[0].Y);
        }

        // 변환 행렬을 업데이트하는 메서드
        private void UpdateTransform()
        {
            _transform.Reset();
            _transform.Translate(pan.X, pan.Y);
            _transform.Scale(zoom, zoom);
        }

        private void PictureBox_MouseDown(object? sender, MouseEventArgs e)
        {
            // 휠 버튼(가운데 버튼)으로 패닝 시작
            if (e.Button == MouseButtons.Middle)
            {
                _isPanning = true;
                _lastPanPoint = e.Location;
                if (PictureBox != null)
                {
                    PictureBox.Cursor = Cursors.Hand;
                }
                return;
            }

            var point = ViewToBitmap(e.Location);
            if (point != Point.Empty)
            {
                MouseDownEvent?.Invoke(sender, new MouseEventArgs(e.Button, e.Clicks, point.X, point.Y, e.Delta));
            }
        }

        private void PictureBox_MouseMove(object? sender, MouseEventArgs e)
        {
            if (_isPanning)
            {
                int deltaX = e.Location.X - _lastPanPoint.X;
                int deltaY = e.Location.Y - _lastPanPoint.Y;

                pan.X += deltaX;
                pan.Y += deltaY;

                _lastPanPoint = e.Location;

                UpdateTransform();
                if (PictureBox != null)
                {
                    PictureBox.Invalidate();
                }
                return;
            }

            var point = ViewToBitmap(e.Location);
            if (point != Point.Empty)
            {
                MouseMoveEvent?.Invoke(sender, new MouseEventArgs(e.Button, e.Clicks, point.X, point.Y, e.Delta));
            }
        }

        private void PictureBox_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle && _isPanning)
            {
                _isPanning = false;
                if (PictureBox != null)
                {
                    PictureBox.Cursor = Cursors.Default;
                }
                return;
            }

            var point = ViewToBitmap(e.Location);
            if (point != Point.Empty)
            {
                MouseUpEvent?.Invoke(sender, new MouseEventArgs(e.Button, e.Clicks, point.X, point.Y, e.Delta));
            }
        }
        
        // Paint 이벤트 핸들러: 변환 행렬 적용 및 픽셀 보간 설정
        private void PictureBox_Paint(object? sender, PaintEventArgs e)
        {
            if (PictureBox == null || _currentBitmap == null || _gridBackground == null) return;

            // 원본 픽셀을 선명하게 보존하기 위한 그래픽스 설정
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;

            e.Graphics.Transform = _transform;

            // 그리드 배경 렌더링
            e.Graphics.DrawImage(_gridBackground, new Point(0, 0));

            // 메인 비트맵 렌더링
            if (_currentTool == ToolType.Eraser && _maskBitmap != null)
            {
                // 지우개 도구: Min 함수 기반 합성
                // DirtyRect가 비어있으면 전체 영역 사용
                // Clamp the rectangle to bitmap boundaries
                // 경계 검사 강화: 음수 좌표 방지 및 스레드 안전성 확보
                Rectangle rect;
                lock (_dirtyLock)
                {
                    if (_dirtyRect.IsEmpty)
                    {
                        rect = new Rectangle(0, 0, _currentBitmap.Width, _currentBitmap.Height);
                    }
                    else
                    {
                        // 비트맵 경계 내로 dirtyRect 클램핑
                        Rectangle bounds = new Rectangle(0, 0, _currentBitmap.Width, _currentBitmap.Height);
                        rect = Rectangle.Intersect(_dirtyRect, bounds);
                        
                        // 교차 영역이 없으면 전체 영역 사용
                        if (rect.IsEmpty)
                        {
                            rect = new Rectangle(0, 0, _currentBitmap.Width, _currentBitmap.Height);
                        }
                    }
                }
                
                // 추가 경계 검사
                if (rect.Width <= 0 || rect.Height <= 0)
                {
                    return;
                }
                    
                // DirtyRect 영역만큼의 비트맵 생성
                using (var composite = new Bitmap(rect.Width, rect.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                {
                    // Create a new rectangle for the compositing operation that matches the composite bitmap size
                    var compositeRect = new Rectangle(0, 0, rect.Width, rect.Height);
                    
                    var mainData = _currentBitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    var maskData = _maskBitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    var compositeData = composite.LockBits(compositeRect, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    unsafe
                    {
                        byte* mainPtr = (byte*)mainData.Scan0;
                        byte* maskPtr = (byte*)maskData.Scan0;
                        byte* compPtr = (byte*)compositeData.Scan0;

                        int bitmapWidth = _currentBitmap.Width;
                        int bitmapHeight = _currentBitmap.Height;
                        int mainStride = mainData.Stride;
                        int compositeStride = compositeData.Stride;
                        
                        
                        for (int y = 0; y < rect.Height; y++)
                        {
                            int srcY = rect.Top + y;
                            
                            // Skip if out of bounds (shouldn't happen after clamping, but just in case)
                            if (srcY < 0 || srcY >= bitmapHeight) continue;
                            
                            for (int x = 0; x < rect.Width; x++)
                            {
                                int srcX = rect.Left + x;
                                if (srcX < 0 || srcX >= bitmapWidth) continue;
                                
                                // Validate pixel offset
                                int srcOffset = srcY * mainStride + srcX * 4;
                                if (srcOffset < 0 || srcOffset + 3 >= mainStride * bitmapHeight) continue;
                                
                                int destOffset = y * compositeStride + x * 4;
                                if (destOffset < 0 || destOffset + 3 >= compositeStride * rect.Height) continue;
                                
                                // Get alpha values using validated offsets
                                byte mainA = mainPtr[srcOffset + 3];
                                byte maskA = maskPtr[srcOffset + 3];
                                byte newA = Math.Min(mainA, maskA);

                                // RGB는 메인 비트맵 유지
                                compPtr[destOffset] = mainPtr[srcOffset];     // B
                                compPtr[destOffset + 1] = mainPtr[srcOffset + 1]; // G
                                compPtr[destOffset + 2] = mainPtr[srcOffset + 2]; // R
                                compPtr[destOffset + 3] = newA;         // A
                            }
                        }
                    }

                    _currentBitmap.UnlockBits(mainData);
                    _maskBitmap.UnlockBits(maskData);
                    composite.UnlockBits(compositeData);

                    e.Graphics.DrawImage(composite, new Point(rect.Left, rect.Top));
                }
            }
            else if (_tempBitmap != null)
            {
                // 브러시/펜슬 도구: 표준 알파 블렌딩
                using (var composite = new Bitmap(_currentBitmap.Width, _currentBitmap.Height))
                using (var g = Graphics.FromImage(composite))
                {
                    g.DrawImage(_currentBitmap, Point.Empty);
                    g.DrawImage(_tempBitmap, Point.Empty);
                    e.Graphics.DrawImage(composite, Point.Empty);
                }
            }
            else
            {
                // 일반 메인 비트맵 렌더링
                e.Graphics.DrawImage(_currentBitmap, Point.Empty);
            }

            // 캔버스 경계선 그리기 (확대율 반영) - 점선 제거하고 실선으로 변경
            // 클리핑 없이 항상 전체 캔버스 경계선을 그림
            float borderWidth = 2f * zoom; // 확대율에 따라 두께 조절
            using (Pen borderPen = new Pen(Color.DarkGray, borderWidth))
            {
                // 임시로 클리핑 해제 후 경계선 그림
                var oldClip = e.Graphics.Clip;
                e.Graphics.ResetClip();
                
                e.Graphics.DrawRectangle(
                    borderPen,
                    new Rectangle(0, 0, _currentBitmap.Width - 1, _currentBitmap.Height - 1)
                );
                
                // 원래 클리핑 영역 복원
                e.Graphics.Clip = oldClip;
            }
        }

        // 마우스 휠 이벤트 핸들러 (새 로직 적용)
        private void PictureBox_MouseWheel(object? sender, MouseEventArgs e)
        {
            if (PictureBox == null || _currentBitmap == null) return;

            float oldZoom = zoom;
            float zoomDelta = 0.1f;
            if (e.Delta > 0)
            {
                zoom += zoomDelta; // 확대
            }
            else
            {
                zoom -= zoomDelta; // 축소
            }
            zoom = Math.Max(0.1f, Math.Min(5f, zoom)); // 제한

            // 마우스 위치 기준으로 확대 위치 보정
            var mouseWorldBefore = new PointF(
                (e.X - pan.X) / oldZoom,
                (e.Y - pan.Y) / oldZoom
            );

            var mouseWorldAfter = new PointF(
                (e.X - pan.X) / zoom,
                (e.Y - pan.Y) / zoom
            );

            pan.X += (mouseWorldAfter.X - mouseWorldBefore.X) * zoom;
            pan.Y += (mouseWorldAfter.Y - mouseWorldBefore.Y) * zoom;

            // 변환 행렬 업데이트: 이동 -> 확대 순서
            UpdateTransform();
            PictureBox.Invalidate();
        }

    }
}