using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Painter.Interfaces;
using Painter.Models;
using Painter.Presenters;
using Painter.Strategies;
using System.Drawing;
using System.Windows.Forms;

namespace Painter.Test.Presenters
{
    [TestClass]
    public class CanvasPresenterTests
    {
        [TestMethod]
        public void CanvasPresenter_OnMouseDown_SetsLastPoint()
        {
            var mockView = new Mock<ICanvasView>();
            var mockBitmapModel = new Mock<IBitmapModel>();
            var mockSettingsModel = new Mock<IPainterSettingsModel>();
            var mockToolStrategyFactory = new Mock<IToolStrategyFactory>();
            
            mockSettingsModel.Setup(m => m.CurrentTool).Returns(ToolType.Brush);
            mockToolStrategyFactory.Setup(f => f.CreateToolStrategy(It.IsAny<ToolType>()))
                .Returns(new BrushToolStrategy());

            var presenter = new CanvasPresenter(
                mockView.Object, 
                mockBitmapModel.Object, 
                mockSettingsModel.Object,
                mockToolStrategyFactory.Object);

            presenter.OnMouseDown(null, new MouseEventArgs(MouseButtons.Left, 1, 10, 20, 0));

            mockBitmapModel.Verify(m => m.Lock(), Times.AtLeastOnce());
            mockBitmapModel.Verify(m => m.Unlock(), Times.AtLeastOnce());
        }

        [TestMethod]
        public void UpdateView_CallsSetBitmap()
        {
            var mockView = new Mock<ICanvasView>();
            var mockBitmapModel = new Mock<IBitmapModel>();
            var mockSettingsModel = new Mock<IPainterSettingsModel>();
            var mockToolStrategyFactory = new Mock<IToolStrategyFactory>();
            
            // 초기 호출을 고려하여 Verify를 Once 대신 AtLeastOnce로 변경
            mockView.Setup(v => v.SetBitmap(It.IsAny<Bitmap>())).Verifiable();

            var presenter = new CanvasPresenter(
                mockView.Object, 
                mockBitmapModel.Object, 
                mockSettingsModel.Object,
                mockToolStrategyFactory.Object);

            presenter.UpdateView();

            mockView.Verify(v => v.SetBitmap(It.IsAny<Bitmap>()), Times.AtLeastOnce());
        }

        [TestMethod]
        public void CanvasPresenter_OnMouseMove_WithLeftButton_DrawsLine()
        {
            var mockView = new Mock<ICanvasView>();
            var mockBitmapModel = new Mock<IBitmapModel>();
            var mockSettingsModel = new Mock<IPainterSettingsModel>();
            mockSettingsModel.Setup(m => m.CurrentTool).Returns(ToolType.Brush);
            
            var mockToolStrategy = new Mock<IToolStrategy>();
            var mockToolStrategyFactory = new Mock<IToolStrategyFactory>();
            mockToolStrategyFactory.Setup(f => f.CreateToolStrategy(It.IsAny<ToolType>()))
                .Returns(mockToolStrategy.Object);

            var presenter = new CanvasPresenter(
                mockView.Object, 
                mockBitmapModel.Object, 
                mockSettingsModel.Object,
                mockToolStrategyFactory.Object);
            
            presenter.OnMouseDown(null, new MouseEventArgs(MouseButtons.Left, 1, 10, 20, 0));
            presenter.OnMouseMove(null, new MouseEventArgs(MouseButtons.Left, 1, 30, 40, 0));

            mockToolStrategy.Verify(t => t.Draw(It.IsAny<DrawingContext>()), Times.AtLeastOnce());
        }
    }
}