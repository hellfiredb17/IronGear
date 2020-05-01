using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexWorld;

public class BoardEditorInputController : MonoBehaviour
{
    //---- Variables
    //--------------
    [SerializeField] private BoardEditorPreferences _preferences;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameObject _mouse;
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private BoardEditorTool _tool;

    private float _zoomValue;
    private Vector3 _lastPosition;

    private HexTileModel _currentModel;
    private HexTile _currentHover;
    private HexTile _lastConvert;    
    private bool _leftBtnDown;
    private bool _rightBtnDown;

    //---- Unity
    //----------
    private void Awake()
    {
        _zoomValue = _mainCamera.orthographicSize;
        _cameraTarget.position = _mainCamera.transform.position;
    }

    private void FixedUpdate()
    {
        HexTile current = Raycast();

        // Hight light current under mouse
        if(current != _currentHover)
        {
            if(_currentHover)
            {
                _currentHover.View.OnPointerExit();
            }
            _currentHover = current;
            if(_currentHover)
            {
                _currentHover.View.OnPointerEnter();
            }
        }

        // Left button down
        if(!_leftBtnDown && Input.GetMouseButtonDown(0))
        {
            _leftBtnDown = true;
        }
        else if (_leftBtnDown && Input.GetMouseButtonUp(0))
        {
            _leftBtnDown = false;
            _lastConvert = null;
        }

        if(_leftBtnDown && current != _lastConvert)
        {
            _lastConvert = current;
            _tool.UpdateTile(_lastConvert, _currentModel);
        }

        HandleKeyboardInput();

        // Right button down
        if (!_rightBtnDown && Input.GetMouseButtonDown(1))
        {
            _rightBtnDown = true;
            _lastPosition = Input.mousePosition;
            _lastPosition.z = _mainCamera.transform.position.y;
        }
        else if (_rightBtnDown && Input.GetMouseButtonUp(1))
        {
            _rightBtnDown = false;
        }

        if(_rightBtnDown)
        {
            PanCamera();
        }
        
        LerpToTarget();
    }

    private void HandleKeyboardInput()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            ZoomIn();
        }

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            ZoomOut();
        }
    }

    //---- Public
    //-----------
    public void UpdateModelData(HexTileModel hexTileModel)
    {
        _currentModel = hexTileModel;
        _lastConvert = null;
    }

    //---- Private
    //------------
    private void LerpToTarget()
    {
        _mainCamera.transform.position = Vector3.Lerp(_mainCamera.transform.position, _cameraTarget.position, 
            Time.fixedDeltaTime * _preferences.CameraSpeed);
    }

    private void PanCamera()
    {
        Vector3 lastPos = _mainCamera.ScreenToWorldPoint(_lastPosition);
        lastPos.y = _mainCamera.transform.position.y;

        Vector3 currentPos = Input.mousePosition;
        currentPos.z = _mainCamera.transform.position.y;
        currentPos = _mainCamera.ScreenToWorldPoint(currentPos);
        currentPos.y = _mainCamera.transform.position.y;

        Vector3 delta = currentPos - lastPos;
        _cameraTarget.position += delta;

        _lastPosition = Input.mousePosition;
        _lastPosition.z = _mainCamera.transform.position.y;
    }

    private void ZoomOut()
    {
        _zoomValue += _preferences.ZoomInterval;
        _zoomValue = Mathf.Clamp(_zoomValue, _preferences.ZoomClamp.x, _preferences.ZoomClamp.y);
        _mainCamera.orthographicSize = _zoomValue;
    }

    private void ZoomIn()
    {
        _zoomValue -= _preferences.ZoomInterval;
        _zoomValue = Mathf.Clamp(_zoomValue, _preferences.ZoomClamp.x, _preferences.ZoomClamp.y);
        _mainCamera.orthographicSize = _zoomValue;
    }

    private HexTile Raycast()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = _mainCamera.transform.position.y;
        Vector3 mousePointerToWorld = _mainCamera.ScreenToWorldPoint(mousePos);
        mousePointerToWorld.y = _mainCamera.transform.position.y;
        _mouse.transform.position = mousePointerToWorld;
        Debug.DrawLine(mousePointerToWorld, mousePointerToWorld + (Vector3.down * 20), Color.yellow);
        if(Physics.Raycast(mousePointerToWorld, Vector3.down, out RaycastHit ray))
        {
            HexTile tile = ray.collider.GetComponentInParent<HexTile>();
            if(tile != null)
            {
                return tile;
            }
        }
        return null;
    }
}
