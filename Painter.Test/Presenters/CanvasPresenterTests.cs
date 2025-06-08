using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Painter.Interfaces;
using Painter.Models;
using Painter.Presenters;
using System.Drawing;
using System.Windows.Forms;
using Painter.Strategies;

namespace Painter.Test.Presenters
{
    [TestClass]
    public class CanvasPresenterTests
    {
        [TestMethod]
        public void CanvasPresenter_OnMouseDown_SetsLastPoint()
        {
            // Arrange
            var mockView = new Mock<ICanvasView>();
            var mockBitmapModel = new Mock<IBitmapModel>();
            var mockSettingsModel = new Mock<IPainterSettingsModel>();
            var mockToolStrategyFactory = new Mock<IToolStrategyFactory>();
            
            var presenter = new CanvasPresenter(
                mockView.Object, 
                mockBitmapModel.Object, 
                mockSettingsModel.Object,
                mockToolStrategyFactory.Object);

            // Act
            presenter.OnMouseDown(null, new MouseEventArgs(MouseButtons.Left, 1, 10, 20, 0));

            // Assert
            // 검증 로직 (마지막 포인트가 설정되었는지 확인)
        }

        [TestMethod]
        public void CanvasPresenter_OnMouseMove_WithLeftButton_DrawsLine()
        {
            // Arrange
            var mockView = new Mock<ICanvasView>();
            var mockBitmapModel = new Mock<IBitmapModel>();
            var mockSettingsModel = new Mock<IPainterSettingsModel>();
            mockSettingsModel.Setup(m => m.CurrentTool).Returns(ToolType.Brush);
            
            // 도구 전략 모의 설정
            var mockToolStrategy = new Mock<IToolStrategy>();
            var mockToolStrategyFactory = new Mock<IToolStrategyFactory>();
            mockToolStrategyFactory.Setup(f => f.CreateToolStrategy(It.IsAny<ToolType>()))
                .Returns(mockToolStrategy.Object);

            var presenter = new CanvasPresenter(
                mockView.Object, 
                mockBitmapModel.Object, 
                mockSettingsModel.Object,
                mockToolStrategyFactory.Object);
            
            // 마우스 다운 이벤트 시뮬레이션
            presenter.OnMouseDown(null, new MouseEventArgs(MouseButtons.Left, 1, 10, 20, 0));

            // Act
            presenter.OnMouseMove(null, new MouseEventArgs(MouseButtons.Left, 1, 30, 40, 0));

            // Assert
            mockBitmapModel.Verify(m => m.Lock(), Times.AtLeastOnce);
            mockBitmapModel.Verify(m => m.Unlock(), Times.AtLeastOnce);
            mockView.Verify(m => m.SetBitmap(It.IsAny<Bitmap>()), Times.AtLeastOnce);
            mockToolStrategyFactory.Verify(f => f.CreateToolStrategy(ToolType.Brush), Times.AtLeastOnce);
        }

        // [TestMethod]
        // public void UpdateView_CallsSetBitmap()
        // {
        //     // Arrange
        //     var mockView = new Mock<ICanvasView>();
        //     var mockBitmapModel = new Mock<IBitmapModel>();
        //     var mockSettingsModel = new Mock<IPainterSettingsModel>();
        //     var mockToolStrategyFactory = new Mock<IToolStrategyFactory>();
            
        //     var presenter = new CanvasPresenter(
        //         mockView.Object, 
        //         mockBitmapModel.Object, 
        //         mockSettingsModel.Object,
        //         mockToolStrategyFactory.Object);

        //     // Act
        //     presenter.UpdateView();

        //     // Assert
        //     mockView.Verify(v => v.SetBitmap(It.IsAny<Bitmap>()), Times.Once);
        // }

        [TestMethod]
        public void CanvasPresenter_OnMouseMove_WithPencilTool_DrawsPixels()
        {
            // Arrange
            var mockView = new Mock<ICanvasView>();
            var mockBitmapModel = new Mock<IBitmapModel>();
            var mockSettingsModel = new Mock<IPainterSettingsModel>();
            mockSettingsModel.Setup(m => m.CurrentTool).Returns(ToolType.Pencil);
            mockSettingsModel.Setup(m => m.PrimaryColor).Returns(Color.Blue);
            mockSettingsModel.Setup(m => m.BrushSize).Returns(3);
            
            // 펜슬 도구 전략 모의 설정
            var mockPencilStrategy = new Mock<IToolStrategy>();
            var mockToolStrategyFactory = new Mock<IToolStrategyFactory>();
            mockToolStrategyFactory.Setup(f => f.CreateToolStrategy(ToolType.Pencil))
                .Returns(mockPencilStrategy.Object);

            var presenter = new CanvasPresenter(
                mockView.Object, 
                mockBitmapModel.Object, 
                mockSettingsModel.Object,
                mockToolStrategyFactory.Object);
            
            // 마우스 다운 이벤트 시뮬레이션
            presenter.OnMouseDown(null, new MouseEventArgs(MouseButtons.Left, 1, 10, 20, 0));

            // Act
            presenter.OnMouseMove(null, new MouseEventArgs(MouseButtons.Left, 1, 30, 40, 0));

            // Assert
            mockPencilStrategy.Verify(p => p.Draw(It.IsAny<DrawingContext>()), Times.AtLeastOnce);
            // 펜슬 전략이 DrawingContext의 SetPixel 액션을 호출했는지 확인
            mockPencilStrategy.Verify(p => p.Draw(It.Is<DrawingContext>(ctx => 
            ctx.SetPixel != null)), Times.AtLeastOnce);
        }
    }
}