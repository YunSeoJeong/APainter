using System;
using System.Drawing;

namespace Painter.Strategies
{
    public class DrawingContext
    {
        public Point StartPoint { get; }
        public Point EndPoint { get; }
        public Color PrimaryColor { get; }
        public int BrushSize { get; }
        public Action<int, int, Color> SetPixel { get; }
        public Func<int, int, Color> GetPixel { get; }  // 픽셀 값 읽기 메서드 추가
        public float Opacity { get; } = 1.0f;           // 기본 투명도
        public DateTime LastDrawTime { get; set; } = DateTime.Now;

        public DrawingContext(Point start, Point end,
                            Color primaryColor,
                            int brushSize,
                            Action<int, int, Color> setPixel,
                            Func<int, int, Color> getPixel,  // 새 파라미터
                            float opacity = 1.0f)            // 새 파라미터
        {
            StartPoint = start;
            EndPoint = end;
            PrimaryColor = primaryColor;
            BrushSize = brushSize;
            SetPixel = setPixel;
            GetPixel = getPixel;
            Opacity = opacity;
        }
    }
}