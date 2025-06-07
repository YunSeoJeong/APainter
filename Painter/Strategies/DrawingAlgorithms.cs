using System;
using System.Collections.Generic;
using System.Drawing;

namespace Painter.Strategies
{
    public static class DrawingAlgorithms
    {
        public static void DrawLine(DrawingContext context)
        {
            if (context.StartPoint == context.EndPoint)
            {
                DrawPoint(context.EndPoint, context);
                return;
            }
            
            var points = new List<Point> { context.StartPoint, context.EndPoint };
            DrawCurve(points, context);
        }

        public static void DrawCurve(List<Point> points, DrawingContext context)
        {
            if (points.Count < 2) return;

            context.LastDrawTime = DateTime.Now;

            // 첫 번째 선분 그리기
            DrawLineSegment(points[0], points[1], context);

            if (points.Count > 2)
            {
                // 동적 보간 간격 계산
                float step = CalculateDynamicStep(points);
                
                for (int i = 0; i < points.Count - 1; i++)
                {
                    Point p0 = i > 0 ? points[i - 1] : points[i];
                    Point p1 = points[i];
                    Point p2 = points[i + 1];
                    Point p3 = (i + 2 < points.Count) ? points[i + 2] : points[i + 1];

                    for (float t = 0; t <= 1; t += step)
                    {
                        float t2 = t * t;
                        float t3 = t2 * t;
                        
                        float x = 0.5f * ((2 * p1.X) +
                            (-p0.X + p2.X) * t +
                            (2 * p0.X - 5 * p1.X + 4 * p2.X - p3.X) * t2 +
                            (-p0.X + 3 * p1.X - 3 * p2.X + p3.X) * t3);
                            
                        float y = 0.5f * ((2 * p1.Y) +
                            (-p0.Y + p2.Y) * t +
                            (2 * p0.Y - 5 * p1.Y + 4 * p2.Y - p3.Y) * t2 +
                            (-p0.Y + 3 * p1.Y - 3 * p2.Y + p3.Y) * t3);
                            
                        DrawPoint(new Point((int)x, (int)y), context, preventOverlap: true);
                    }
                }
            }
        }

        private static float CalculateDynamicStep(List<Point> points)
        {
            if (points.Count < 2) return 0.05f;

            double totalDist = 0;
            for (int i = 1; i < points.Count; i++)
            {
                totalDist += Distance(points[i-1], points[i]);
            }
            double avgDist = totalDist / (points.Count - 1);
            
            // 거리 기반 동적 스텝 계산 (거리가 짧을수록 밀도 높음)
            float step = (float)Math.Clamp(1.0 / avgDist, 0.01, 0.1);
            return step;
        }

        private static double Distance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }

        private static void DrawLineSegment(Point start, Point end, DrawingContext context)
        {
            int x0 = start.X;
            int y0 = start.Y;
            int x1 = end.X;
            int y1 = end.Y;
        
            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;
        
            DrawPoint(start, context);
        
            while (true)
            {
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
                DrawPoint(new Point(x0, y0), context, preventOverlap: true);
            }
        }

        public static void DrawPoint(Point point, DrawingContext context, bool preventOverlap = false)
        {
            int radius = context.BrushSize / 2;
            HashSet<(int, int)> drawnPixels = new HashSet<(int, int)>();

            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    int targetX = point.X + i;
                    int targetY = point.Y + j;
                    
                    // 중복 픽셀 방지
                    if (preventOverlap && drawnPixels.Contains((targetX, targetY)))
                        continue;
                    
                    float distance = (float)Math.Sqrt(i * i + j * j);
                    if (distance <= radius)
                    {
                        float alpha = 1 - (distance / radius);
                        Color newColor = context.PrimaryColor;
                        
                        if (context.PrimaryColor.A < 255)
                        {
                            Color bgColor = context.GetPixel(targetX, targetY);
                            newColor = BlendColors(bgColor, context.PrimaryColor, alpha * context.Opacity);
                        }
                        
                        context.SetPixel(targetX, targetY, newColor);
                        drawnPixels.Add((targetX, targetY)); // 렌더링된 픽셀 기록
                    }
                }
            }
        }

        private static Color BlendColors(Color bg, Color fg, float alpha)
        {
            float invAlpha = 1 - alpha;
            int r = (int)(fg.R * alpha + bg.R * invAlpha);
            int g = (int)(fg.G * alpha + bg.G * invAlpha);
            int b = (int)(fg.B * alpha + bg.B * invAlpha);
            int a = Math.Min(255, (int)(fg.A * alpha + bg.A * invAlpha));
            return Color.FromArgb(a, r, g, b);
        }
    }
}