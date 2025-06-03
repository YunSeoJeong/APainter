using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace Painter
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // DI 컨테이너 설정
            var serviceProvider = DependencyInjection.ConfigureServices();
            
            // 메인 폼 실행
            var mainForm = serviceProvider.GetRequiredService<MainForm>();
            Application.Run(mainForm);
        }
    }
}