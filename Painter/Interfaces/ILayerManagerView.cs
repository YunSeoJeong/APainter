using System;
using System.Collections.Generic;

namespace Painter.Interfaces
{
    public interface ILayerManagerView : IView
    {
        event EventHandler AddLayerClicked;
        void UpdateLayerList(List<string> layerNames);
        void ShowError(string message); // 오류 표시 메서드 추가
    }
}