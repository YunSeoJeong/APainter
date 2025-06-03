using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Painter.Presenters;
using Painter.Interfaces;
using Painter.Interfaces;

namespace Painter.Test.Presenters
{
    [TestClass]
    public class ToolboxPresenterTests
    {
        private Mock<IToolboxView>? _mockView;
        private Mock<IPainterSettingsModel>? _mockSettingsModel;
        private ToolboxPresenter? _presenter;

        [TestInitialize]
        public void Setup()
        {
            _mockView = new Mock<IToolboxView>();
            _mockSettingsModel = new Mock<IPainterSettingsModel>();
            
            _presenter = new ToolboxPresenter(
                _mockView.Object,
                _mockSettingsModel.Object
            );
        }

        [TestMethod]
        public void OnBrushSelected_SetsBrushTool()
        {
            // Act
            _mockView!.Raise(v => v.BrushSelected += null, null, EventArgs.Empty);

            // Assert
            _mockSettingsModel!.Verify(m => 
                m.SetTool(ToolType.Brush), Times.Once);
        }

        [TestMethod]
        public void OnPencilSelected_SetsPencilTool()
        {
            // Act
            _mockView!.Raise(v => v.PencilSelected += null, null, EventArgs.Empty);

            // Assert
            _mockSettingsModel!.Verify(m => 
                m.SetTool(ToolType.Pencil), Times.Once);
        }

        [TestMethod]
        public void Constructor_SubscribesToViewEvents()
        {
            // Arrange
            var brushHandler = _mockView!.Invocations
                .First(i => i.Method.Name == "add_BrushSelected").Arguments[0];
            var pencilHandler = _mockView!.Invocations
                .First(i => i.Method.Name == "add_PencilSelected").Arguments[0];

            // Act & Assert
            Assert.IsNotNull(brushHandler);
            Assert.IsNotNull(pencilHandler);
        }
    }
}