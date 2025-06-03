using Painter.Strategies;

namespace Painter.Strategies
{
    public interface IToolStrategy
    {
        void Draw(DrawingContext context);
    }
}