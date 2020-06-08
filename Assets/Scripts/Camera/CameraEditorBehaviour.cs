using HexWorld;
using System.CodeDom;
using UnityEngine;

public class CameraEditorBehaviour : InputCamera
{
    //---- Events
    //-----------
    public MouseRayCast OnMouseHoverHexTile;
    public MouseRayCast OnMouseClickHexTile;

    //---- Variables
    //--------------
    [SerializeField] private BoardEditorPreferences _preferences;    
    [SerializeField] private GameObject _mouseTarget;
    [SerializeField] private Transform _cameraTarget;

    // Inputs
    private Vector3 _lastMousePosition;
    private Vector3 _mousePosition;
    private float _zoomValue;
    private bool _mouseBtn0Down;
    private bool _mouseBtn1Down;

    //---- Unity
    //----------
    protected override void Awake()
    {
        base.Awake();
        _cameraTarget.rotation = transform.rotation;
        _cameraTarget.position = transform.position;
    }

    private void Start()
    {
        _inputController.Enable(true);
    }

    protected override void OnEnable()
    {
        _inputController.OnMousePosition += OnMousePosition;
        _inputController.OnMouseLeftBtnDown += OnMouseBtn0Down;
        _inputController.OnMouseRightBtnDown += OnMouseBtn1Down;
        _inputController.OnMouseScrollY += OnMouseScroll;
        base.OnEnable();
    }

    private void OnDisable()
    {
        _inputController.OnMousePosition -= OnMousePosition;
        _inputController.OnMouseLeftBtnDown -= OnMouseBtn0Down;
        _inputController.OnMouseRightBtnDown -= OnMouseBtn1Down;
        _inputController.OnMouseScrollY -= OnMouseScroll;
    }

    private void FixedUpdate()
    {        
        LerpToTarget();    
    }

    private void LateUpdate()
    {
        if(RaycastToTile(out HexTile tile))
        {
            OnMouseHoverHexTile?.Invoke(tile);
            if (_mouseBtn0Down)
            {
                OnMouseClickHexTile?.Invoke(tile);
            }
        }

        if (_mouseBtn1Down)
        {
            PanCamera();
        }
    }

    //---- Public
    //-----------
    public InputController Input => _inputController;

    public void JumpTo(Vector3 pos)
    {
        transform.position = pos;
        _cameraTarget.position = pos;
    }

    //---- Movement
    //-------------
    private void LerpToTarget()
    {
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, _cameraTarget.position,
            Time.fixedDeltaTime * _preferences.CameraSpeed);
    }

    private void PanCamera()
    {
        Vector3 lastPos = _camera.ScreenToWorldPoint(_lastMousePosition);
        lastPos.y = _camera.transform.position.y;

        Vector3 currentPos = _mousePosition;
        currentPos.z = _camera.transform.position.y;
        currentPos = _camera.ScreenToWorldPoint(currentPos);
        currentPos.y = _camera.transform.position.y;

        Vector3 delta = currentPos - lastPos;
        _cameraTarget.position += delta;

        _lastMousePosition = _mousePosition;
        _lastMousePosition.z = _camera.transform.position.y;
    }   

    //---- Events
    //-----------
    private void OnMousePosition(Vector3 position)
    {
        _mousePosition = position;
    }

    private void OnMouseBtn0Down(bool isDown)
    {        
        _mouseBtn0Down = isDown;
    }

    private void OnMouseBtn1Down(bool isDown)
    {        
        _mouseBtn1Down = isDown;
        if(_mouseBtn1Down)
        {
            _lastMousePosition = _mousePosition;
        }
    }

    private void OnMouseScroll(float value)
    {        
        _zoomValue += (value *  _preferences.ZoomInterval) * -1.0f;
        _zoomValue = Mathf.Clamp(_zoomValue, _preferences.ZoomClamp.x, _preferences.ZoomClamp.y);
        _camera.orthographicSize = _zoomValue;
    }
}
