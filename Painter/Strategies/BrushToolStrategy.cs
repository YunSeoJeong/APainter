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
        private float _brushSize; // 브러시 크기 저장 필드 추가

        public float Radius => _brushSize; // 반지름 속성 구현

        public void Draw(DrawingContext context)
        {
            // 반지름 경계 처리: radius 바깥 영역 무시
            float radius = Radius;
            // 속도 계산 및 필터링
            double speed = CalculateSpeed(context.StartPoint, _lastPoint, _lastDrawTime);
            _lastPoint = context.EndPoint;
            _lastDrawTime = DateTime.Now;

            // 저역통과 필터 적용 (과도한 변화 방지)
            const double filterFactor = 0.2;
            _smoothedSpeed = _smoothedSpeed * (1 - filterFactor) + speed * filterFactor;

            // 속도 감응: 빠를수록 브러시 투명도 감소 (기본 투명도 0.8 ~ 최소 0.4)
            float baseOpacity = 0.8f;
            float adjustedOpacity = (float)(baseOpacity - Math.Clamp(_smoothedSpeed * 0.15, 0, 0.4));
            
            DrawBrushLine(context, adjustedOpacity, radius);
        }

        private void DrawBrushLine(DrawingContext context, float baseOpacity, float radius)
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
                        // 브러시 범위 내에서만 처리 (반지름 경계 강화)
                        float distance = (float)Math.Sqrt(bx*bx + by*by);
                        if (distance <= radius)
                        {
                            int px = x0 + bx;
                            int py = y0 + by;
                            
                            if (px >= 0 && py >= 0)
                            {
                                // 고체 코어와 감소된 페이드 영역
                                float solidCore = radius * 0.7f;
                                float fadeLength = radius - solidCore;
                                float distanceFactor;
                                if (distance <= solidCore)
                                    distanceFactor = 1.0f;
                                else
                                    distanceFactor = 1.0f - (distance - solidCore) / fadeLength;
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

        public void SetBrushSize(float brushSize)
        {
            _brushSize = brushSize;
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