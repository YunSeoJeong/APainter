using System;

namespace Painter.Interfaces
{
    public interface IToolboxView : IView
    {
        event EventHandler BrushSelected;
        event EventHandler PencilSelected;
        event EventHandler EraserSelected;
        event EventHandler SpraySelected;
        
        void SetActiveTool(ToolType toolType);
    }
}