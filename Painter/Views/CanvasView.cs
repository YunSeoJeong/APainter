using Painter.Presenters;
using System;
using System.Windows.Forms;

namespace Painter.Views
{
    public class CanvasView
    {
        // 필드
        private CanvasPresenter _canvasPresenter; // 그림 영역 Presenter
        private PictureBox _pictureBox; // 그림 표시 영역

        /// <summary>
        /// UI 초기화
        /// </summary>
        public void InitializeComponent() { throw new NotImplementedException(); }

        /// <summary>
        /// 마우스 다운 이벤트 처리
        /// </summary>
        /// <param name="sender">이벤트 발생 객체</param>
        /// <param name="e">이벤트 인자</param>
        public void OnMouseDown(object sender, MouseEventArgs e) { throw new NotImplementedException(); }

        /// <summary>
        /// 마우스 이동 이벤트 처리
        /// </summary>
        /// <param name="sender">이벤트 발생 객체</param>
        /// <param name="e">이벤트 인자</param>
        public void OnMouseMove(object sender, MouseEventArgs e) { throw new NotImplementedException(); }

        /// <summary>
        /// 마우스 업 이벤트 처리
        /// </summary>
        /// <param name="sender">이벤트 발생 객체</param>
        /// <param name="e">이벤트 인자</param>
        public void OnMouseUp(object sender, MouseEventArgs e) { throw new NotImplementedException(); }
    }
}