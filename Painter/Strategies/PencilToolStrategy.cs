using System;
using System.Collections.Generic;
using System.Drawing;

namespace Painter.Strategies
{
    public class PencilToolStrategy : IToolStrategy
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

            // 속도 감응: 투명도 증가 강도 감소 (0.15 → 0.08)
            float baseOpacity = 0.4f; // 기본 투명도 감소 (0.8 → 0.4)
            float adjustedOpacity = (float)(baseOpacity + Math.Clamp(_smoothedSpeed * 0.08, 0, 0.3));
            
            // 조정된 투명도로 새로운 컨텍스트 생성
            var pencilContext = new DrawingContext(
                context.StartPoint,
                context.EndPoint,
                Color.FromArgb((int)(adjustedOpacity * 255), context.PrimaryColor),
                context.BrushSize,
                context.SetPixel,
                context.GetPixel,
                adjustedOpacity
            );
            
            // 곡선 그리기 (겹침 방지 활성화)
            DrawingAlgorithms.DrawCurve(new List<Point> { pencilContext.StartPoint, pencilContext.EndPoint }, pencilContext);
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