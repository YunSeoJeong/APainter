# APainter

AI 기반 그림 도구

## 설명
APainter는 C#으로 구현된 그림 도구로, ComfyUI와의 통합을 통해 AI 기반 이미지 생성 기능을 제공합니다. MVP 아키텍처 기반으로 설계되어 뷰, 프레젠터, 모델 간의 명확한 분리를 유지합니다.

## 주요 기능
- 레이어 관리
- 캔버스 편집
- 다양한 도구 전략 (펜, 브러시, 지우개)
- 설정 관리
- 파일 입출력

## 프로젝트 구조
```
Painter/
├── Interfaces/       - 인터페이스 정의
├── Models/           - 데이터 모델 (비트맵, 설정, ComfyUI 통합)
├── Presenters/       - 비즈니스 로직 (프레젠터 클래스)
├── Strategies/       - 도구 전략 패턴 구현
├── Views/            - UI 구성 요소
└── Painter.Test/     - 단위 테스트
```

## 설치
1. [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) 설치
2. NuGet 패키지 복원:
```bash
dotnet restore Painter.sln
```
3. 빌드:
```bash
dotnet build Painter.sln
```

## 사용법
1. Visual Studio에서 솔루션 열기
2. 또는 CLI에서 실행:
```bash
dotnet run --project Painter
```
3. 도구 상자에서 원하는 도구 선택
4. 캔버스에서 그림 그리기
5. 설정 메뉴에서 AI 파라미터 조정

## 기여
1. [코드 가이드라인](docs/code_guidelines.md) 및 [네이밍 컨벤션](docs/naming_conventions.md) 참조
2. 새로운 브랜치 생성
3. 변경 사항 커밋
4. Pull request 생성

## 라이선스
이 프로젝트는 MIT 라이선스를 따릅니다. 자세한 내용은 [LICENSE](LICENSE) 파일을 참조하세요.