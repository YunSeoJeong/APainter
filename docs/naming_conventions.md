# APainter 명명 규칙

## 1. 일반 규칙

*   **식별자:** 의미를 명확하게 전달하고, 간결하게 작성합니다.
*   **대소문자:** C# 표준 명명 규칙을 따릅니다 (PascalCase, camelCase).
*   **약어:** 일반적으로 사용되는 약어 외에는 사용을 자제합니다.
*   **접두사/접미사:** 특별한 경우 외에는 사용을 자제합니다.

## 2. 클래스 및 구조체

*   **이름:** PascalCase를 사용합니다.
*   **예시:** `BitmapModel`, `MainForm`

## 3. 인터페이스

*   **이름:** PascalCase를 사용하고, `I` 접두사를 붙입니다.
*   **예시:** `IView`, `IModel`

## 4. 메서드

*   **이름:** PascalCase를 사용합니다.
*   **예시:** `GetBitmap()`, `SaveToFile()`

## 5. 속성 (Properties)

*   **이름:** PascalCase를 사용합니다.
*   **예시:** `Width`, `Height`

## 6. 필드

*   **이름:** _camelCase를 사용합니다.
*   **접근 제한자:** private으로 선언합니다.
*   **예시:** `_bitmap`, `_filePath`

## 7. 지역 변수

*   **이름:** camelCase를 사용합니다.
*   **예시:** `color`, `x`, `y`

## 8. 상수

*   **이름:** 모두 대문자로 작성하고, 단어 사이에 밑줄(_)을 사용합니다.
*   **예시:** `MAX_WIDTH`, `DEFAULT_COLOR`

## 9. 이벤트

*   **이름:** PascalCase를 사용하고, `EventHandler` 접미사를 붙입니다.
*   **예시:** `BrushButtonClicked`, `FileSaveMenuItemClicked`

## 10. 매개변수

*   **이름:** camelCase를 사용합니다.
*   **예시:** `x`, `y`, `color`