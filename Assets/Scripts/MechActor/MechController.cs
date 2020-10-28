using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Rigs
{   
    public class MechController : MonoBehaviour
    {
        //---- Variables
        //--------------
        private static float EPSILON = 0.01f;
        public Mech Root;
        private Coroutine _movementCoroutine;
        private Quaternion _rotation;

        //---- Updates
        //------------
        private void FixedUpdate()
        {
            if(!Mathf.Approximately(Root.transform.rotation.y, _rotation.y))
            {
                RotateTo();
            }
        }

        //---- Public
        //-----------        
        public void MoveTo(Queue<HexTile> path, Action onComplete = null)
        {
            if(_movementCoroutine != null)
            {
                StopCoroutine(_movementCoroutine);
            }
            _movementCoroutine = StartCoroutine(InternalMoveTo(path, onComplete));
        }

        public void AimAtTarget(Vector3 target)
        {
            target.y = Root.transform.position.y;
            target = target - Root.transform.position;
            _rotation = Quaternion.LookRotation(target, Vector3.up);
            Root.transform.rotation = _rotation;
        }

        //---- Private
        //------------
        private void RotateTo()
        {
            float ty = _rotation.eulerAngles.y;
            float cy = Root.transform.rotation.eulerAngles.y;
            float delta = ty - cy;
        }

        private IEnumerator InternalMoveTo(Queue<HexTile> path, Action onComplete)
        {
            if(path.Count == 0)
            {
                Debug.LogError("Path count is 0, not moving");
                _movementCoroutine = null;
                yield break;
            }

            float movementSpeed = 1.0f; // pull from model
            float upperRotationSpeed = 1.0f; // pull from model
            float lowerRotationSpeed = 1.0f; // pull from model

            HexTile target = path.Dequeue();
            while(target != null)
            {
                // For now just do the movement, can do the rotations later
                if(AtLocation(Root.transform.position, target.View.Pos))
                {
                    Debug.Log("Reached location");
                    Root.transform.position = target.View.Pos;
                    Root.View.Index = target.View.Index;
                    target = path.Count > 0 ? path.Dequeue() : null;                    
                }
                else 
                {
                    Root.transform.position = Vector3.Lerp(Root.transform.position, target.View.Pos, Time.deltaTime * movementSpeed);
                    yield return null;
                }                
            }
            onComplete?.Invoke();
            _movementCoroutine = null;
        }

        private bool AtLocation(Vector3 atPos, Vector3 toPos)
        {
            float sqrtMag = Vector3.SqrMagnitude(toPos - atPos);
            return sqrtMag < EPSILON;
        }

    } // end class
} // end namespace
