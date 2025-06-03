using Painter.Presenters;
using System;
using System.Windows.Forms;

namespace Painter.Views
{
    public class MenuView
    {
        // 필드
        private MenuPresenter _menuPresenter; // 메뉴 Presenter
        private ToolStripMenuItem _fileSaveMenuItem; // 파일 저장 메뉴
        // ... (기타 메뉴 항목)

        /// <summary>
        /// UI 초기화
        /// </summary>
        public void InitializeComponent() { throw new NotImplementedException(); }

        /// <summary>
        /// 파일 저장 메뉴 클릭 이벤트 처리
        /// </summary>
        /// <param name="sender">이벤트 발생 객체</param>
        /// <param name="e">이벤트 인자</param>
        public void OnFileSaveMenuItemClicked(object sender, EventArgs e) { throw new NotImplementedException(); }
    }
}