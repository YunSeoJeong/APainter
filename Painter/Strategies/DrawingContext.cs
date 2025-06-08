using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Painter.Strategies
{
    public unsafe class DrawingContext : IDisposable
    {
        private Bitmap? _bitmap;
        private BitmapData? _bitmapData;
        private byte* _scan0;
        private int _stride;

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
        public float Opacity { get; } = 1.0f;           // 기본 투명도

        public DrawingContext(Point start, Point end,
                            Color primaryColor,
                            int brushSize,
                            Bitmap bitmap,  // 비트맵을 직접 받음
                            float opacity = 1.0f)
        {
            StartPoint = start;
            EndPoint = end;
            PrimaryColor = primaryColor;
            BrushSize = brushSize;
            Opacity = opacity;
            _bitmap = bitmap;

            // 비트맵을 잠금
            LockBits();
        }

        private void LockBits()
        {
            if (_bitmap == null) return;
            _bitmapData = _bitmap.LockBits(
                new Rectangle(0, 0, _bitmap.Width, _bitmap.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb
            );
            _scan0 = (byte*)_bitmapData.Scan0;
            _stride = _bitmapData.Stride;
        }

        public void Dispose()
        {
            UnlockBits();
        }

        private void UnlockBits()
        {
            if (_bitmapData == null || _bitmap == null) return;
            _bitmap.UnlockBits(_bitmapData);
            _bitmapData = null;
        }

        public void SetPixel(int x, int y, Color color)
        {
            if (_bitmapData == null) return;
            if (x < 0 || y < 0 || x >= _bitmapData.Width || y >= _bitmapData.Height) return;

            byte* row = _scan0 + (y * _stride);
            int* pixel = (int*)(row + x * 4);
            *pixel = color.ToArgb();
        }

        public Color GetPixel(int x, int y)
        {
            if (_bitmapData == null) return Color.Transparent;
            if (x < 0 || y < 0 || x >= _bitmapData.Width || y >= _bitmapData.Height)
                return Color.Transparent;

            byte* row = _scan0 + (y * _stride);
            int pixelValue = *(int*)(row + x * 4);
            return Color.FromArgb(pixelValue);
        }
    }
}
