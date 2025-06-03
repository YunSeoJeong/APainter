using Painter.Models;
using Painter.Views;
using System;

namespace Painter.Presenters
{
    public class CanvasPresenter
    {
        // 필드
        private BitmapModel _bitmapModel; // 그림 데이터 Model
        private CanvasView _canvasView; // 그림 영역 View

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="bitmapModel">BitmapModel</param>
        /// <param name="canvasView">CanvasView</param>
        public CanvasPresenter(BitmapModel bitmapModel, CanvasView canvasView) { throw new NotImplementedException(); }

        /// <summary>
        /// 마우스 다운 이벤트 처리
        /// </summary>
        /// <param name="x">X 좌표</param>
        /// <param name="y">Y 좌표</param>
        public void OnMouseDown(int x, int y) { throw new NotImplementedException(); }

        /// <summary>
        /// 마우스 이동 이벤트 처리
        /// </summary>
        /// <param name="x">X 좌표</param>
        /// <param name="y">Y 좌표</param>
        public void OnMouseMove(int x, int y) { throw new NotImplementedException(); }

        /// <summary>
        /// 마우스 업 이벤트 처리
        /// </summary>
        /// <param name="x">X 좌표</param>
        /// <param name="y">Y 좌표</param>
        public void OnMouseUp(int x, int y) { throw new NotImplementedException(); }
    }
}