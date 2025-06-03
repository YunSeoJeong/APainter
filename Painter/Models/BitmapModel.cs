using System;
using System.Drawing;
using System.Drawing.Imaging;
using Painter.Interfaces; // 네임스페이스 추가

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

        public BitmapModel() : this(800, 600) { }

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
            unsafe
            {
                byte* row = (byte*)_scan0 + y * _bitmapData!.Stride;
                int* pixel = (int*)(row + x * 4);
                *pixel = color.ToArgb();
            }
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

        // 추가된 속성 구현
        public int Width => _width;
        public int Height => _height;
    }
}