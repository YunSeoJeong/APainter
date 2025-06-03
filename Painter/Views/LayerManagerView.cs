using Painter.Presenters;
using System;
using System.Windows.Forms;

namespace Painter.Views
{
    public class LayerManagerView
    {
        // 필드
        private LayerManagerPresenter _layerManagerPresenter; // 레이어 관리 View
        private ListBox _layerListBox; // 레이어 목록 표시
        private Button _btnAddLayer; // 레이어 추가 버튼
        // ... (기타 레이어 관리 버튼)

        /// <summary>
        /// UI 초기화
        /// </summary>
        public void InitializeComponent() { throw new NotImplementedException(); }

        /// <summary>
        /// 레이어 추가 버튼 클릭 이벤트 처리
        /// </summary>
        /// <param name="sender">이벤트 발생 객체</param>
        /// <param name="e">이벤트 인자</param>
        public void OnAddLayerButtonClicked(object sender, EventArgs e) { throw new NotImplementedException(); }
    }
}