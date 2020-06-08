using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HexWorld
{    
    public class GameCamera : InputCamera
    {
        //---- Variables
        //--------------
        private static float EPSILON_SQRT = Mathf.Epsilon * Mathf.Epsilon;

        public Action<HexTile> OnTileClick;

        [SerializeField] private float _radius;
        [Range(0, Mathf.PI)]
        [SerializeField] private float _angleOne;
        [Range(0, 2*Mathf.PI)]
        [SerializeField] private float _angleTwo;
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _moveTarget;
        [SerializeField] private float _speed;

        private HexTile _currentTile;

        private bool _active;
        private bool _btn0Down;

        //---- Unity
        //----------
        private void Start()
        {
            _inputController.Enable(true);
        }

        private void FixedUpdate()
        {      
            if(!_active)
            {
                return;
            }

            if(!AtLocation())
            {
                transform.position = Vector3.Lerp(transform.position, _moveTarget.position, Time.fixedDeltaTime * _speed);                
            }
        }

        private void LateUpdate()
        {
            if(!_active)
            {
                return;
            }
            _moveTarget.position = PointOnSphere();
        }

        protected override void OnEnable()
        {
            _inputController.OnMouseLeftBtnDown += OnBtn0;
            base.OnEnable();
        }

        private void OnDisable()
        {
            _inputController.OnMouseLeftBtnDown -= OnBtn0;
        }

        //---- Public
        //-----------
        public void SetTarget(Transform target)
        {
            _target = target;

            // Move camera to point on sphere, look at target
            transform.position = PointOnSphere();
            transform.LookAt(_target, Vector3.up);
            _moveTarget.position = transform.position;

            _active = true;
        }

        //---- Input Events
        //-----------------
        private void OnBtn0(bool isDown)
        {
            if (isDown)
            {
                RaycastToTile(out _currentTile);                
            }            
            else if (!isDown && _btn0Down && _currentTile != null)
            {
                // Button released
                if (RaycastToTile(out HexTile tile))
                {
                    if(tile == _currentTile)
                    {
                        // TODO - move to target
                        Debug.Log(_currentTile.name + " clicked");
                        OnTileClick?.Invoke(_currentTile);
                    }
                }
            }           

            _btn0Down = isDown;
        }

        //---- Private
        //------------
        /// Point on sphere equations
        /// x = x0 + rSinAngleOne * cosAngleTwo
        /// y = y0 + rSinAngleOne * sinAngleTwo
        /// z = z0 + rCostAngleOne
        private Vector3 PointOnSphere()
        {
            float x = _target.position.x + (_radius * Mathf.Sin(_angleOne) * Mathf.Cos(_angleTwo));
            float y = _target.position.y + (_radius * Mathf.Sin(_angleOne) * Mathf.Sin(_angleTwo));
            float z = _target.position.z + (_radius * Mathf.Cos(_angleOne));
            return new Vector3(x, y, z);
        }

        private bool AtLocation()
        {
            float sqrtMag = Vector3.SqrMagnitude(_moveTarget.position - transform.position);
            return sqrtMag < EPSILON_SQRT;
        }

    } // end class
} // end namespace
