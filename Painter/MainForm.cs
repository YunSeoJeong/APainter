using System;
using System.Windows.Forms;
using Painter.Views;

namespace Painter
{
    public partial class MainForm : Form
    {
        // 필드
        private ToolboxView _toolboxView; // 도구 상자 View
        private CanvasView _canvasView; // 그림 영역 View
        private LayerManagerView _layerManagerView; // 레이어 관리 View
        private MenuView _menuView; // 메뉴 View

        public MainForm()
        {
            // 디자이너에서 UI 요소 초기화
        }
    }
}
