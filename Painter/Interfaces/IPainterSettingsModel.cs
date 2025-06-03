using System;

namespace Painter.Interfaces
{
    public interface IPainterSettingsModel
    {
        ToolType CurrentTool { get; }
        Color PrimaryColor { get; set; }
        int BrushSize { get; set; }
        event Action ToolChanged;
        void SetTool(ToolType tool);
    }

    public enum ToolType
    {
        Brush,
        Pencil,
        Eraser
    }
}