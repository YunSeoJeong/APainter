using System;
using System.Drawing;
using System.Drawing.Imaging;
using Painter.Interfaces;

namespace Painter.Models
{
    public class BitmapModel : IBitmapModel, IDisposable
    {
        private Bitmap _bitmap;
        private BitmapData? _bitmapData;
        private IntPtr _scan0;
        private bool _isLocked = false;
        private int _width;
        private int _height;

        public BitmapModel(int width, int height)
        {
            _bitmap = new Bitmap(width, height);
            _width = width;
            _height = height;
        }

        // 기본 해상도를 1200x800으로 변경
        public BitmapModel() : this(1200, 800) { }

        public Bitmap GetBitmap() => _bitmap;

        public void Lock()
        {
            if (_isLocked) return;
            _bitmapData = _bitmap.LockBits(
                new Rectangle(0, 0, _bitmap.Width, _bitmap.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb
            );
            _scan0 = _bitmapData.Scan0;
            _isLocked = true;
        }

        public void Unlock()
        {
            if (!_isLocked) return;
            _bitmap.UnlockBits(_bitmapData!);
            _bitmapData = null;
            _isLocked = false;
        }

        public void SetPixel(int x, int y, Color color)
        {
            if (!_isLocked) return;
            if (x < 0 || x >= _width || y < 0 || y >= _height)
                return;
                
            unsafe
            {
                byte* row = (byte*)_scan0 + y * _bitmapData!.Stride;
                int* pixel = (int*)(row + x * 4);
                
                // 알파 블렌딩 (오버플로우 방지)
                Color bgColor = Color.FromArgb(*pixel);
                Color blendedColor = BlendColors(bgColor, color);
                *pixel = blendedColor.ToArgb();
            }
        }

        private Color BlendColors(Color bg, Color fg)
        {
            float fgAlpha = fg.A / 255f;
            float bgAlpha = bg.A / 255f;
            float outAlpha = fgAlpha + (1 - fgAlpha) * bgAlpha;
            
            // 알파가 0인 경우 투명 반환
            if (outAlpha <= 0.001f) return Color.Transparent;
            
            // RGB 계산
            int r = (int)((fg.R * fgAlpha + bg.R * bgAlpha * (1 - fgAlpha)) / outAlpha);
            int g = (int)((fg.G * fgAlpha + bg.G * bgAlpha * (1 - fgAlpha)) / outAlpha);
            int b = (int)((fg.B * fgAlpha + bg.B * bgAlpha * (1 - fgAlpha)) / outAlpha);
            
            // 값 범위 제한 (0-255)
            r = Math.Clamp(r, 0, 255);
            g = Math.Clamp(g, 0, 255);
            b = Math.Clamp(b, 0, 255);
            
            return Color.FromArgb((int)(outAlpha * 255), r, g, b);
        }

        public Color GetPixel(int x, int y)
        {
            if (!_isLocked) return Color.Empty;
            unsafe
            {
                byte* row = (byte*)_scan0 + y * _bitmapData!.Stride;
                int* pixel = (int*)(row + x * 4);
                return Color.FromArgb(*pixel);
            }
        }

        public void Clear(Color color)
        {
            using (var g = Graphics.FromImage(_bitmap))
            {
                g.Clear(color);
            }
        }

        public void Dispose()
        {
            if (_isLocked) Unlock();
            _bitmap?.Dispose();
        }

        public int Width => _width;
        public int Height => _height;
    }
}