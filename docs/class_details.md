# APainter MVP 아키텍처 클래스 상세 필드 및 메서드

## 1. 인터페이스 계층

### 1.1. View-Presenter 인터페이스
#### IView
*   **메서드:**
    *   void Initialize(): UI 초기화

#### IMainView : IView
*   **메서드:**
    *   void SetChildViews(): 하위 뷰 초기화

#### ICanvasView : IView
*   **메서드:**
    *   void SetBitmap(Bitmap bitmap): 그림 영역 업데이트
    *   event MouseEventHandler MouseDownEvent
    *   event MouseEventHandler MouseMoveEvent
    *   event MouseEventHandler MouseUpEvent
    *   event EventHandler<ZoomEventArgs> ZoomInClicked
    *   event EventHandler<ZoomEventArgs> ZoomOutClicked
    *   void SetZoom(float zoomFactor, PointF center)
    *   event MouseEventHandler MouseWheel

#### IToolboxView : IView
*   **메서드:**
    *   event EventHandler BrushSelected
    *   event EventHandler PencilSelected
    *   event EventHandler EraserSelected
    *   event EventHandler SpraySelected

#### ILayerManagerView : IView
*   **메서드:**
    *   event EventHandler AddLayerClicked

#### IMenuView : IView
*   **메서드:**
    *   event EventHandler FileSaveClicked

### 1.2. Presenter-Model 인터페이스
#### IBitmapModel
*   **메서드:**
    *   Bitmap GetBitmap()
    *   void Lock()
    *   void Unlock()
    *   void SetPixel(int x, int y, Color color)
    *   Color GetPixel(int x, int y)
    *   void Clear(Color color)
    *   void Dispose()

#### IFileModel
*   **메서드:**
    *   void SaveToFile(Bitmap bitmap, string filePath)
    *   Bitmap LoadFromFile(string filePath)

#### IComfyUIModel
*   **메서드:**
    *   Task<string> GenerateImage(string prompt)
    *   Task<ImageInfo> GetImageInfo(string imageId)
    *   Task<Workflow> GetWorkflow(string workflowId)

#### IPainterSettingsModel
*   **속성:**
    *   ToolType CurrentTool { get; }
    *   Color PrimaryColor { get; set; }
    *   int BrushSize { get; set; }
    *   float ZoomFactor { get; set; }
*   **이벤트:**
    *   event Action ToolChanged
    *   event Action PrimaryColorChanged
    *   event Action BrushSizeChanged
    *   event Action ZoomFactorChanged
*   **메서드:**
    *   void SetTool(ToolType tool)

## 2. Model 계층

### 2.1. BitmapModel : IBitmapModel
*   **필드:**
    *   Bitmap _bitmap
    *   int _width
    *   int _height
    *   BitmapData _bitmapData
    *   IntPtr _scan0
    *   bool _isLocked
*   **생성자:**
    *   BitmapModel(int width, int height)
    *   BitmapModel()
*   **메서드:**
    *   Bitmap GetBitmap()
    *   void Lock()
    *   void Unlock()
    *   void SetPixel(int x, int y, Color color)
    *   Color GetPixel(int x, int y)
    *   void Clear(Color color)
    *   void Dispose()
*   **속성:**
    *   int Width
    *   int Height

### 2.2. ComfyUIModel : IComfyUIModel
*   **필드:**
    *   HttpClient _httpClient
    *   string _apiEndpoint
*   **생성자:**
    *   ComfyUIModel(HttpClient httpClient, string apiEndpoint)
*   **메서드:**
    *   Task<string> GenerateImage(string prompt)
    *   Task<ImageInfo> GetImageInfo(string imageId)
    *   Task<Workflow> GetWorkflow(string workflowId)

### 2.3. FileModel : IFileModel
*   **메서드:**
    *   void SaveToFile(Bitmap bitmap, string filePath)
    *   Bitmap LoadFromFile(string filePath)

### 2.4. ImageInfo
*   (아직 구현되지 않음)

### 2.5. Workflow
*   (아직 구현되지 않음)

### 2.6. PainterSettingsModel : IPainterSettingsModel
*   **필드:**
    *   ToolType _currentTool
    *   Color _primaryColor
    *   int _brushSize
    *   float _zoomFactor
