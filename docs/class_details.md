# APainter MVP 아키텍처 클래스 상세 필드 및 메서드

## 1. 인터페이스

### 1.1. IView
*   **메서드:**
    *   void Initialize(): UI 초기화

### 1.2. ICanvasView : IView
*   **메서드:**
    *   void SetBitmap(Bitmap bitmap): 그림 영역 업데이트
    *   event MouseEventHandler MouseDownEvent
    *   event MouseEventHandler MouseMoveEvent
    *   event MouseEventHandler MouseUpEvent

### 1.3. IToolboxView : IView
*   **메서드:**
    *   event EventHandler BrushSelected
    *   event EventHandler PencilSelected

## 2. Model

### 2.1. BitmapModel
*   **필드:**
    *   Bitmap _bitmap: 그림 데이터 저장 및 관리
    *   int _width: 그림 폭
    *   int _height: 그림 높이
    *   BitmapData _bitmapData: Lock된 Bitmap 데이터
    *   IntPtr _scan0: 스캔 시작 주소
*   **생성자:**
    *   BitmapModel(int width, int height): 지정된 폭과 높이로 BitmapModel 초기화
    *   BitmapModel(): 기본 크기(800x600)로 BitmapModel 초기화
*   **메서드:**
    *   Bitmap GetBitmap(): Bitmap 반환
    *   void Lock(): Bitmap 데이터 잠금
    *   void Unlock(): Bitmap 데이터 해제
    *   void SetPixel(int x, int y, Color color): 특정 좌표에 색상 설정 (LockBits 사용)
    *   Color GetPixel(int x, int y): 특정 좌표의 색상 반환 (LockBits 사용)
    *   void Clear(Color color): 전체 영역을 특정 색상으로 채우기 (Graphics 사용)
    *   void Save(string filePath): 그림을 파일로 저장
    *   void Load(string filePath): 파일에서 그림 불러오기

### 2.2. ComfyUIModel
*   **필드:**
    *   HttpClient _httpClient: ComfyUI API 통신
    *   string _apiEndpoint: ComfyUI API 엔드포인트
*   **생성자:**
    *   ComfyUIModel(HttpClient httpClient, string apiEndpoint): HttpClient와 API 엔드포인트를 사용하여 ComfyUIModel 초기화
*   **메서드:**
    *   Task<string> GenerateImage(string prompt): ComfyUI API를 사용하여 이미지 생성
    *   Task<ImageInfo> GetImageInfo(string imageId): 이미지 정보 가져오기
    *   Task<Workflow> GetWorkflow(string workflowId): 워크플로우 정보 가져오기

### 2.3. FileModel
*   **필드:**
    *   string filePath: 파일 경로
*   **메서드:**
    *   void SaveToFile(Bitmap bitmap, string filePath): Bitmap 데이터를 파일로 저장
    *   Bitmap LoadFromFile(string filePath): 파일에서 Bitmap 데이터 불러오기

### 2.4. ImageInfo
*   **생성자:**
    *   ImageInfo(): ImageInfo 초기화

### 2.5. Workflow
*   **생성자:**
    *   Workflow(): Workflow 초기화

## 3. View

### 3.1. MainForm
*   **필드:**
    *   ToolboxView toolboxView: 도구 상자 View
    *   CanvasView canvasView: 그림 영역 View
    *   LayerManagerView layerManagerView: 레이어 관리 View
    *   MenuView menuView: 메뉴 View
*   **메서드:**
    *   void Initialize(): UI 초기화

### 3.2. CanvasView : ICanvasView
*   **필드:**
    *   PictureBox pictureBox: 그림 표시 영역
*   **메서드:**
    *   void Initialize(): UI 초기화
    *   void SetBitmap(Bitmap bitmap)
    *   event MouseEventHandler MouseDownEvent
    *   event MouseEventHandler MouseMoveEvent
    *   event MouseEventHandler MouseUpEvent

### 3.3. ToolboxView : IToolboxView
*   **필드:**
    *   Button btnBrush: 붓 도구 버튼
    *   Button btnPencil: 연필 도구 버튼
*   **메서드:**
    *   void Initialize(): UI 초기화
    *   event EventHandler BrushSelected
    *   event EventHandler PencilSelected

### 3.4. LayerManagerView : IView
*   **필드:**
    *   ListBox layerListBox: 레이어 목록 표시
    *   Button btnAddLayer: 레이어 추가 버튼
*   **메서드:**
    *   void Initialize(): UI 초기화
    *   event EventHandler AddLayerClicked

### 3.5. MenuView : IView
*   **필드:**
    *   MenuItem fileSaveMenuItem: 파일 저장 메뉴
*   **메서드:**
    *   void Initialize(): UI 초기화
    *   event EventHandler FileSaveClicked

## 4. Presenter

### 4.1. MainPresenter
*   **필드:**
    *   IMainView view
    *   BitmapModel bitmapModel
    *   ComfyUIModel comfyUIModel
    *   FileModel fileModel
*   **메서드:**
    *   MainPresenter(IMainView view, BitmapModel bitmapModel, ComfyUIModel comfyUIModel, FileModel fileModel): 생성자
    *   void Run(): 어플리케이션 실행

### 4.2. CanvasPresenter
*   **필드:**
    *   ICanvasView view
    *   BitmapModel model
*   **메서드:**
    *   CanvasPresenter(ICanvasView view, BitmapModel model): 생성자
    *   void OnMouseDown(int x, int y): 마우스 다운 이벤트 처리
    *   void OnMouseMove(int x, int y): 마우스 이동 이벤트 처리
    *   void OnMouseUp(int x, int y): 마우스 업 이벤트 처리
    *   void UpdateView(): 뷰 업데이트

### 4.3. ToolboxPresenter
*   **필드:**
    *   IToolboxView view
    *   BitmapModel model
*   **메서드:**
    *   ToolboxPresenter(IToolboxView view, BitmapModel model): 생성자
    *   void OnBrushSelected(): 붓 도구 클릭 처리
    *   void OnPencilSelected(): 연필 도구 클릭 처리

### 4.4. LayerManagerPresenter
*   **필드:**
    *   ILayerManagerView view
    *   BitmapModel model
*   **메서드:**
    *   LayerManagerPresenter(ILayerManagerView view, BitmapModel model): 생성자
    *   void OnAddLayerClicked(): 레이어 추가 버튼 클릭 처리

### 4.5. MenuPresenter
*   **필드:**
    *   IMenuView view
    *   FileModel model
*   **메서드:**
    *   MenuPresenter(IMenuView view, FileModel model): 생성자
    *   void OnFileSaveClicked(): 파일 저장 메뉴 클릭 처리