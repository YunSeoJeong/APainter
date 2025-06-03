using System;
using System.Windows.Forms;
using Painter.Interfaces;
using Painter.Presenters;

namespace Painter.Views
{
    /// <summary>MenuView 클래스</summary>
    public class MenuView : MenuStrip, IMenuView
    {
        public ToolStripMenuItem? FileSaveMenuItem { get; private set; }
        public event EventHandler? FileSaveClicked;

        /// <summary>MenuView 생성자</summary>
        public MenuView()
        {
            Initialize();
        }

        /// <summary>메뉴 UI 초기화</summary>
        public void Initialize()
        {
            // 파일 메뉴 생성
            var fileMenu = new ToolStripMenuItem("File");
            
            // 저장 메뉴 항목 생성
            FileSaveMenuItem = new ToolStripMenuItem("Save");
            FileSaveMenuItem.Click += OnFileSaveClicked;
            
            fileMenu.DropDownItems.Add(FileSaveMenuItem);
            Items.Add(fileMenu);
        }

        private void OnFileSaveClicked(object? sender, EventArgs e)
        {
            FileSaveClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}