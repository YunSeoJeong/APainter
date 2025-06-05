using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Painter;
using Painter.Interfaces;
using Painter.Models;
using Painter.Presenters;
using Painter.Strategies;
using Painter.Views;

namespace Painter.Test
{
    [TestClass]
    public class DependencyInjectionTests
    {
        [TestMethod]
        public void ConfigureServices_RegistersAllRequiredServices()
        {
            // Arrange & Act
            var serviceProvider = DependencyInjection.ConfigureServices();

            // Assert
            Assert.IsNotNull(serviceProvider.GetService<IBitmapModel>());
            Assert.IsNotNull(serviceProvider.GetService<IFileModel>());
            Assert.IsNotNull(serviceProvider.GetService<IPainterSettingsModel>());
            Assert.IsNotNull(serviceProvider.GetService<IComfyUIModel>());
            Assert.IsNotNull(serviceProvider.GetService<IToolStrategyFactory>());
            Assert.IsNotNull(serviceProvider.GetService<IMainView>());
            Assert.IsNotNull(serviceProvider.GetService<MainPresenter>());
        }

        [TestMethod]
        public void ConfigureServices_VerifySingletonLifetime()
        {
            // Arrange
            var serviceProvider = DependencyInjection.ConfigureServices();

            // Act
            var instance1 = serviceProvider.GetService<IBitmapModel>();
            var instance2 = serviceProvider.GetService<IBitmapModel>();

            // Assert
            Assert.AreSame(instance1, instance2);
        }

        [TestMethod]
        public void ConfigureServices_VerifyTransientLifetime()
        {
            // Arrange
            var serviceProvider = DependencyInjection.ConfigureServices();

            // Act
            var instance1 = serviceProvider.GetService<IToolStrategy>();
            var instance2 = serviceProvider.GetService<IToolStrategy>();

            // Assert
            Assert.AreNotSame(instance1, instance2);
        }

        [TestMethod]
        public void ConfigureServices_ResolvesPresenterDependencies()
        {
            // Arrange
            var serviceProvider = DependencyInjection.ConfigureServices();

            // Act & Assert
            Assert.IsNotNull(serviceProvider.GetService<MainPresenter>());
            Assert.IsNotNull(serviceProvider.GetService<CanvasPresenter>());
            Assert.IsNotNull(serviceProvider.GetService<ToolboxPresenter>());
            Assert.IsNotNull(serviceProvider.GetService<LayerManagerPresenter>());
            Assert.IsNotNull(serviceProvider.GetService<MenuPresenter>());
        }
    }
}