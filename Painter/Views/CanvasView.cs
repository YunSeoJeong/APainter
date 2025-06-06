using System;
using System.Drawing;
using System.Drawing.Drawing2D; // Matrix 클래스를 위해 추가
using System.Windows.Forms;
using Painter.Interfaces;
using Painter.Presenters;

namespace Painter.Views
{
    /// <summary>CanvasView 클래스</summary>
    public class CanvasView : UserControl, ICanvasView
    {
        public PictureBox? PictureBox { get; private set; }
        private Bitmap? _currentBitmap;
        private float zoom = 1.0f; // 확대/축소 배율 (1.0 = 100%)
        private PointF pan = new PointF(0, 0); // pan 벡터 (이동 벡터)
        private Matrix _transform; // 변환 행렬
        private bool _isPanning = false;
        private Point _lastPanPoint;

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
                BackColor = Color.White,
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

        // 뷰 좌표를 비트맵 좌표로 변환 (변환 행렬 적용)
        private Point ViewToBitmap(Point viewPoint)
        {
            if (PictureBox == null || _currentBitmap == null)
                return viewPoint;

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
            if (PictureBox == null || _currentBitmap == null) return;

            // 원본 픽셀을 선명하게 보존하기 위한 그래픽스 설정
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;

            e.Graphics.Transform = _transform;
            e.Graphics.DrawImage(_currentBitmap, new Point(0, 0));
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