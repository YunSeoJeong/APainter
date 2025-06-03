using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Painter.Models
{
    public class FileModel
    {
        public void SaveToFile(Bitmap bitmap, string filePath)
        {
            if (bitmap == null)
                throw new ArgumentNullException(nameof(bitmap));
            
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be empty", nameof(filePath));

            try
            {
                ImageFormat format = GetImageFormat(filePath);
                bitmap.Save(filePath, format);
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to save image: {ex.Message}", ex);
            }
        }

        public Bitmap LoadFromFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be empty", nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Image file not found", filePath);

            try
            {
                return new Bitmap(filePath);
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to load image: {ex.Message}", ex);
            }
        }

        private ImageFormat GetImageFormat(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return extension switch
            {
                ".jpg" or ".jpeg" => ImageFormat.Jpeg,
                ".png" => ImageFormat.Png,
                ".bmp" => ImageFormat.Bmp,
                ".gif" => ImageFormat.Gif,
                ".tiff" => ImageFormat.Tiff,
                _ => throw new NotSupportedException($"Unsupported file format: {extension}")
            };
        }
    }
}