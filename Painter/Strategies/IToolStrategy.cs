using Painter.Strategies;

namespace Painter.Strategies
{
    public interface IToolStrategy
    {
        void Draw(DrawingContext context);
        float Radius { get; } // 도구의 현재 반경 반환
        void SetBrushSize(float brushSize); // 브러시 크기 설정 메서드 추가
    }
}