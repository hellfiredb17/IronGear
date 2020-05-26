using UnityEngine;

namespace Rigs
{
    public class MechBaseView : MonoBehaviour
    {
        //---- Variables
        //--------------
        public MechBase Root;
        private BaseComponent _modelAsset;

        //---- Properties
        //---------------
        public Transform TorsoSlot => _modelAsset != null ? _modelAsset.TorsoSlot : null;

        //---- Public
        //-----------
        public void AttachBaseAsset(BaseComponent baseComponent)
        {            
            RemoveBaseAsset();
            _modelAsset = baseComponent;
            _modelAsset.transform.SetParent(transform, false);
        }

        public void RemoveBaseAsset()
        {            
            if(_modelAsset == null)
            {
                return;
            }
            if (Application.isPlaying)
            {
                Destroy(_modelAsset.gameObject);
            }
            else
            {
                DestroyImmediate(_modelAsset.gameObject);
            }
            _modelAsset = null;
        }

    } // end class
} // end namespace
