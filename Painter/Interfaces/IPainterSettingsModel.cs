using System;
using System.Drawing;

namespace Painter.Interfaces
{
    public interface IPainterSettingsModel
    {
        ToolType CurrentTool { get; }
        Color PrimaryColor { get; set; }
        int BrushSize { get; set; }
        
        event Action ToolChanged;
        event Action PrimaryColorChanged;
        event Action BrushSizeChanged;
        
        void SetTool(ToolType tool);
    }

    public enum ToolType
    {
        Brush,
        Pencil,
        Eraser
    }
}