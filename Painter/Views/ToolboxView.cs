using Painter.Presenters;
using System;
using System.Windows.Forms;

namespace Painter.Views
{
    public class ToolboxView
    {
        // 필드
        private ToolboxPresenter _toolboxPresenter; // 도구 상자 Presenter
        private Button _btnBrush; // 붓 도구 버튼
        private Button _btnPencil; // 연필 도구 버튼
        // ... (기타 도구 버튼)

        /// <summary>
        /// UI 초기화
        /// </summary>
        public void InitializeComponent() { throw new NotImplementedException(); }

        /// <summary>
        /// 붓 도구 버튼 클릭 이벤트 처리
        /// </summary>
        /// <param name="sender">이벤트 발생 객체</param>
        /// <param name="e">이벤트 인자</param>
        public void OnBrushButtonClicked(object sender, EventArgs e) { throw new NotImplementedException(); }
    }
}