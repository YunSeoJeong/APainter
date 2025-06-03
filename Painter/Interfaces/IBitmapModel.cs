using System.Drawing;

namespace Painter.Interfaces
{
    public interface IBitmapModel
    {
        Bitmap GetBitmap();
        void Lock();
        void Unlock();
        void SetPixel(int x, int y, Color color);
        Color GetPixel(int x, int y);
        void Clear(Color color);
        void Save(string filePath);
        void Load(string filePath);
    }
}