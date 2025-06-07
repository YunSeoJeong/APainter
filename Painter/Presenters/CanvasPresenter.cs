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
            
            // 도구 변경 이벤트 구독 추가
            _settingsModel.ToolChanged += OnToolChanged;
            
            // 초기 비트맵 설정
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
        }
        
        private void DrawLine(Point start, Point end)
        {
            _bitmapModel.Lock();
            try
            {
                Console.WriteLine($"Using tool: {_settingsModel.CurrentTool}");
                var toolStrategy = _toolStrategyFactory.CreateToolStrategy(_settingsModel.CurrentTool);
                var context = new DrawingContext(
                    start,
                    end,
                    _settingsModel.PrimaryColor,
                    _settingsModel.BrushSize,
                    (x, y, color) => _bitmapModel.SetPixel(x, y, color),
                    (x, y) => _bitmapModel.GetPixel(x, y), // GetPixel 메서드 추가
                    1.0f // 기본 투명도
                );
                toolStrategy.Draw(context);
            }
            finally
            {
                _bitmapModel.Unlock();
                UpdateView();
            }
        }

        // 도구 변경 핸들러 추가
        private void OnToolChanged()
        {
            System.Diagnostics.Debug.WriteLine($"Tool changed to: {_settingsModel.CurrentTool}");
        }

        public void UpdateView()
        {
            _view.SetBitmap(_bitmapModel.GetBitmap());
        }
    }
}
