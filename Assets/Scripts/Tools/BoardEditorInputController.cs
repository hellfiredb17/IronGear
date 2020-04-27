using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexWorld;

public class BoardEditorInputController : MonoBehaviour
{
    //---- Variables
    //--------------
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameObject _mouse;
    [SerializeField] private BoardEditorTool _tool;

    private HexTileModel _currentModel;
    private HexTile _currentHover;
    private HexTile _lastConvert;    
    private bool _leftBtnDown;
    private bool _rightBtnDown;

    //---- Unity
    //----------
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

        // Right button down
        if(!_rightBtnDown && Input.GetMouseButtonDown(1))
        {
            _rightBtnDown = true;
        }
        else if (_rightBtnDown && Input.GetMouseButtonUp(1))
        {
            _rightBtnDown = false;
        }

        if(_rightBtnDown)
        {
            // Handle pan camera
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
    private void HandlePanCamera()
    {
        // TODO - move camera base on mouse movement;
        // Maybe just lerp to mouse position?
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
