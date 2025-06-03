using Microsoft.Extensions.DependencyInjection;
using Painter.Interfaces;
using Painter.Models;
using Painter.Presenters;
using Painter.Views;

namespace Painter
{
    public static class DependencyInjection
    {
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            
            // 모델 등록
            services.AddSingleton<IBitmapModel, BitmapModel>();
            services.AddSingleton<FileModel>();
            services.AddSingleton<ComfyUIModel>();
            
            // 뷰 등록
            services.AddTransient<ICanvasView, CanvasView>();
            
            // 프레젠터 등록
            services.AddTransient<CanvasPresenter>(provider => 
                new CanvasPresenter(
                    provider.GetRequiredService<ICanvasView>(),
                    provider.GetRequiredService<IBitmapModel>()
                )
            );
            
            return services.BuildServiceProvider();
        }
    }
}