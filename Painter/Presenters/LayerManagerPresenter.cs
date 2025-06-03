using Painter.Models;
using Painter.Views;
using System;

namespace Painter.Presenters
{
    public class LayerManagerPresenter
    {
        // 필드
        private BitmapModel _bitmapModel; // 그림 데이터 Model
        private LayerManagerView _layerManagerView; // 레이어 관리 View

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="bitmapModel">BitmapModel</param>
        /// <param name="layerManagerView">LayerManagerView</param>
        public LayerManagerPresenter(BitmapModel bitmapModel, LayerManagerView layerManagerView) { throw new NotImplementedException(); }

        /// <summary>
        /// 레이어 추가 버튼 클릭 처리
        /// </summary>
        public void OnAddLayerButtonClicked() { throw new NotImplementedException(); }
        // ... (기타 레이어 관리 버튼 클릭 처리)
    }
}