using Painter.Strategies;

namespace Painter.Strategies
{
    public class BrushToolStrategy : IToolStrategy
    {
        public void Draw(DrawingContext context)
        {
            DrawingAlgorithms.DrawLine(context);
        }
    }
}