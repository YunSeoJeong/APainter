using System;
using System.Drawing;
using System.Windows.Forms;
using Painter.Interfaces;
using Painter.Models;

namespace Painter.Presenters
{
    public class CanvasPresenter
    {
        private readonly ICanvasView _view;
        private readonly IBitmapModel _bitmapModel;
        private readonly IPainterSettingsModel _settingsModel;
        private Point? _lastPoint;

        public CanvasPresenter(ICanvasView view, IBitmapModel bitmapModel, IPainterSettingsModel settingsModel)
        {
            _view = view;
            _bitmapModel = bitmapModel;
            _settingsModel = settingsModel;

            _view.MouseDownEvent += OnMouseDown;
            _view.MouseMoveEvent += OnMouseMove;
            _view.MouseUpEvent += OnMouseUp;
        }

        public void OnMouseDown(object? sender, MouseEventArgs e)
        {
            _lastPoint = e.Location;
        }

        public void OnMouseMove(object? sender, MouseEventArgs e)
        {
            if (_lastPoint.HasValue && e.Button == MouseButtons.Left)
            {
                DrawLine(e.Location, _settingsModel.CurrentTool);
                _lastPoint = e.Location;
            }
        }

        public void OnMouseUp(object? sender, MouseEventArgs e)
        {
            _lastPoint = null;
        }

        private void DrawLine(Point currentPoint, ToolType toolType)
        {
            if (!_lastPoint.HasValue) return;

            _bitmapModel.Lock();
            try
            {
                switch (toolType)
                {
                    case ToolType.Brush:
                        DrawLine(currentPoint, _settingsModel.PrimaryColor, _settingsModel.BrushSize);
                        break;
                    case ToolType.Pencil:
                        DrawLine(currentPoint, Color.Black, 1);
                        break;
                    case ToolType.Eraser:
                        DrawLine(currentPoint, Color.White, _settingsModel.BrushSize);
                        break;
                }
            }
            finally
            {
                _bitmapModel.Unlock();
                UpdateView();
            }
        }

        private void DrawLine(Point currentPoint, Color color, int size)
        {
            // 선 그리기 로직 구현 (간소화)
            int x1 = _lastPoint!.Value.X;
            int y1 = _lastPoint!.Value.Y;
            int x2 = currentPoint.X;
            int y2 = currentPoint.Y;

            // 실제 선 그리기 알고리즘 구현 필요
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    _bitmapModel.SetPixel(x2 + i, y2 + j, color);
                }
            }
        }

        public void UpdateView()
        {
            _view.SetBitmap(_bitmapModel.GetBitmap());
        }
    }
}