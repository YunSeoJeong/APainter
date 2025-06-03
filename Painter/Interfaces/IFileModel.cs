namespace Painter.Interfaces
{
    public interface IFileModel
    {
        void SaveToFile(System.Drawing.Bitmap bitmap, string filePath);
        System.Drawing.Bitmap LoadFromFile(string filePath);
    }
}