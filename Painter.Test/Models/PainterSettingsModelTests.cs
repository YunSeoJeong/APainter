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
            // Arrange
            var settingsModel = new PainterSettingsModel();
            bool eventRaised = false;
            settingsModel.ToolChanged += () => eventRaised = true;

            // Act
            settingsModel.SetTool(ToolType.Pencil);

            // Assert
            Assert.AreEqual(ToolType.Pencil, settingsModel.CurrentTool);
            Assert.IsTrue(eventRaised);
        }

        [TestMethod]
        public void PrimaryColor_Setter_SetsColor()
        {
            // Arrange
            var settingsModel = new PainterSettingsModel();
            var newColor = Color.Red;

            // Act
            settingsModel.PrimaryColor = newColor;

            // Assert
            Assert.AreEqual(newColor, settingsModel.PrimaryColor);
        }

        [TestMethod]
        public void BrushSize_Setter_SetsSize()
        {
            // Arrange
            var settingsModel = new PainterSettingsModel();
            var newSize = 10;

            // Act
            settingsModel.BrushSize = newSize;

            // Assert
            Assert.AreEqual(newSize, settingsModel.BrushSize);
        }

        [TestMethod]
        public void Constructor_InitializesWithDefaultValues()
        {
            // Arrange & Act
            var settingsModel = new PainterSettingsModel();

            // Assert
            Assert.AreEqual(ToolType.Brush, settingsModel.CurrentTool);
            Assert.AreEqual(Color.Black, settingsModel.PrimaryColor);
            Assert.AreEqual(5, settingsModel.BrushSize);
        }
        
        [TestMethod]
        public void ToolChangedEvent_NotifiesMultipleSubscribers()
        {
            // Arrange
            var settingsModel = new PainterSettingsModel();
            int eventCount = 0;
            
            settingsModel.ToolChanged += () => eventCount++;
            settingsModel.ToolChanged += () => eventCount++;

            // Act
            settingsModel.SetTool(ToolType.Pencil);

            // Assert
            Assert.AreEqual(2, eventCount);
        }

        [TestMethod]
        public void BrushSize_WhenSet_TriggersBrushSizeChangedEvent()
        {
            // Arrange
            var settingsModel = new PainterSettingsModel();
            bool eventTriggered = false;
            settingsModel.BrushSizeChanged += () => eventTriggered = true;

            // Act
            settingsModel.BrushSize = 10;

            // Assert
            Assert.IsTrue(eventTriggered);
        }

        [TestMethod]
        public void PrimaryColor_WhenSet_TriggersPrimaryColorChangedEvent()
        {
            // Arrange
            var settingsModel = new PainterSettingsModel();
            bool eventTriggered = false;
            settingsModel.PrimaryColorChanged += () => eventTriggered = true;

            // Act
            settingsModel.PrimaryColor = Color.Red;

            // Assert
            Assert.IsTrue(eventTriggered);
        }
    }
}