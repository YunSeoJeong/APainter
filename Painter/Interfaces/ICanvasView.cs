using System;
using System.Drawing;
using System.Windows.Forms;

namespace Painter.Interfaces
{
    public interface ICanvasView
    {
        void Initialize();
        void SetBitmap(Bitmap bitmap);
        void SetCompositeBitmap(Bitmap mainBitmap, Bitmap? tempBitmap, Bitmap? maskBitmap, ToolType tool);
        event MouseEventHandler MouseDownEvent;
        event MouseEventHandler MouseMoveEvent;
        event MouseEventHandler MouseUpEvent;
    }
}