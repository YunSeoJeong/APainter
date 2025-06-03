using System;
using System.Drawing;
using Painter.Strategies;

namespace Painter.Strategies
{
    public static class DrawingAlgorithms
    {
        public static void DrawLine(DrawingContext context)
        {
            Point start = context.StartPoint;
            Point end = context.EndPoint;
            Color color = context.PrimaryColor;
            int size = context.BrushSize;
            
            int x0 = start.X;
            int y0 = start.Y;
            int x1 = end.X;
            int y1 = end.Y;

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                DrawPoint(x0, y0, context, color, size);

                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        private static void DrawPoint(int x, int y, DrawingContext context, Color color, int size)
        {
            int halfSize = size / 2;
            for (int i = -halfSize; i <= halfSize; i++)
            {
                for (int j = -halfSize; j <= halfSize; j++)
                {
                    int targetX = x + i;
                    int targetY = y + j;
                    context.SetPixel(targetX, targetY, color);
                }
            }
        }
    }
}