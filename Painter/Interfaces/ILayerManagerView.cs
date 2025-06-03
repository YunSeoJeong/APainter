using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Painter.Interfaces
{
    public interface ILayerManagerView : IView
    {
        event EventHandler AddLayerClicked;
        void UpdateLayerList(IEnumerable<string> layerNames);
    }
}