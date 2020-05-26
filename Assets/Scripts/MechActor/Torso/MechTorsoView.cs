using UnityEditor;
using UnityEngine;

namespace Rigs
{
    public class MechTorsoView : MonoBehaviour
    {
        //---- Variables
        //--------------
        public MechTorso Root;
        private TorsoComponent _modelAsset;

        //---- Properties
        //---------------
        public Transform ArmLeftSlot => _modelAsset != null ? _modelAsset.ArmLeftSlot : null;
        public Transform ArmRightSlot => _modelAsset != null ? _modelAsset.ArmRightSlot : null;

        //---- Public
        //-----------
        public void AttachTorsoAsset(TorsoComponent torsoComponent)
        {
            RemoveTorsoAsset();
            _modelAsset = torsoComponent;
            _modelAsset.transform.SetParent(transform, false);
        }

        public void RemoveTorsoAsset()
        {
            if (_modelAsset == null)
            {
                return;                
            }
            Destroy(_modelAsset.gameObject);
            _modelAsset = null;
        }
    } // end class
} // end namespace
