using Painter.Models;
using Painter.Views;
using System;
using System.Windows.Forms;

namespace Painter.Presenters
{
    public class MenuPresenter
    {
        // 필드
        private FileModel _fileModel; // 파일 Model
        private MenuView _menuView; // 메뉴 View

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="fileModel">FileModel</param>
        /// <param name="menuView">MenuView</param>
        public MenuPresenter(FileModel fileModel, MenuView menuView) { throw new NotImplementedException(); }

        /// <summary>
        /// 파일 저장 메뉴 클릭 처리
        /// </summary>
        public void OnFileSaveMenuItemClicked() { throw new NotImplementedException(); }
        // ... (기타 메뉴 항목 클릭 처리)
    }
}