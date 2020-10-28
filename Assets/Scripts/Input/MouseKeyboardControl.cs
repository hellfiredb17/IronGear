using System;
using UnityEngine;

public class MouseKeyboardControl : MonoBehaviour
{
    private static MouseKeyboardControl _instance;

    //---- Events
    //-----------
    public InputController.InputVector3 MouseToWorld;
        
    //---- Variables
    //--------------
    [SerializeField] private InputController _inputController;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private RectTransform _mousePosition;

    //---- Unity
    //----------
    private void Awake()
    {        
        _instance = this;                
    }

    public void OnEnable()
    {
        _inputController.OnMousePosition += ConvertMouseToWorld;
        _inputController.Enable(true);
    }

    public void OnDisable()
    {
        _inputController.OnMousePosition -= ConvertMouseToWorld;
    }

    //---- Public
    //-----------
    public static void Subscribe<T>(string eventName, T func) where T : Delegate
    {
        if(_instance == null)
        {
            return;
        }
        _instance.MouseToWorld += (func as InputController.InputVector3);
    }

    public static void UnSubscribe<T>(string eventName, T func) where T : Delegate
    {
        if(_instance == null)
        {
            return;
        }
        _instance.MouseToWorld -= (func as InputController.InputVector3);
    }

    //---- Private
    //------------
    private void ConvertMouseToWorld(Vector3 pixelPosition)
    {
        Ray ray = _mainCamera.ScreenPointToRay(pixelPosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, _mainCamera.farClipPlane);
        if(hits.Length == 0)
        {            
            return;
        }

        Vector3 pos = Vector3.zero;
        for(int i = 0; i < hits.Length; i++)
        {
            pos = hits[i].point;
        }
        _targetPosition.position = pos;
        MouseToWorld?.Invoke(pos);
    }
}
