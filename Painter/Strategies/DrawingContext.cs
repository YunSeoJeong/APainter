using System;
using System.Drawing;

namespace Painter.Strategies
{
    public class DrawingContext
    {
        public static Color BlendColors(Color src, Color dst)
        {
            // 알파 블렌딩 구현
            float srcAlpha = src.A / 255f;
            float dstAlpha = dst.A / 255f * (1 - srcAlpha);
            float outAlpha = srcAlpha + dstAlpha;
            
            if (outAlpha == 0) return Color.Transparent;
            
            return Color.FromArgb(
                (int)(outAlpha * 255),
                (int)((src.R * srcAlpha + dst.R * dstAlpha) / outAlpha),
                (int)((src.G * srcAlpha + dst.G * dstAlpha) / outAlpha),
                (int)((src.B * srcAlpha + dst.B * dstAlpha) / outAlpha)
            );
        }

        public static Color MultiplyColors(Color mask, Color color)
        {
            // 마스크 곱연산 구현
            float alpha = mask.A / 255f;
            return Color.FromArgb(
                color.A,
                (int)(color.R * alpha),
                (int)(color.G * alpha),
                (int)(color.B * alpha)
            );
        }
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
