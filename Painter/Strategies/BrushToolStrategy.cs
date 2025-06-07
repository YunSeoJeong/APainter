using System;
using System.Collections.Generic;
using System.Drawing;

namespace Painter.Strategies
{
    public class BrushToolStrategy : IToolStrategy
    {
        private Point _lastPoint;
        private DateTime _lastDrawTime = DateTime.Now;
        private double _smoothedSpeed = 0;

        public void Draw(DrawingContext context)
        {
            // 속도 계산 및 필터링
            double speed = CalculateSpeed(context.StartPoint, _lastPoint, _lastDrawTime);
            _lastPoint = context.EndPoint;
            _lastDrawTime = DateTime.Now;

            // 저역통과 필터 적용 (과도한 변화 방지)
            const double filterFactor = 0.2;
            _smoothedSpeed = _smoothedSpeed * (1 - filterFactor) + speed * filterFactor;

            // 속도 감응: 빠를수록 브러시 크기 감소 (기본 크기의 30%~100%)
            int baseSize = context.BrushSize;
            int adjustedSize = (int)(baseSize * (1 - Math.Clamp(_smoothedSpeed * 0.3, 0, 0.5)));
            
            // 조정된 크기로 새로운 컨텍스트 생성
            var brushContext = new DrawingContext(
                context.StartPoint,
                context.EndPoint,
                context.PrimaryColor,
                adjustedSize,
                context.SetPixel,
                context.GetPixel,
                context.Opacity
            );
            
            // 곡선 그리기
            DrawingAlgorithms.DrawCurve(new List<Point> { brushContext.StartPoint, brushContext.EndPoint }, brushContext);
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