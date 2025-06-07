using Painter.Interfaces;
using Painter.Strategies;

namespace Painter.Strategies
{
    public class ToolStrategyFactory : IToolStrategyFactory
    {
        public IToolStrategy CreateToolStrategy(ToolType toolType)
        {
            return toolType switch
            {
                ToolType.Brush => new BrushToolStrategy(),
                ToolType.Pencil => new PencilToolStrategy(),
                ToolType.Eraser => new EraserToolStrategy(),
                ToolType.Spray => new SprayToolStrategy(), // 스프레이 도구 추가
                _ => new PencilToolStrategy()
            };
        }
    }
}