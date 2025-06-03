using System.Drawing;
using System;

namespace Painter.Models
{
    // Models
    public class FileModel
    {
        private string _filePath; // 파일 경로

        /// <summary>
        /// FileModel 생성자
        /// </summary>
        /// <param name="filePath">파일 경로</param>
        public FileModel(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Bitmap 데이터를 파일로 저장
        /// </summary>
        /// <param name="bitmap">Bitmap 데이터</param>
        /// <param name="filePath">파일 경로</param>
        public void SaveToFile(Bitmap bitmap, string filePath) { throw new NotImplementedException(); }

        /// <summary>
        /// 파일에서 Bitmap 데이터 불러오기
        /// </summary>
        /// <param name="filePath">파일 경로</param>
        /// <returns>Bitmap 데이터</returns>
        public Bitmap LoadFromFile(string filePath) { throw new NotImplementedException(); }
    }
}