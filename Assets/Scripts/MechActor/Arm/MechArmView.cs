using UnityEngine;

namespace Rigs
{
    public class MechArmView : MonoBehaviour
    {
        //---- Variables
        //--------------
        public MechArm Root;
        private ArmComponent _modelAsset;

        //---- Properties
        //---------------
        public Transform FireSlot => _modelAsset != null ? _modelAsset.FireTransform : null;

        //---- Public
        //-----------
        public void AttachArmAsset(ArmComponent arm)
        {
            RemoveArmAsset();
            arm.transform.SetParent(transform, false);
            _modelAsset = arm;
        }

        public void RemoveArmAsset()
        {
            if(_modelAsset == null)
            {
                return;
            }

            // Remove gameobject
            Destroy(_modelAsset.gameObject);
            _modelAsset = null;
        }
    } // end class
} // end namespace
