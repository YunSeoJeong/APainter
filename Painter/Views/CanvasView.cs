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
        public event MouseEventHandler? MouseDownEvent;
        public event MouseEventHandler? MouseMoveEvent;
        public event MouseEventHandler? MouseUpEvent;

        private CanvasPresenter? _presenter;

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

            // 마우스 이벤트 핸들러 연결
            PictureBox.MouseDown += (s, e) => MouseDownEvent?.Invoke(s, e);
            PictureBox.MouseMove += (s, e) => MouseMoveEvent?.Invoke(s, e);
            PictureBox.MouseUp += (s, e) => MouseUpEvent?.Invoke(s, e);

            Controls.Add(PictureBox);
        }

        /// <summary>프레젠터 설정</summary>
        public void SetPresenter(CanvasPresenter presenter)
        {
            _presenter = presenter;
        }

        /// <summary>비트맵을 UI 스레드에서 안전하게 설정</summary>
        public void SetBitmap(Bitmap bitmap)
        {
            if (PictureBox != null && PictureBox.InvokeRequired)
            {
                PictureBox.Invoke(new Action(() => PictureBox.Image = bitmap));
            }
            else if (PictureBox != null)
            {
                PictureBox.Image = bitmap;
            }
        }
    }
}