using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using Painter.Models;

namespace Painter.Tests.Models
{
    [TestClass]
    public class BitmapModelTests
    {
        [TestMethod]
        public void GetBitmap_ShouldReturnBitmap()
        {
            // Arrange
            var model = new BitmapModel(800, 600);
            
            // Act
            var result = model.GetBitmap();
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(800, result.Width);
            Assert.AreEqual(600, result.Height);
        }

        [TestMethod]
        public void SetPixel_ShouldChangePixelColor()
        {
            // Arrange
            var model = new BitmapModel(10, 10);
            var testColor = Color.Red;
            
            // Act
            model.Lock();
            model.SetPixel(5, 5, testColor);
            var resultColor = model.GetPixel(5, 5);
            model.Unlock();
            
            // Assert
            Assert.AreEqual(testColor.ToArgb(), resultColor.ToArgb());
        }

        [TestMethod]
        public void Clear_ShouldFillWithColor()
        {
            // Arrange
            var model = new BitmapModel(10, 10);
            var clearColor = Color.Blue;
            
            // Act
            model.Clear(clearColor);
            model.Lock();
            var resultColor = model.GetPixel(0, 0);
            model.Unlock();
            
            // Assert
            Assert.AreEqual(clearColor.ToArgb(), resultColor.ToArgb());
        }
    }
}