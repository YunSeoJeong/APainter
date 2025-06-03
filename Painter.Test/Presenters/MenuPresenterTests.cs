using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Painter.Presenters;
using Painter.Interfaces;
using System.Drawing;

namespace Painter.Test.Presenters
{
    [TestClass]
    public class MenuPresenterTests
    {
        private Mock<IMenuView>? _mockView;
        private Mock<IFileModel>? _mockFileModel;
        private Mock<IBitmapModel>? _mockBitmapModel;
        private MenuPresenter? _presenter;

        [TestInitialize]
        public void Setup()
        {
            _mockView = new Mock<IMenuView>();
            _mockFileModel = new Mock<IFileModel>();
            _mockBitmapModel = new Mock<IBitmapModel>();
            
            _presenter = new MenuPresenter(
                _mockView.Object,
                _mockFileModel.Object,
                _mockBitmapModel.Object
            );
        }

        [TestMethod]
        public void Constructor_SubscribesToViewEvents()
        {
            // Arrange
            var saveHandler = _mockView!.Invocations
                .First(i => i.Method.Name == "add_FileSaveClicked").Arguments[0];

            // Act & Assert
            Assert.IsNotNull(saveHandler);
        }
    }
}