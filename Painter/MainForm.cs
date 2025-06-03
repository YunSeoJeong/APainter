using System;
using System.Drawing;
using System.Windows.Forms;
using Painter.Interfaces;
using Painter.Views;

namespace Painter
{
    public partial class MainForm : Form, IMainView
    {
        private readonly IToolboxView _toolboxView;
        private readonly ICanvasView _canvasView;
        private readonly ILayerManagerView _layerManagerView;
        private readonly IMenuView _menuView;

        public MainForm(
            IToolboxView toolboxView,
            ICanvasView canvasView,
            ILayerManagerView layerManagerView,
            IMenuView menuView)
        {
            _toolboxView = toolboxView;
            _canvasView = canvasView;
            _layerManagerView = layerManagerView;
            _menuView = menuView;
            
            Initialize();
            AddViewsToForm();
        }

        public void Initialize()
        {
            // 기본 폼 설정
            Text = "APainter";
            Size = new Size(1200, 800);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(45, 45, 48); // 다크 모드 배경
        }

        private void AddViewsToForm()
        {
            if (_menuView is Control menuControl)
            {
                menuControl.Dock = DockStyle.Top;
                Controls.Add(menuControl);
            }
            
            // 메인 패널 (도구상자 + 캔버스)
            var mainPanel = new Panel { Dock = DockStyle.Fill };
            
            if (_toolboxView is Control toolboxControl)
            {
                toolboxControl.Width = 200;
                toolboxControl.Dock = DockStyle.Left;
                mainPanel.Controls.Add(toolboxControl);
            }
            
            if (_canvasView is Control canvasControl)
            {
                canvasControl.Dock = DockStyle.Fill;
                mainPanel.Controls.Add(canvasControl);
            }
            
            Controls.Add(mainPanel);
            
            if (_layerManagerView is Control layerManagerControl)
            {
                layerManagerControl.Width = 250;
                layerManagerControl.Dock = DockStyle.Right;
                Controls.Add(layerManagerControl);
            }
        }
    }
}
