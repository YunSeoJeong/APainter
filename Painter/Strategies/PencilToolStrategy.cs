using Painter.Strategies;

namespace Painter.Strategies
{
    public class PencilToolStrategy : IToolStrategy
    {
        public void Draw(DrawingContext context)
        {
            // 펜슬 특성에 맞는 그리기 알고리즘 구현
            DrawingAlgorithms.DrawLine(context);
        }
    }
}