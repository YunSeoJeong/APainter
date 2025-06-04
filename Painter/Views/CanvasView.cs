using System;
using System.Drawing;
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

        public event MouseEventHandler? MouseDownEvent;
        public event MouseEventHandler? MouseMoveEvent;
        public event MouseEventHandler? MouseUpEvent;

        /// <summary>CanvasView 생성자</summary>
        public CanvasView()
        {
            Initialize();
        }

        /// <summary>UI 컴포넌트 초기화</summary>
        public void Initialize()
        {
            PictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            // 마우스 이벤트 핸들러 연결 (변경: 람다식 대신 메서드 사용)
            PictureBox.MouseDown += PictureBox_MouseDown;
            PictureBox.MouseMove += PictureBox_MouseMove;
            PictureBox.MouseUp += PictureBox_MouseUp;

            Controls.Add(PictureBox);
        }

        /// <summary>비트맵을 UI 스레드에서 안전하게 설정</summary>
        public void SetBitmap(Bitmap bitmap)
        {
            _currentBitmap = bitmap; // 현재 비트맵 저장
            if (PictureBox != null && PictureBox.InvokeRequired)
            {
                PictureBox.Invoke(new Action(() => PictureBox.Image = bitmap));
            }
            else if (PictureBox != null)
            {
                PictureBox.Image = bitmap;
            }
        }

        // 뷰 좌표를 비트맵 좌표로 변환 (Zoom 모드 고려)
        private Point ViewToBitmap(Point viewPoint)
        {
            if (PictureBox == null || _currentBitmap == null || PictureBox.Image == null)
                return viewPoint;

            // PictureBox의 SizeMode가 Zoom인 경우 변환
            int pbWidth = PictureBox.Width;
            int pbHeight = PictureBox.Height;
            int imgWidth = _currentBitmap.Width;
            int imgHeight = _currentBitmap.Height;

            float imageRatio = (float)imgWidth / imgHeight;
            float controlRatio = (float)pbWidth / pbHeight;

            int imageWidth, imageHeight;
            if (imageRatio >= controlRatio)
            {
                imageWidth = pbWidth;
                imageHeight = (int)(imageWidth / imageRatio);
            }
            else
            {
                imageHeight = pbHeight;
                imageWidth = (int)(imageHeight * imageRatio);
            }

            int imageLeft = (pbWidth - imageWidth) / 2;
            int imageTop = (pbHeight - imageHeight) / 2;

            // 마우스 좌표가 이미지 영역 내에 있는지 확인
            if (viewPoint.X < imageLeft || viewPoint.X >= imageLeft + imageWidth || 
                viewPoint.Y < imageTop || viewPoint.Y >= imageTop + imageHeight)
            {
                return Point.Empty; // 영역 밖
            }

            int x = (int)((viewPoint.X - imageLeft) * ((float)imgWidth / imageWidth));
            int y = (int)((viewPoint.Y - imageTop) * ((float)imgHeight / imageHeight));
            return new Point(x, y);
        }

        private void PictureBox_MouseDown(object? sender, MouseEventArgs e)
        {
            var point = ViewToBitmap(e.Location);
            if (point != Point.Empty)
            {
                MouseDownEvent?.Invoke(sender, new MouseEventArgs(e.Button, e.Clicks, point.X, point.Y, e.Delta));
            }
        }

        private void PictureBox_MouseMove(object? sender, MouseEventArgs e)
        {
            var point = ViewToBitmap(e.Location);
            if (point != Point.Empty)
            {
                MouseMoveEvent?.Invoke(sender, new MouseEventArgs(e.Button, e.Clicks, point.X, point.Y, e.Delta));
            }
        }

        private void PictureBox_MouseUp(object? sender, MouseEventArgs e)
        {
            var point = ViewToBitmap(e.Location);
            if (point != Point.Empty)
            {
                MouseUpEvent?.Invoke(sender, new MouseEventArgs(e.Button, e.Clicks, point.X, point.Y, e.Delta));
            }
        }
    }
}