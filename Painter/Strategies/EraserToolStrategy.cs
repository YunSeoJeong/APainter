using System;
using System.Collections.Generic;
using System.Drawing;

namespace Painter.Strategies
{
    public class EraserToolStrategy : IToolStrategy
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

            // 속도 감응: 빠를수록 지우기 강도 감소 (기본 투명도 0.9 ~ 최소 0.3)
            float baseOpacity = 0.9f;
            float adjustedOpacity = (float)(baseOpacity - Math.Clamp(_smoothedSpeed * 0.15, 0, 0.6));
            
            // 투명색으로 지우개 컨텍스트 생성
            var eraserContext = new DrawingContext(
                context.StartPoint,
                context.EndPoint,
                Color.Transparent,
                context.BrushSize,
                context.SetPixel,
                context.GetPixel,
                adjustedOpacity
            );
            
            // 곡선 그리기
            DrawingAlgorithms.DrawCurve(new List<Point> { eraserContext.StartPoint, eraserContext.EndPoint }, eraserContext);
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