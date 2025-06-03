using System;
using System.Windows.Forms;
using Painter.Interfaces;
using Painter.Views;

namespace Painter.Presenters
{
    public class MainPresenter
    {
        private readonly IMainView? _mainView;

        public MainPresenter(IMainView? mainView)
        {
            _mainView = mainView;
        }

        public void Run()
        {
            // 메인 폼 초기화
            _mainView?.Initialize();
            Application.Run(_mainView as Form);
        }
    }
}