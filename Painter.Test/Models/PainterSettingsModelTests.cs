using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Painter.Models;
using Painter.Interfaces;
using System.Drawing;

namespace Painter.Test.Models
{
    [TestClass]
    public class PainterSettingsModelTests
    {
        [TestMethod]
        public void SetTool_ChangesCurrentToolAndRaisesEvent()
        {
            var settingsModel = new PainterSettingsModel();
            bool eventRaised = false;
            settingsModel.ToolChanged += () => eventRaised = true;

            settingsModel.SetTool(ToolType.Pencil);

            Assert.AreEqual(ToolType.Pencil, settingsModel.CurrentTool);
            Assert.IsTrue(eventRaised);
        }

        [TestMethod]
        public void PrimaryColor_Setter_SetsColor()
        {
            var settingsModel = new PainterSettingsModel();
            var newColor = Color.Red;

            settingsModel.PrimaryColor = newColor;

            Assert.AreEqual(newColor, settingsModel.PrimaryColor);
        }

        [TestMethod]
        public void BrushSize_Setter_SetsSize()
        {
            var settingsModel = new PainterSettingsModel();
            var newSize = 10;

            settingsModel.BrushSize = newSize;

            Assert.AreEqual(newSize, settingsModel.BrushSize);
        }

        [TestMethod]
        public void Constructor_InitializesWithDefaultValues()
        {
            var settingsModel = new PainterSettingsModel();

            Assert.AreEqual(ToolType.Brush, settingsModel.CurrentTool);
            Assert.AreEqual(Color.Black, settingsModel.PrimaryColor);
            Assert.AreEqual(5, settingsModel.BrushSize);
        }

        [TestMethod]
        public void ToolChangedEvent_NotifiesMultipleSubscribers()
        {
            var settingsModel = new PainterSettingsModel();
            int eventCount = 0;

            settingsModel.ToolChanged += () => eventCount++;
            settingsModel.ToolChanged += () => eventCount++;

            settingsModel.SetTool(ToolType.Pencil);

            Assert.AreEqual(2, eventCount);
        }

        [TestMethod]
        public void BrushSize_WhenSet_TriggersBrushSizeChangedEvent()
        {
            var settingsModel = new PainterSettingsModel();
            bool eventTriggered = false;
            settingsModel.BrushSizeChanged += () => eventTriggered = true;

            settingsModel.BrushSize = 10;

            Assert.IsTrue(eventTriggered);
        }

        [TestMethod]
        public void PrimaryColor_WhenSet_TriggersPrimaryColorChangedEvent()
        {
            var settingsModel = new PainterSettingsModel();
            bool eventTriggered = false;
            settingsModel.PrimaryColorChanged += () => eventTriggered = true;

            settingsModel.PrimaryColor = Color.Red;

            Assert.IsTrue(eventTriggered);
        }

        [TestMethod]
        public void MultiplePropertyChanges_TriggerCorrectEvents()
        {
            var settingsModel = new PainterSettingsModel();
            int toolChangedCount = 0;
            int colorChangedCount = 0;
            int sizeChangedCount = 0;

            settingsModel.ToolChanged += () => toolChangedCount++;
            settingsModel.PrimaryColorChanged += () => colorChangedCount++;
            settingsModel.BrushSizeChanged += () => sizeChangedCount++;

            settingsModel.SetTool(ToolType.Eraser);
            settingsModel.PrimaryColor = Color.Blue;
            settingsModel.BrushSize = 8;
            settingsModel.PrimaryColor = Color.Green;

            Assert.AreEqual(1, toolChangedCount);
            Assert.AreEqual(2, colorChangedCount);
            Assert.AreEqual(1, sizeChangedCount);
        }

        [TestMethod]
        public void EventUnsubscription_WorksCorrectly()
        {
            var settingsModel = new PainterSettingsModel();
            int eventCount = 0;

            void EventHandler() => eventCount++;
            settingsModel.ToolChanged += EventHandler;

            settingsModel.SetTool(ToolType.Pencil);
            settingsModel.ToolChanged -= EventHandler;
            settingsModel.SetTool(ToolType.Brush);

            Assert.AreEqual(1, eventCount);
        }
    }
}