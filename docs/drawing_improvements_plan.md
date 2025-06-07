# Drawing Tools Improvement Plan (Updated)

## 문제 진단
- 안티앨리어싱이 픽셀 겹침으로 인해 효과 미흡
- 빠른 그리기 시 선이 끊어지는 현상
- 투명도 누적로 인한 색상 과포화

## 개선 방안

### 1. 포인트 밀도 최적화
```csharp
// DrawingAlgorithms.cs
public static void DrawCurve(List<Point> points, DrawingContext context)
{
    // 동적 보간 간격 계산: 속도에 따라 간격 조절
    float step = CalculateDynamicStep(points, context);
    
    for (float t = 0; t <= 1; t += step)
    {
        // [기존 Catmull-Rom 연산]
        DrawPoint(..., context, preventOverlap: true); // 중복 픽셀 방지 옵션
    }
}

private static float CalculateDynamicStep(List<Point> points)
{
    // 점 간 평균 거리 계산
    double totalDist = 0;
    for (int i = 1; i < points.Count; i++)
    {
        totalDist += Distance(points[i-1], points[i]);
    }
    double avgDist = totalDist / (points.Count - 1);
    
    // 거리 기반 동적 스텝 계산
    return (float)Math.Clamp(1.0 / avgDist, 0.01, 0.1);
}
```

### 2. 알파 누적 방지
```csharp
// BitmapModel.cs
private static Color BlendColors(Color bg, Color fg)
{
    float alpha = fg.A / 255.0f;
    
    // 최대 알파 제한 (80%)
    alpha = Math.Min(alpha, 0.8f);
    
    // 개선된 블렌딩 공식
    int r = (int)(fg.R * alpha + bg.R * (1 - alpha));
    int g = (int)(fg.G * alpha + bg.G * (1 - alpha));
    int b = (int)(fg.B * alpha + bg.B * (1 - alpha));
    
    // 알파는 배경 알파 유지 (누적 방지)
    return Color.FromArgb(bg.A, r, g, b);
}
```

### 3. 속도 계산 안정화
```csharp
// BrushToolStrategy.cs
private double CalculateSpeed(Point current, Point previous)
{
    // 저역통과 필터 적용 (과도한 변화 방지)
    const double filterFactor = 0.2;
    double currentSpeed = base.CalculateSpeed(current, previous);
    _smoothedSpeed = _smoothedSpeed * (1 - filterFactor) + currentSpeed * filterFactor;
    
    return _smoothedSpeed;
}

// 민감도 계수 조정
int adjustedSize = (int)(baseSize * (1 - Math.Clamp(_smoothedSpeed * 0.3, 0, 0.5)));
```

## 테스트 계획
1. 고속 드로잉 테스트: 선의 연속성 확인
2. 중첩 영역 테스트: 색상 과포화 여부 확인
3. 다양한 곡률 테스트: 부드러운 곡선 표현 확인

수정 사항 적용 후 다시 테스트를 요청드리겠습니다.