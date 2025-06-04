using System;
using System.Windows.Forms;
using Painter.Interfaces;
using Painter.Views;

namespace Painter.Presenters
{
    public class MainPresenter
    {
        private readonly IMainView? _mainView;
        private readonly CanvasPresenter? _canvasPresenter;
        private readonly ToolboxPresenter? _toolboxPresenter;
        private readonly LayerManagerPresenter? _layerManagerPresenter;
        private MenuPresenter? _menuPresenter;

        public MainPresenter(
            IMainView? mainView,
            CanvasPresenter? canvasPresenter,
            ToolboxPresenter? toolboxPresenter,
            LayerManagerPresenter? layerManagerPresenter,
            MenuPresenter? menuPresenter)
        {
            _mainView = mainView;
            _canvasPresenter = canvasPresenter;
            _toolboxPresenter = toolboxPresenter;
            _layerManagerPresenter = layerManagerPresenter;
            _menuPresenter = menuPresenter;
        }

        public void Run()
        {
            // 메인 폼 초기화
            _mainView?.Initialize();
            Application.Run(_mainView as Form);
        }
    }
}