using Microsoft.Extensions.DependencyInjection;
using Painter.Interfaces;
using Painter.Models;
using Painter.Presenters;
using Painter.Views;
using System;
using System.Net.Http;

namespace Painter
{
    public static class DependencyInjection
    {
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            
            // 공유 서비스 등록
            services.AddSingleton<HttpClient>();
            
            // 모델 등록
            services.AddSingleton<IBitmapModel, BitmapModel>();
            services.AddSingleton<IFileModel, FileModel>();
            services.AddSingleton<IPainterSettingsModel, PainterSettingsModel>();
            services.AddSingleton<IComfyUIModel>(provider => 
                new ComfyUIModel(
                    provider.GetRequiredService<HttpClient>(), 
                    "http://localhost:8188/")); // TODO: 설정 파일에서 가져오도록 개선
            
            // 뷰 등록
            services.AddTransient<IMainView, MainForm>();
            services.AddTransient<ICanvasView, CanvasView>();
            services.AddTransient<IToolboxView, ToolboxView>();
            services.AddTransient<ILayerManagerView, LayerManagerView>();
            services.AddTransient<IMenuView, MenuView>();
            
            // 프레젠터 등록
            services.AddTransient<MainPresenter>();
            services.AddTransient<CanvasPresenter>();
            services.AddTransient<ToolboxPresenter>();
            services.AddTransient<LayerManagerPresenter>();
            services.AddTransient<MenuPresenter>();
            
            return services.BuildServiceProvider();
        }
    }
}