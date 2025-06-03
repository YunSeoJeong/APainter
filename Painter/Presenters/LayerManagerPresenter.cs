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
            // 새 레이어 생성 (기본 크기 800x600)
            // TODO 현재 캔버스 크기에 맞추어 레이어 생성
            var newLayer = new Bitmap(800, 600);
            using (var g = Graphics.FromImage(newLayer))
            {
                g.Clear(Color.Transparent);
            }
            
            _layers.Add(newLayer);
            UpdateLayerList();
        }

        private void UpdateLayerList()
        {
            // 레이어 이름 목록 생성
            var layerNames = _layers.Select((_, index) => $"Layer {index + 1}").ToList();
            _view.UpdateLayerList(layerNames);
        }
    }
}