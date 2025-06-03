using Painter.Models;
using Painter.Views;
using System;

namespace Painter.Presenters
{
    public class MainPresenter
    {
        // 필드
        private BitmapModel _bitmapModel; // 그림 데이터 Model
        private ComfyUIModel _comfyUIModel; // ComfyUI Model
        private FileModel _fileModel; // 파일 Model
        private MainForm _mainForm; // 메인 폼

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="bitmapModel">BitmapModel</param>
        /// <param name="comfyUIModel">ComfyUIModel</param>
        /// <param name="fileModel">FileModel</param>
        /// <param name="mainForm">MainForm</param>
        public MainPresenter(BitmapModel bitmapModel, ComfyUIModel comfyUIModel, FileModel fileModel, MainForm mainForm) { throw new NotImplementedException(); }

        /// <summary>
        /// 어플리케이션 실행
        /// </summary>
        public void Run() { throw new NotImplementedException(); }
    }
}