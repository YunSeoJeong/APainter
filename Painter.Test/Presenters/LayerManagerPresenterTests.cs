using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Painter.Presenters;
using Painter.Interfaces;
using System.Drawing;

namespace Painter.Test.Presenters
{
    [TestClass]
    public class LayerManagerPresenterTests
    {
        private Mock<ILayerManagerView>? _mockView;
        private Mock<IBitmapModel>? _mockBitmapModel;
        private LayerManagerPresenter? _presenter;

        [TestInitialize]
        public void Setup()
        {
            _mockView = new Mock<ILayerManagerView>();
            _mockBitmapModel = new Mock<IBitmapModel>();
            
            _presenter = new LayerManagerPresenter(
                _mockView.Object,
                _mockBitmapModel.Object
            );
        }

        [TestMethod]
        public void Constructor_SubscribesToViewEvents()
        {
            // Arrange
            var addLayerHandler = _mockView!.Invocations
                .First(i => i.Method.Name == "add_AddLayerClicked").Arguments[0];

            // Act & Assert
            Assert.IsNotNull(addLayerHandler);
        }
    }
}