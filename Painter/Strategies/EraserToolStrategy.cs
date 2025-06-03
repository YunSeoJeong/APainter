using Painter.Strategies;
using System.Drawing;

namespace Painter.Strategies
{
    public class EraserToolStrategy : IToolStrategy
    {
        public void Draw(DrawingContext context)
        {
            // 새 컨텍스트 생성 (배경색으로 설정)
            var eraserContext = new DrawingContext(
                context.StartPoint,
                context.EndPoint,
                Color.White, // 배경색
                context.BrushSize,
                context.SetPixel
            );
            
            DrawingAlgorithms.DrawLine(eraserContext);
        }
    }
}