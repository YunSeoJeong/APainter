using System;
using Painter.Models; // BitmapModel 클래스를 사용하기 위해 추가
using Painter.Views;

namespace Painter
{
    public class ToolboxPresenter
    {
        private BitmapModel _bitmapModel; // 그림 데이터 Model
        private ToolboxView _toolboxView; // 도구 상자 View

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="bitmapModel">BitmapModel</param>
        /// <param name="toolboxView">ToolboxView</param>
        public ToolboxPresenter(BitmapModel bitmapModel, ToolboxView toolboxView) { throw new NotImplementedException(); }

        /// <summary>
        /// 붓 도구 클릭 처리
        /// </summary>
        public void OnBrushButtonClicked() { throw new NotImplementedException(); }
        // ... (기타 도구 클릭 처리)
    }
}