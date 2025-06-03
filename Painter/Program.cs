using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Painter.Presenters;
using Painter.Views;
using Painter.Models;
using Painter.Interfaces;

namespace Painter
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // DI 컨테이너 설정
            var serviceProvider = DependencyInjection.ConfigureServices();

            // MainPresenter 가져오기
            var mainPresenter = serviceProvider.GetRequiredService<MainPresenter>();

            // 애플리케이션 실행
            mainPresenter.Run();
        }
    }
}