using System;

namespace Painter.Interfaces
{
    public interface IToolboxView : IView
    {
        event EventHandler BrushSelected;
        event EventHandler PencilSelected;
        event EventHandler EraserSelected;
        
        void SetActiveTool(ToolType toolType);
    }
}