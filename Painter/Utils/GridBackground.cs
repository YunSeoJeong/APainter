using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace Painter.Utils
{
    public static class GridBackground
    {
        private static Dictionary<string, Bitmap> _gridCache = new Dictionary<string, Bitmap>();

        /// <summary>그리드 배경 패턴 생성 (캐싱 적용)</summary>
        public static Bitmap GenerateGrid(int width, int height)
        {
            string cacheKey = $"{width}x{height}";
            
            // 캐시된 그리드 반환
            if (_gridCache.ContainsKey(cacheKey))
            {
                return _gridCache[cacheKey];
            }

            Bitmap gridBitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(gridBitmap))
            {
                // 투명 배경 채우기
                g.Clear(Color.Transparent);
                
                // 보조 그리드선 (2px 간격) - 사용자 요청에 따라 2px로 복원
                using (Pen lightPen = new Pen(Color.FromArgb(240, 240, 240), 1f))
                {
                    for (int x = 0; x < width; x += 2)
                    {
                        g.DrawLine(lightPen, x, 0, x, height);
                    }
                    for (int y = 0; y < height; y += 2)
                    {
                        g.DrawLine(lightPen, 0, y, width, y);
                    }
                }
                
                // 주 그리드선 (10px 간격) - 사용자 요청에 따라 10px로 복원
                using (Pen mainPen = new Pen(Color.FromArgb(224, 224, 224), 1f))
                {
                    for (int x = 0; x < width; x += 10)
                    {
                        g.DrawLine(mainPen, x, 0, x, height);
                    }
                    for (int y = 0; y < height; y += 10)
                    {
                        g.DrawLine(mainPen, 0, y, width, y);
                    }
                }
            }
            
            // 새 그리드 캐싱
            _gridCache[cacheKey] = gridBitmap;
            return gridBitmap;
        }
    }
}