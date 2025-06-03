using System;
using System.Drawing;
using System.Windows.Forms;
using Painter.Interfaces;
using Painter.Presenters;

namespace Painter.Views
{
    public class CanvasView : UserControl, ICanvasView
    {
        public PictureBox PictureBox { get; private set; }
        public event MouseEventHandler MouseDownEvent;
        public event MouseEventHandler MouseMoveEvent;
        public event MouseEventHandler MouseUpEvent;

        private CanvasPresenter _presenter;

        public CanvasView()
        {
            Initialize();
        }

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

        public void SetPresenter(CanvasPresenter presenter)
        {
            _presenter = presenter;
        }

        public void SetBitmap(Bitmap bitmap)
        {
            if (PictureBox.InvokeRequired)
            {
                PictureBox.Invoke(new Action(() => PictureBox.Image = bitmap));
            }
            else
            {
                PictureBox.Image = bitmap;
            }
        }
    }
}