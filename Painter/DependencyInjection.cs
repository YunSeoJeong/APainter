using Microsoft.Extensions.DependencyInjection;
using Painter.Interfaces;
using Painter.Models;
using Painter.Presenters;
using Painter.Strategies;
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
                    "http://localhost:8188/"));
            
            // 팩토리 및 전략 등록
            services.AddSingleton<IToolStrategyFactory, ToolStrategyFactory>();
            services.AddTransient<IToolStrategy, BrushToolStrategy>();
            services.AddTransient<IToolStrategy, PencilToolStrategy>();
            services.AddTransient<IToolStrategy, EraserToolStrategy>();
            
            // 뷰 등록
            services.AddSingleton<IMainView, MainForm>();
            services.AddSingleton<ICanvasView, CanvasView>();
            services.AddSingleton<IToolboxView, ToolboxView>();
            services.AddSingleton<ILayerManagerView, LayerManagerView>();
            services.AddSingleton<IMenuView, MenuView>();
            
            // 프레젠터 등록
            services.AddSingleton<MainPresenter>();
            services.AddSingleton<CanvasPresenter>();
            services.AddSingleton<ToolboxPresenter>();
            services.AddSingleton<LayerManagerPresenter>();
            services.AddSingleton<MenuPresenter>();
            
            return services.BuildServiceProvider();
        }
    }
}