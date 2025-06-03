using System;
using System.Drawing;
using System.Windows.Forms;

namespace Painter.Interfaces
{
    public interface ICanvasView
    {
        void Initialize();
        void SetBitmap(Bitmap bitmap);
        event MouseEventHandler MouseDownEvent;
        event MouseEventHandler MouseMoveEvent;
        event MouseEventHandler MouseUpEvent;
    }
}