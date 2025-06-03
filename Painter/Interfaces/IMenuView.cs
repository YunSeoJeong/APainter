using System;

namespace Painter.Interfaces
{
    public interface IMenuView : IView
    {
        event EventHandler FileSaveClicked;
    }
}