using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rigs
{
    /// <summary>
    /// Mech GameObject container/wrapper for MVC
    /// </summary>
    public class Mech : MonoBehaviour
    {
        //---- Variables
        //--------------
        public MechModel Model;
        public MechView View;
        public MechController Controller;

        //---- Unity
        //----------
        private void Awake()
        {
            View.Root = this;
            Controller.Root = this;
        }

    } // end class
} // end namespace
