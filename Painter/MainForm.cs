using System;
using System.Drawing;
using System.Windows.Forms;
using Painter.Views;

namespace Painter
{
    public class MainForm : Form
    {
        private ToolboxView _toolboxView;
        private CanvasView _canvasView;
        private LayerManagerView _layerManagerView;
        private MenuView _menuView;

        public MainForm()
        {
            Initialize();
        }

        private void Initialize()
        {
            // 기본 폼 설정
            Text = "APainter";
            Size = new Size(1200, 800);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(45, 45, 48); // 다크 모드 배경
            
            // 메뉴 뷰 (상단 고정)
            _menuView = new MenuView();
            _menuView.Dock = DockStyle.Top;
            Controls.Add(_menuView);
            
            // 메인 패널 (도구상자 + 캔버스)
            var mainPanel = new Panel { Dock = DockStyle.Fill };
            
            // 도구 상자 (좌측 200px 고정)
            _toolboxView = new ToolboxView();
            _toolboxView.Width = 200;
            _toolboxView.Dock = DockStyle.Left;
            mainPanel.Controls.Add(_toolboxView);
            
            // 캔버스 영역 (나머지 공간)
            _canvasView = new CanvasView();
            _canvasView.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(_canvasView);
            
            Controls.Add(mainPanel);
            
            // 레이어 관리자 (우측 250px 고정)
            _layerManagerView = new LayerManagerView();
            _layerManagerView.Width = 250;
            _layerManagerView.Dock = DockStyle.Right;
            Controls.Add(_layerManagerView);
        }
    }
}
