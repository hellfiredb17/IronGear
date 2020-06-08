using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

namespace HexWorld
{
    [RequireComponent(typeof(Camera), typeof(InputController))]
    public class InputCamera : MonoBehaviour
    {
        //---- Delegate
        //-------------
        public delegate void MouseRayCast(HexTile tile);

        //---- Variables
        //--------------
        protected Camera _camera;
        protected InputController _inputController;

        //---- Properties
        //---------------
        public Camera GetCamera => _camera;

        //---- Unity
        //----------
        protected virtual void Awake()
        {
            _camera = GetComponent<Camera>();
            _inputController = GetComponent<InputController>();
        }

        protected virtual void OnEnable()
        {            
            _inputController.Set(); // sets init values
        }

        //---- Protected
        //--------------
        protected bool RaycastToTile(out HexTile tileHit)
        {
            tileHit = null;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = _camera.transform.position.y;
            Ray rayToWorld = _camera.ScreenPointToRay(mousePos);
            
            RaycastHit[] hits = Physics.RaycastAll(rayToWorld);
            if(hits == null || hits.Length == 0)
            {
                return false;
            }

            // Get closest tile hit
            float shortDist = float.MaxValue;
            for(int i = 0; i < hits.Length; i++)
            {
                HexTile tile = hits[i].collider.GetComponentInParent<HexTile>();
                if(tile == null)
                {
                    continue;
                }
                if(hits[i].distance < shortDist)
                {
                    shortDist = hits[i].distance;
                    tileHit = tile;
                }
            }            
            return tileHit != null;
        }

    } // end class
} // end namespace
