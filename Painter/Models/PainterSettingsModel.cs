using System;
using System.Drawing;
using Painter.Interfaces;

namespace Painter.Models
{
    public class PainterSettingsModel : IPainterSettingsModel
    {
        private ToolType _currentTool = ToolType.Brush;
        private Color _primaryColor = Color.Black;
        private int _brushSize = 5;
        
        public event Action? ToolChanged;
        
        public ToolType CurrentTool => _currentTool;
        public Color PrimaryColor
        {
            get => _primaryColor;
            set => _primaryColor = value;
        }
        public int BrushSize
        {
            get => _brushSize;
            set => _brushSize = value;
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