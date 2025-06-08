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
            
            // 알파 블렌딩 적용한 커스텀 드로잉
            DrawLineWithBlending(context, adjustedOpacity);
        }

        private void DrawLineWithBlending(DrawingContext context, float baseOpacity)
        {
            int brushSize = context.BrushSize;
            Point start = context.StartPoint;
            Point end = context.EndPoint;

            // 브레젠햄 라인 알고리즘 구현
            int x0 = start.X;
            int y0 = start.Y;
            int x1 = end.X;
            int y1 = end.Y;

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = (x0 < x1) ? 1 : -1;
            int sy = (y0 < y1) ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                // 현재 점에 원형 브러시 적용
                for (int bx = -brushSize; bx <= brushSize; bx++)
                {
                    for (int by = -brushSize; by <= brushSize; by++)
                    {
                        // 브러시 범위 내에서만 처리
                        if (bx * bx + by * by <= brushSize * brushSize)
                        {
                            int px = x0 + bx;
                            int py = y0 + by;
                            
                            if (px >= 0 && py >= 0)
                            {
                                // 거리 비례 투명도 계산 (중앙 100% → 가장자리 0%)
                                double distance = Math.Sqrt(bx*bx + by*by);
                                float distanceFactor = 1.0f - (float)Math.Clamp(distance / brushSize, 0, 1);
                                float pixelOpacity = baseOpacity * distanceFactor;
                                
                                // MAX 함수 기반 알파 처리 (과누적 문제 해결)
                                int newAlpha = (int)(pixelOpacity * 255);
                                Color existingColor = context.GetPixel(px, py);
                                
                                // 기존 알파보다 큰 경우에만 업데이트
                                if (newAlpha > existingColor.A)
                                {
                                    Color finalColor = Color.FromArgb(
                                        newAlpha,
                                        context.PrimaryColor.R,
                                        context.PrimaryColor.G,
                                        context.PrimaryColor.B
                                    );
                                    context.SetPixel(px, py, finalColor);
                                }
                            }
                        }
                    }
                }

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