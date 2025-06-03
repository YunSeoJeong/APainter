using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Painter.Interfaces;
using Painter.Models;

namespace Painter.Presenters
{
    public class LayerManagerPresenter
    {
        private readonly ILayerManagerView _view;
        private readonly IBitmapModel _bitmapModel;
        private readonly List<Bitmap> _layers = new List<Bitmap>();

        public LayerManagerPresenter(ILayerManagerView view, IBitmapModel bitmapModel)
        {
            _view = view;
            _bitmapModel = bitmapModel;
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            _view.AddLayerClicked += OnAddLayerClicked;
        }

        public void OnAddLayerClicked(object? sender, EventArgs e)
        {
            try
            {
                // 현재 캔버스 크기에 맞추어 레이어 생성
                var newLayer = new Bitmap(_bitmapModel.Width, _bitmapModel.Height);
                using (var g = Graphics.FromImage(newLayer))
                {
                    g.Clear(Color.Transparent);
                }
                
                _layers.Add(newLayer);
                UpdateLayerList();
            }
            catch (Exception ex)
            {
                // 표준화된 오류 처리
                _view.ShowError($"레이어 추가 실패: {ex.Message}");
            }
        }

        private void UpdateLayerList()
        {
            try
            {
                // 레이어 이름 목록 생성
                var layerNames = _layers.Select((_, index) => $"레이어 {index + 1}").ToList();
                _view.UpdateLayerList(layerNames);
            }
            catch (Exception ex)
            {
                _view.ShowError($"레이어 목록 업데이트 실패: {ex.Message}");
            }
        }
    }
}