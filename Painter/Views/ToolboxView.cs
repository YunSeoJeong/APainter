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
        public event EventHandler? BrushSelected;
        public event EventHandler? PencilSelected;
        public event EventHandler? EraserSelected;

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

            // 도구 버튼 생성 및 추가
            BtnBrush = CreateToolButton("Brush", OnBrushSelected);
            BtnPencil = CreateToolButton("Pencil", OnPencilSelected);
            BtnEraser = CreateToolButton("Eraser", OnEraserSelected);

            Controls.Add(BtnBrush);
            Controls.Add(BtnPencil);
            Controls.Add(BtnEraser);
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
                Height = 30
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
    }
}