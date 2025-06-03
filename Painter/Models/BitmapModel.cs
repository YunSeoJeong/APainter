using System;
using System.Drawing;
using System.Drawing.Imaging;
using Painter.Interfaces;

namespace Painter.Models
{
    public class BitmapModel : IBitmapModel, IDisposable
    {
        private Bitmap _bitmap;
        private int _width;
        private int _height;
        private BitmapData _bitmapData;
        private IntPtr _scan0;
        private bool _isLocked = false;

        public BitmapModel(int width, int height)
        {
            _width = width;
            _height = height;
            _bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        }

        public BitmapModel() : this(800, 600) { }

        public Bitmap GetBitmap() => _bitmap;

        public void Lock()
        {
            if (_isLocked) return;
            
            _bitmapData = _bitmap.LockBits(
                new Rectangle(0, 0, _width, _height),
                ImageLockMode.ReadWrite,
                _bitmap.PixelFormat
            );
            _scan0 = _bitmapData.Scan0;
            _isLocked = true;
        }

        public void Unlock()
        {
            if (!_isLocked) return;
            
            _bitmap.UnlockBits(_bitmapData);
            _bitmapData = null;
            _isLocked = false;
        }

        public void SetPixel(int x, int y, Color color)
        {
            if (!_isLocked)
                throw new InvalidOperationException("Lock() must be called before pixel operations");

            if (x < 0 || x >= _width || y < 0 || y >= _height)
                return;

            unsafe
            {
                byte* ptr = (byte*)_scan0 + y * _bitmapData.Stride + x * 4;
                ptr[0] = color.B;
                ptr[1] = color.G;
                ptr[2] = color.R;
                ptr[3] = color.A;
            }
        }

        public Color GetPixel(int x, int y)
        {
            if (!_isLocked)
                throw new InvalidOperationException("Lock() must be called before pixel operations");

            if (x < 0 || x >= _width || y < 0 || y >= _height)
                throw new ArgumentOutOfRangeException("Coordinates out of bounds");

            unsafe
            {
                byte* ptr = (byte*)_scan0 + y * _bitmapData.Stride + x * 4;
                return Color.FromArgb(ptr[3], ptr[2], ptr[1], ptr[0]);
            }
        }

        public void Clear(Color color)
        {
            using (Graphics g = Graphics.FromImage(_bitmap))
            {
                g.Clear(color);
            }
        }

        public void Save(string filePath)
        {
            try
            {
                _bitmap.Save(filePath, ImageFormat.Png);
            }
            catch (Exception ex)
            {
                throw new IOException("Failed to save image", ex);
            }
        }

        public void Load(string filePath)
        {
            try
            {
                Bitmap loadedBitmap = new Bitmap(filePath);

                _bitmap.Dispose();
                _bitmap = loadedBitmap;
                _width = _bitmap.Width;
                _height = _bitmap.Height;
            }
            catch (Exception ex)
            {
                throw new IOException("Failed to load image", ex);
            }
        }

        public void Dispose()
        {
            Unlock();
            _bitmap?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}