using Painter.Interfaces;
using Painter.Strategies;

namespace Painter.Strategies
{
    public interface IToolStrategyFactory
    {
        IToolStrategy CreateToolStrategy(ToolType toolType);
    }
}