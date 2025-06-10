using System;
using System.Drawing;
using Painter.Interfaces;

namespace Painter.Models
{
    public class PainterSettingsModel : IPainterSettingsModel
    {
        private ToolType _currentTool = ToolType.Brush;
        private Color _primaryColor = Color.Black;
        private int _brushSize = 3; // 기본 크기 5 → 10으로 증가
        
        public event Action? ToolChanged;
        public event Action? PrimaryColorChanged;
        public event Action? BrushSizeChanged;
        
        public ToolType CurrentTool => _currentTool;
        
        public Color PrimaryColor
        {
            get => _primaryColor;
            set
            {
                if (_primaryColor != value)
                {
                    _primaryColor = value;
                    PrimaryColorChanged?.Invoke();
                }
            }
        }
        
        public int BrushSize
        {
            get => _brushSize;
            set
            {
                if (_brushSize != value)
                {
                    _brushSize = value;
                    BrushSizeChanged?.Invoke();
                }
            }
        }
        
        public void SetTool(ToolType tool)
        {
            if (_currentTool != tool)
            {
                _currentTool = tool;
                ToolChanged?.Invoke();
            }
        }
    }
}