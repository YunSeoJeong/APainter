using System;
using System.Windows.Forms;
using Painter.Interfaces;

namespace Painter.Views
{
    /// <summary>ToolboxView 클래스</summary>
    public class ToolboxView : FlowLayoutPanel, IToolboxView
    {
        public Button? BtnBrush { get; private set; }
        public Button? BtnPencil { get; private set; }
        public Button? BtnEraser { get; private set; }
        public Button? BtnSpray { get; private set; }
        public event EventHandler? BrushSelected;
        public event EventHandler? PencilSelected;
        public event EventHandler? EraserSelected;
        public event EventHandler? SpraySelected;

        /// <summary>ToolboxView 생성자</summary>
        public ToolboxView()
        {
            Initialize();
        }

        /// <summary>UI 컴포넌트 초기화</summary>
        public void Initialize()
        {
            // 레이아웃 설정
            FlowDirection = FlowDirection.TopDown;
            WrapContents = false;
            Padding = new Padding(0, 30, 0, 0);

            // 도구 버튼 생성 및 추가
            BtnBrush = CreateToolButton("Brush", OnBrushSelected);
            BtnPencil = CreateToolButton("Pencil", OnPencilSelected);
            BtnEraser = CreateToolButton("Eraser", OnEraserSelected);
            BtnSpray = CreateToolButton("Spray", OnSpraySelected);

            Controls.Add(BtnBrush);
            Controls.Add(BtnPencil);
            Controls.Add(BtnEraser);
            Controls.Add(BtnSpray);
            SetActiveTool(ToolType.Brush);
        }

        /// <summary>도구 버튼 생성 메서드</summary>
        /// <param name="text">버튼에 표시할 텍스트</param>
        /// <param name="handler">버튼 클릭 시 실행할 이벤트 핸들러</param>
        /// <returns>생성된 Button 객체</returns>
        protected virtual Button CreateToolButton(string text, EventHandler handler)
        {
            var button = new Button
            {
                Text = text,
                Width = 100,
                Height = 30,
                BackColor = System.Drawing.SystemColors.Control
            };
            button.Click += handler;
            return button;
        }

        /// <summary>브러시 도구 선택 이벤트 핸들러</summary>
        private void OnBrushSelected(object? sender, EventArgs e)
        {
            BrushSelected?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>연필 도구 선택 이벤트 핸들러</summary>
        private void OnPencilSelected(object? sender, EventArgs e)
        {
            PencilSelected?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>지우개 도구 선택 이벤트 핸들러</summary>
        private void OnEraserSelected(object? sender, EventArgs e)
        {
            EraserSelected?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>스프레이 도구 선택 이벤트 핸들러</summary>
        private void OnSpraySelected(object? sender, EventArgs e)
        {
           SpraySelected?.Invoke(this, EventArgs.Empty);
        }

        public void SetActiveTool(ToolType toolType)
        {
            BtnBrush!.BackColor = System.Drawing.SystemColors.Control;
            BtnPencil!.BackColor = System.Drawing.SystemColors.Control;
            BtnEraser!.BackColor = System.Drawing.SystemColors.Control;
            BtnSpray!.BackColor = System.Drawing.SystemColors.Control;

            switch (toolType)
            {
                case ToolType.Brush:
                    BtnBrush.BackColor = System.Drawing.Color.LightBlue;
                    break;
                case ToolType.Pencil:
                    BtnPencil.BackColor = System.Drawing.Color.LightBlue;
                    break;
                case ToolType.Eraser:
                    BtnEraser.BackColor = System.Drawing.Color.LightBlue;
                    break;
               case ToolType.Spray:
                    BtnSpray.BackColor = System.Drawing.Color.LightBlue;
                    break;
            }
        }
    }
}