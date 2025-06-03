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
        }

        public void OnMouseDown(object? sender, MouseEventArgs e)
        {
            _lastPoint = e.Location;
        }

        public void OnMouseMove(object? sender, MouseEventArgs e)
        {
            if (_lastPoint.HasValue && e.Button == MouseButtons.Left)
            {
                _bitmapModel.Lock();
                try
                {
                    var toolStrategy = _toolStrategyFactory.CreateToolStrategy(_settingsModel.CurrentTool);
                    var context = new DrawingContext(
                        _lastPoint.Value, 
                        e.Location,
                        _settingsModel.PrimaryColor,
                        _settingsModel.BrushSize,
                        (x, y, color) => _bitmapModel.SetPixel(x, y, color)
                    );
                    toolStrategy.Draw(context);
                }
                finally
                {
                    _bitmapModel.Unlock();
                    UpdateView();
                }
            }
        }

        public void OnMouseUp(object? sender, MouseEventArgs e)
        {
            _lastPoint = null;
        }

        public void UpdateView()
        {
            _view.SetBitmap(_bitmapModel.GetBitmap());
        }
    }
}