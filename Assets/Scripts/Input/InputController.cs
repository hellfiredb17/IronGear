using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexWorld;

public class InputController : MonoBehaviour
{
    //---- Delegates
    //--------------
    public delegate void InputBool(bool value);
    public delegate void Inputfloat(float value);
    public delegate void InputVector2(Vector2 value);
    public delegate void InputVector3(Vector3 value);

    //---- Events
    //-----------
    public InputBool OnMouseLeftBtnDown;
    public InputBool OnMouseRightBtnDown;
    public Inputfloat OnMouseScrollY;
    public InputVector3 OnMousePosition;

    //---- Variables
    //--------------
    private bool _enabled;

    // Mouse
    private Vector3 _mousePosition;
    private bool _leftMouseBtnDown;
    private bool _rightMouseBtnDown;

    // Keyboard

    //---- Unity
    //----------
    private void Awake()
    {
        // mouse
        _mousePosition = Input.mousePosition;
        _leftMouseBtnDown = false;
        _rightMouseBtnDown = false;
    }

    private void FixedUpdate()
    {
        if(!_enabled)
        {
            return;
        }

        HandleMouseInput();
        HandleKeyboardInput();
    }

    //---- Public
    //-----------
    public void Enable(bool value)
    {        
        if (!_enabled && value)
        {
            InputReset();
        }
        _enabled = value;        
    }

    public void Set()
    {
        // Mouse
        OnMousePosition?.Invoke(_mousePosition);
        OnMouseLeftBtnDown?.Invoke(_leftMouseBtnDown);
        OnMouseRightBtnDown?.Invoke(_rightMouseBtnDown);

        // Keyboard
        // TODO
    }

    //---- Private
    //------------
    private void InputReset()
    {
        // Mouse
        _mousePosition = Input.mousePosition;
        _leftMouseBtnDown = false;
        _rightMouseBtnDown = false;

        // Keyboard
        // TODO

        Set();
    }

    private void HandleKeyboardInput()
    {
        // TODO - add in keyboard functions here
    }

    private void HandleMouseInput()
    {
        // Left btn
        if (!_leftMouseBtnDown && Input.GetMouseButton(0))
        {
            _leftMouseBtnDown = true;
            OnMouseLeftBtnDown?.Invoke(true);
        }
        if (_leftMouseBtnDown && !Input.GetMouseButton(0))
        {
            _leftMouseBtnDown = false;
            OnMouseLeftBtnDown?.Invoke(false);
        }

        // Right btn
        if (!_rightMouseBtnDown && Input.GetMouseButton(1))
        {
            _rightMouseBtnDown = true;
            OnMouseRightBtnDown?.Invoke(true);
        }
        if (_rightMouseBtnDown && !Input.GetMouseButton(1))
        {
            _rightMouseBtnDown = false;
            OnMouseRightBtnDown?.Invoke(false);
        }

        // Scroll wheel
        Vector2 scroll = Input.mouseScrollDelta;
        if (scroll.y != 0)
        {
            OnMouseScrollY?.Invoke(scroll.y);
        }

        // Movement
        Vector3 pos = Input.mousePosition;
        if (!CompareVectors(_mousePosition, pos))
        {
            _mousePosition = pos;
            OnMousePosition?.Invoke(_mousePosition);
        }
    }

    private bool CompareVectors(Vector3 left, Vector3 right)
    {
        return Mathf.Approximately(left.x, right.x) &&
            Mathf.Approximately(left.y, right.y) &&
            Mathf.Approximately(left.z, right.z);
    }    
}
