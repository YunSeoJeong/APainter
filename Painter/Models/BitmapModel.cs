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
        private int _lockCount = 0; // Track nested lock requests
        private int _width;
        private int _height;

        public BitmapModel(int width, int height)
        {
            _bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            _width = width;
            _height = height;
        }

        // 기본 해상도를 1200x800으로 변경
        public BitmapModel() : this(1200, 800) { }

        public Bitmap GetBitmap() => _bitmap;

        public void Lock()
        {
            if (_lockCount == 0)
            {
                _bitmapData = LockBits(
                    new Rectangle(0, 0, _bitmap.Width, _bitmap.Height),
                    ImageLockMode.ReadWrite,
                    PixelFormat.Format32bppArgb
                );
                _scan0 = _bitmapData.Scan0;
            }
            _lockCount++;
        }

        public void Unlock()
        {
            if (_lockCount == 0) return;
            
            _lockCount--;
            
            if (_lockCount == 0)
            {
                UnlockBits(_bitmapData!);
                _bitmapData = null;
            }
        }

        public BitmapData LockBits(Rectangle rect, ImageLockMode flags, PixelFormat format)
        {
            // Always return existing lock if bitmap is already locked
            if (_lockCount > 0)
            {
                return _bitmapData!;
            }
            
            // Only lock if not already locked
            return _bitmap.LockBits(rect, flags, format);
        }

        public void UnlockBits(BitmapData bitmapData)
        {
            // Only unlock if we're the top-level lock
            if (_lockCount == 0)
            {
                _bitmap.UnlockBits(bitmapData);
                _bitmapData = null;
            }
        }

        public void SetPixel(int x, int y, Color color)
        {
            if (x < 0 || x >= _width || y < 0 || y >= _height)
                return;
                
            if (_lockCount == 0) return;
            
            unsafe
            {
                byte* row = (byte*)_scan0 + y * _bitmapData!.Stride;
                int* pixel = (int*)(row + x * 4);
                
                // 단순 픽셀 덮어쓰기 (알파 블렌딩 없음)
                *pixel = color.ToArgb();
            }
        }

        public Color GetPixel(int x, int y)
        {
            if (_lockCount == 0) return Color.Empty;
            
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
            if (_lockCount > 0)
            {
                _lockCount = 1;
                Unlock();
            }
            _bitmap?.Dispose();
        }

        public int Width => _width;
        public int Height => _height;
    }
}