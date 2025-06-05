using System;
using System.Drawing;

namespace Painter.Strategies
{
    public struct DrawingContext
    {
        public Point StartPoint { get; }
        public Point EndPoint { get; }
        public Color PrimaryColor { get; }
        public int BrushSize { get; }
        public Action<int, int, Color> SetPixel { get; }

        public DrawingContext(Point start, Point end,
                            Color primaryColor,
                            int brushSize,
                            Action<int, int, Color> setPixel)
        {
            StartPoint = start;
            EndPoint = end;
            PrimaryColor = primaryColor;
            BrushSize = brushSize;
            SetPixel = setPixel;
        }
    }
}