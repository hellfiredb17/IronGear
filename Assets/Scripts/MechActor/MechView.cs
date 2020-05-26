using UnityEngine;

namespace Rigs
{
    public class MechView : MonoBehaviour
    {
        //---- Variables
        //--------------
        public Mech Root;        

        //---- Public
        //-----------
        public void RelinkComponents()
        {
            // Mech will also have a base, torso - arms can differ
            // Base
            Root.Controller.Base.View.transform.position = Root.View.transform.position;
            // Torso
            Root.Controller.Torso.View.transform.position = Root.Controller.Base.View.TorsoSlot.position;
            // Arms
            if(Root.Model.ArmLeftModel != null)
            {
                Root.Controller.ArmLeft.View.transform.position = Root.Controller.Torso.View.ArmLeftSlot.position;
            }
            if (Root.Model.ArmRightModel != null)
            {
                Root.Controller.ArmRight.View.transform.position = Root.Controller.Torso.View.ArmRightSlot.position;
            }
        }        

    } // end class
} // end namespace
