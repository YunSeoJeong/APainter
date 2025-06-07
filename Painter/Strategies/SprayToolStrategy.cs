using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Painter.Strategies
{
    public class SprayToolStrategy : IToolStrategy
    {
        private Point _lastPoint;
        private DateTime _lastDrawTime = DateTime.Now;
        private double _smoothedSpeed = 0;
        private readonly Random _random = new Random();

        public void Draw(DrawingContext context)
        {
            // 속도 계산 및 필터링
            double speed = CalculateSpeed(context.StartPoint, _lastPoint, _lastDrawTime);
            _lastPoint = context.EndPoint;
            _lastDrawTime = DateTime.Now;

            // 저역통과 필터 적용 (과도한 변화 방지)
            const double filterFactor = 0.2;
            _smoothedSpeed = _smoothedSpeed * (1 - filterFactor) + speed * filterFactor;

            // 속도 감응: 빠를수록 입자 밀도 감소
            int baseDensity = 100;
            int adjustedDensity = (int)(baseDensity * (1 - Math.Clamp(_smoothedSpeed * 0.1, 0, 0.7)));
            
            // 무작위 분사 알고리즘
            SprayPaint(context, adjustedDensity);
        }

        private void SprayPaint(DrawingContext context, int density)
        {
            int radius = context.BrushSize / 2;
            for (int i = 0; i < density; i++)
            {
                // 무작위 각도 및 거리 생성
                double angle = _random.NextDouble() * 2 * Math.PI;
                double distance = _random.NextDouble() * radius;
                
                // 무작위 위치 계산
                int x = context.EndPoint.X + (int)(Math.Cos(angle) * distance);
                int y = context.EndPoint.Y + (int)(Math.Sin(angle) * distance);
                
                // 알파 블렌딩 적용
                Color newColor = context.PrimaryColor;
                if (context.PrimaryColor.A < 255)
                {
                    Color bgColor = context.GetPixel(x, y);
                    float alpha = 1 - (float)(distance / radius);
                    newColor = BlendColors(bgColor, context.PrimaryColor, alpha * context.Opacity);
                }
                
                context.SetPixel(x, y, newColor);
            }
        }

        private Color BlendColors(Color bg, Color fg, float alpha)
        {
            float invAlpha = 1 - alpha;
            int r = (int)(fg.R * alpha + bg.R * invAlpha);
            int g = (int)(fg.G * alpha + bg.G * invAlpha);
            int b = (int)(fg.B * alpha + bg.B * invAlpha);
            int a = Math.Min(255, (int)(fg.A * alpha + bg.A * invAlpha));
            return Color.FromArgb(a, r, g, b);
        }

        private double CalculateSpeed(Point current, Point previous, DateTime lastTime)
        {
            if (previous == Point.Empty) return 0;
            
            double distance = Math.Sqrt(Math.Pow(current.X - previous.X, 2) + 
                                       Math.Pow(current.Y - previous.Y, 2));
            TimeSpan timeDelta = DateTime.Now - lastTime;
            
            return distance / Math.Max(timeDelta.TotalSeconds, 0.001);
        }
    }
}