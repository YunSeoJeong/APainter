using System;
using System.Windows.Forms;
using Painter.Interfaces;
using Painter.Models;

namespace Painter.Presenters
{
    public class MenuPresenter
    {
        private readonly IMenuView? _view;
        private readonly IFileModel? _fileModel;
        private readonly IBitmapModel? _bitmapModel;

        public MenuPresenter(IMenuView? view, IFileModel? fileModel, IBitmapModel? bitmapModel)
        {
            _view = view;
            _fileModel = fileModel;
            _bitmapModel = bitmapModel;
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            _view!.FileSaveClicked += OnFileSaveClicked;
        }

        public void OnFileSaveClicked(object? sender, EventArgs e)
        {
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";
                saveFileDialog.Title = "Save Image";
                
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // 실제 비트맵 모델에서 비트맵 가져오기
                        var bitmap = _bitmapModel!.GetBitmap();
                        _fileModel!.SaveToFile(bitmap, saveFileDialog.FileName);
                        MessageBox.Show("File saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to save file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}