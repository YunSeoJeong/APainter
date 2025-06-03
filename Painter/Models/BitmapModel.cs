using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Painter.Models
{
    // Models
    public class BitmapModel
    {
        private Bitmap _bitmap; // 그림 데이터 저장 및 관리
        private int _width; // 그림 폭
        private int _height; // 그림 높이
        private BitmapData _bitmapData; // Lock된 Bitmap 데이터
        private IntPtr _scan0; // 스캔 시작 주소

        /// <summary>
        /// BitmapModel 생성자
        /// </summary>
        /// <param name="width">그림 폭</param>
        /// <param name="height">그림 높이</param>
        public BitmapModel(int width, int height)
        {
            _width = width;
            _height = height;
            _bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        }

        /// <summary>
        /// BitmapModel 기본 생성자 (기본 크기: 800x600)
        /// </summary>
        public BitmapModel() : this(800, 600) { }

        /// <summary>
        /// Bitmap 반환
        /// </summary>
        /// <returns>Bitmap</returns>
        public Bitmap GetBitmap() { return _bitmap; }

        /// <summary>
        /// Bitmap 데이터 잠금
        /// </summary>
        public void Lock()
        {
            _bitmapData = _bitmap.LockBits(new Rectangle(0, 0, _width, _height), ImageLockMode.ReadWrite, _bitmap.PixelFormat);
            _scan0 = _bitmapData.Scan0;
        }

        /// <summary>
        /// Bitmap 데이터 해제
        /// </summary>
        public void Unlock()
        {
            if (_bitmapData != null)
            {
                _bitmap.UnlockBits(_bitmapData);
                _bitmapData = null;
            }
        }

        /// <summary>
        /// 특정 좌표에 색상 설정 (LockBits 사용)
        /// </summary>
        /// <param name="x">X 좌표</param>
        /// <param name="y">Y 좌표</param>
        /// <param name="color">색상</param>
        public void SetPixel(int x, int y, Color color)
        {
            if (_bitmapData == null)
            {
                throw new InvalidOperationException("Lock을 먼저 호출해야 합니다.");
            }

            if (x < 0 || x >= _width || y < 0 || y >= _height)
            {
                return; // 범위를 벗어난 좌표는 무시
            }

            // PixelFormat에 따라 처리 방식 변경
            if (_bitmap.PixelFormat == PixelFormat.Format32bppArgb)
            {
                unsafe
                {
                    byte* p = (byte*)_scan0 + y * _bitmapData.Stride + x * 4;
                    p[0] = color.B;
                    p[1] = color.G;
                    p[2] = color.R;
                    p[3] = color.A;
                }
            }
            else
            {
                // 다른 PixelFormat에 대한 처리 (예: Format24bppRgb)
                throw new NotImplementedException("지원하지 않는 PixelFormat입니다.");
            }
        }


        /// <summary>
        /// 특정 좌표의 색상 반환
        /// </summary>
        /// <param name="x">X 좌표</param>
        /// <param name="y">Y 좌표</param>
        /// <returns>색상</returns>
        public Color GetPixel(int x, int y)
        {
            if (_bitmapData == null)
            {
                throw new InvalidOperationException("Lock을 먼저 호출해야 합니다.");
            }

            if (x < 0 || x >= _width || y < 0 || y >= _height)
            {
                throw new ArgumentOutOfRangeException("좌표가 범위를 벗어났습니다.");
            }

            // PixelFormat에 따라 처리 방식 변경
            if (_bitmap.PixelFormat == PixelFormat.Format32bppArgb)
            {
                unsafe
                {
                    byte* p = (byte*)_scan0 + y * _bitmapData.Stride + x * 4;
                    int b = p[0];
                    int g = p[1];
                    int r = p[2];
                    int a = p[3];
                    return Color.FromArgb(a, r, g, b);
                }
            }
            else
            {
                // 다른 PixelFormat에 대한 처리 (예: Format24bppRgb)
                throw new NotImplementedException("지원하지 않는 PixelFormat입니다.");
            }
        }

        /// <summary>
        /// 전체 영역을 특정 색상으로 채우기
        /// </summary>
        /// <param name="color">색상</param>
        public void Clear(Color color)
        {
            Lock();
            try
            {
                // PixelFormat에 따라 처리 방식 변경
                if (_bitmap.PixelFormat == PixelFormat.Format32bppArgb)
                {
                    unsafe
                    {
                        byte* p = (byte*)_scan0;
                        int argb = color.ToArgb();
                        for (int i = 0; i < _width * _height; i++)
                        {
                            p[0] = color.B;
                            p[1] = color.G;
                            p[2] = color.R;
                            p[3] = color.A;
                            p += 4;
                        }
                    }
                }
                else
                {
                    // 다른 PixelFormat에 대한 처리 (예: Format24bppRgb)
                    throw new NotImplementedException("지원하지 않는 PixelFormat입니다.");
                }
            }
            finally
            {
                Unlock();
            }
        }

        /// <summary>
        /// 그림을 파일로 저장
        /// </summary>
        /// <param name="filePath">파일 경로</param>
        public void Save(string filePath)
        {
            try
            {
                _bitmap.Save(filePath, ImageFormat.Png); // 기본적으로 PNG 형식으로 저장
            }
            catch (Exception ex)
            {
                throw new IOException("파일 저장에 실패했습니다.", ex);
            }
        }

        /// <summary>
        /// 파일에서 그림 불러오기
        /// </summary>
        /// <param name="filePath">파일 경로</param>
        public void Load(string filePath)
        {
            try
            {
                Bitmap loadedBitmap = new Bitmap(filePath);

                // 기존 Bitmap 해제
                _bitmap.Dispose();

                _bitmap = loadedBitmap;
                _width = _bitmap.Width;
                _height = _bitmap.Height;
            }
            catch (Exception ex)
            {
                throw new IOException("파일 불러오기에 실패했습니다.", ex);
            }
        }
    }
}