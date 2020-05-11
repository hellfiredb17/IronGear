using System.CodeDom;
using UnityEngine;

[RequireComponent(typeof(Camera), typeof(InputController))]
public class CameraEditorBehaviour : MonoBehaviour
{
    //---- Delegate
    //-------------
    public delegate void MouseRayCast(HexTile tile);

    //---- Events
    //-----------
    public MouseRayCast OnMouseHoverHexTile;
    public MouseRayCast OnMouseClickHexTile;

    //---- Variables
    //--------------
    [SerializeField] private BoardEditorPreferences _preferences;
    [SerializeField] private Camera _camera;
    [SerializeField] private InputController _inputController;
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
    private void Awake()
    {
        _camera = GetComponent<Camera>();        
        _inputController = GetComponent<InputController>();

        _cameraTarget.rotation = transform.rotation;
        _cameraTarget.position = transform.position;
    }

    private void Start()
    {
        _inputController.Enable(true);
    }

    private void OnEnable()
    {
        _inputController.OnMousePosition += OnMousePosition;
        _inputController.OnMouseLeftBtnDown += OnMouseBtn0Down;
        _inputController.OnMouseRightBtnDown += OnMouseBtn1Down;
        _inputController.OnMouseScrollY += OnMouseScroll;
        _inputController.Set(); // sets init values
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
        Raycast();

        if (_mouseBtn1Down)
        {
            PanCamera();
        }
    }

    //---- Public
    //-----------
    public Camera Camera => _camera;

    public InputController Input => _inputController;

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

    private void Raycast()
    {
        Vector3 mousePos = _mousePosition;
        mousePos.z = _camera.transform.position.y;
        Vector3 mousePointerToWorld = _camera.ScreenToWorldPoint(mousePos);
        mousePointerToWorld.y = _camera.transform.position.y;
        _mouseTarget.transform.position = mousePointerToWorld;
        Debug.DrawLine(mousePointerToWorld, mousePointerToWorld + (Vector3.down * 20), Color.yellow);

        HexTile tile = null;
        if (Physics.Raycast(mousePointerToWorld, Vector3.down, out RaycastHit ray))
        {
            tile = ray.collider.GetComponentInParent<HexTile>();            
        }
        OnMouseHoverHexTile?.Invoke(tile);
        if(_mouseBtn0Down)
        {
            OnMouseClickHexTile?.Invoke(tile);
        }
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
        Debug.Log("Mouse Scroll value: " + value);
        _zoomValue += (value *  _preferences.ZoomInterval) * -1.0f;
        _zoomValue = Mathf.Clamp(_zoomValue, _preferences.ZoomClamp.x, _preferences.ZoomClamp.y);
        _camera.orthographicSize = _zoomValue;
    }
}
