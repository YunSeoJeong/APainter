using System.Drawing;
using Painter.Interfaces;

namespace Painter.Presenters
{
    public class CanvasPresenter
    {
        private readonly ICanvasView _view;
        private readonly IBitmapModel _model;
        private Point _lastPoint;
        private bool _isDrawing = false;

        public CanvasPresenter(ICanvasView view, IBitmapModel model)
        {
            _view = view;
            _model = model;
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            _view.MouseDownEvent += OnMouseDown;
            _view.MouseMoveEvent += OnMouseMove;
            _view.MouseUpEvent += OnMouseUp;
        }

        public void OnMouseDown(object sender, MouseEventArgs e)
        {
            _isDrawing = true;
            _lastPoint = e.Location;
            _model.Lock();
        }

        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDrawing) return;
            
            // 간단한 선 그리기 구현
            using (var g = Graphics.FromImage(_model.GetBitmap()))
            {
                g.DrawLine(Pens.Black, _lastPoint, e.Location);
            }
            _lastPoint = e.Location;
            
            UpdateView();
        }

        public void OnMouseUp(object sender, MouseEventArgs e)
        {
            _isDrawing = false;
            _model.Unlock();
        }

        private void UpdateView()
        {
            _view.SetBitmap(_model.GetBitmap());
        }
    }
}