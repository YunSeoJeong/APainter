using System;
using Painter.Interfaces;
using Painter.Models;

namespace Painter.Presenters
{
    public class ToolboxPresenter
    {
        private readonly IToolboxView? _view;
        private readonly IPainterSettingsModel? _settingsModel;

        public ToolboxPresenter(IToolboxView? view, IPainterSettingsModel? settingsModel)
        {
            _view = view;
            _settingsModel = settingsModel;
            SubscribeToEvents();
            SubscribeToSettingsEvents();
        }

        private void SubscribeToEvents()
        {
            _view!.BrushSelected += OnBrushSelected;
            _view!.PencilSelected += OnPencilSelected;
            _view!.EraserSelected += OnEraserSelected;
        }

        private void SubscribeToSettingsEvents()
        {
            if (_settingsModel != null)
            {
                _settingsModel.ToolChanged += OnToolChanged;
            }
        }

        public void OnToolChanged()
        {
            if (_settingsModel != null && _view != null)
            {
                _view.SetActiveTool(_settingsModel.CurrentTool);
            }
        }

        public void OnBrushSelected(object? sender, EventArgs e)
        {
            _settingsModel!.SetTool(ToolType.Brush);
        }

        public void OnPencilSelected(object? sender, EventArgs e)
        {
            _settingsModel!.SetTool(ToolType.Pencil);
        }

        public void OnEraserSelected(object? sender, EventArgs e)
        {
            _settingsModel!.SetTool(ToolType.Eraser);
        }
    }
}