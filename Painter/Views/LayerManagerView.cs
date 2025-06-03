using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Painter.Interfaces;
using Painter.Presenters;

namespace Painter.Views
{
    /// <summary>LayerManagerView 클래스</summary>
    public class LayerManagerView : UserControl, ILayerManagerView
    {
        public ListBox? LayerListBox { get; private set; }
        public Button? BtnAddLayer { get; private set; }
        public event EventHandler? AddLayerClicked;

        /// <summary>LayerManagerView 생성자</summary>
        public LayerManagerView()
        {
            Initialize();
        }

        /// <summary>UI 컴포넌트 초기화</summary>
        public void Initialize()
        {
            // 레이어 목록 초기화
            LayerListBox = new ListBox
            {
                Dock = DockStyle.Fill,
                SelectionMode = SelectionMode.MultiExtended
            };

            // 레이어 추가 버튼 초기화
            BtnAddLayer = new Button
            {
                Text = "Add Layer",
                Dock = DockStyle.Bottom
            };
            BtnAddLayer.Click += OnAddLayerClicked;

            // 레이아웃 구성
            var splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal
            };
            splitContainer.Panel1.Controls.Add(LayerListBox);
            splitContainer.Panel2.Controls.Add(BtnAddLayer);

            Controls.Add(splitContainer);
        }

        /// <summary>레이어 추가 버튼 클릭 이벤트 핸들러</summary>
        private void OnAddLayerClicked(object? sender, EventArgs e)
        {
            AddLayerClicked?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>레이어 목록 업데이트</summary>
        /// <param name="layerNames">업데이트할 레이어 이름 목록</param>
        public void UpdateLayerList(IEnumerable<string> layerNames)
        {
            if (LayerListBox != null && LayerListBox.InvokeRequired)
            {
                LayerListBox.Invoke(new Action(() => 
                {
                    LayerListBox.Items.Clear();
                    foreach (var name in layerNames)
                    {
                        LayerListBox.Items.Add(name);
                    }
                }));
                return;
            }

            if (LayerListBox != null)
            {
                LayerListBox.Items.Clear();
                foreach (var name in layerNames)
                {
                    LayerListBox.Items.Add(name);
                }
            }
        }

        public void UpdateLayerList(List<string> layerNames)
        {
            throw new NotImplementedException();
        }

        public void ShowError(string message)
        {
            throw new NotImplementedException();
        }
    }
}