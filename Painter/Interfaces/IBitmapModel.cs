using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Painter.Interfaces
{
    public interface IBitmapModel : IDisposable
    {
        Bitmap GetBitmap();
        void Lock();
        void Unlock();
        void SetPixel(int x, int y, Color color);
        Color GetPixel(int x, int y);
        void Clear(Color color);
        int Width { get; }
        int Height { get; }
        
        // LockBits 지원을 위한 메서드 추가
        BitmapData LockBits(Rectangle rect, ImageLockMode flags, PixelFormat format);
        void UnlockBits(BitmapData bitmapData);
    }
}