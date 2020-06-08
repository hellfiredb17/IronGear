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
        [Header("Main")]
        public MechModel Model;
        public MechView View;
        public MechController Controller;

        [Header("Sub")]
        public MechBase Base;
        public MechTorso Torso;
        public MechArm ArmLeft;
        public MechArm ArmRight;

        //---- Unity
        //----------
        private void Awake()
        {
            View.Root = this;
            Controller.Root = this;
        }
    } // end class
} // end namespace
