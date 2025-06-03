using System;
using System.Drawing;

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
        
    }
}