*   **속성:**
    *   ToolType CurrentTool { get; }
    *   Color PrimaryColor { get; set; }
    *   int BrushSize { get; set; }
    *   float ZoomFactor { get; set; }
*   **이벤트:**
    *   event Action ToolChanged
    *   event Action PrimaryColorChanged
    *   event Action BrushSizeChanged
    *   event Action ZoomFactorChanged
*   **메서드:**
    *   void SetTool(ToolType tool)

## 3. View 계층

### 3.1. MainForm : IMainView
*   **필드:**
    *   ToolboxView _toolboxView
    *   CanvasView _canvasView
    *   LayerManagerView _layerManagerView
    *   MenuView _menuView
*   **메서드:**
    *   void Initialize()
    *   void SetChildViews()

### 3.2. CanvasView : ICanvasView
*   **필드:**
    *   PictureBox _pictureBox
*   **메서드:**
    *   void Initialize()
    *   void SetBitmap(Bitmap bitmap)
    *   event MouseEventHandler MouseDownEvent
    *   event MouseEventHandler MouseMoveEvent
    *   event MouseEventHandler MouseUpEvent
    *   event MouseEventHandler MouseWheel

### 3.3. ToolboxView : IToolboxView
*   **필드:**
    *   Button _btnBrush
    *   Button _btnPencil
*   **메서드:**
    *   void Initialize()
    *   event EventHandler BrushSelected
    *   event EventHandler PencilSelected
    *   event EventHandler EraserSelected
    *   event EventHandler SpraySelected
    *   event EventHandler PencilSelected

### 3.4. LayerManagerView : ILayerManagerView
*   **필드:**
    *   ListBox _layerListBox
    *   Button _btnAddLayer
*   **메서드:**
    *   void Initialize()
    *   event EventHandler AddLayerClicked

### 3.5. MenuView : IMenuView
*   **필드:**
    *   MenuItem _fileSaveMenuItem
*   **메서드:**
    *   void Initialize()
    *   event EventHandler FileSaveClicked

## 4. Presenter 계층

### 4.1. MainPresenter
*   **필드:**
    *   IMainView _view
    *   IBitmapModel _bitmapModel
    *   IComfyUIModel _comfyUIModel
    *   IFileModel _fileModel
    *   IPainterSettingsModel _settingsModel
*   **메서드:**
    *   MainPresenter(IMainView view, IBitmapModel bitmapModel, IComfyUIModel comfyUIModel, IFileModel fileModel, IPainterSettingsModel settingsModel)
    *   void Run()

### 4.2. CanvasPresenter
*   **필드:**
    *   ICanvasView _view
    *   IBitmapModel _bitmapModel
    *   IPainterSettingsModel _settingsModel
*   **메서드:**
    *   CanvasPresenter(ICanvasView view, IBitmapModel bitmapModel, IPainterSettingsModel settingsModel)
    *   void OnMouseDown(int x, int y)
    *   void OnMouseMove(int x, int y)
    *   void OnMouseUp(int x, int y)
    *   void UpdateView()
    *   void OnZoomInClicked(object sender, ZoomEventArgs e)
    *   void OnZoomOutClicked(object sender, ZoomEventArgs e)
    *   void OnMouseWheel(object sender, MouseEventArgs e)

### 4.3. ToolboxPresenter
*   **필드:**
    *   IToolboxView _view
    *   IPainterSettingsModel _settingsModel
*   **메서드:**
    *   ToolboxPresenter(IToolboxView view, IPainterSettingsModel settingsModel)
    *   void OnBrushSelected()
    *   void OnSpraySelected()
    *   void OnPencilSelected()

### 4.4. LayerManagerPresenter
*   **필드:**
    *   ILayerManagerView _view
    *   IBitmapModel _bitmapModel
*   **메서드:**
    *   LayerManagerPresenter(ILayerManagerView view, IBitmapModel bitmapModel)
    *   void OnAddLayerClicked()

### 4.5. MenuPresenter
*   **필드:**
    *   IMenuView _view
    *   IFileModel _fileModel
*   **메서드:**
    *   MenuPresenter(IMenuView view, IFileModel fileModel)
    *   void OnFileSaveClicked()

## 5. 기타 클래스

### 5.1. ZoomEventArgs : EventArgs
*   **속성:**
    *   PointF Center { get; set; }