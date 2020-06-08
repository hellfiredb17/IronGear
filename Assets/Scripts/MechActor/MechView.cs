using UnityEngine;
using System.Collections.Generic;

namespace Rigs
{
    public class MechView : MonoBehaviour
    {
        //---- Variables
        //--------------
        public Mech Root;
        public int Index;

        public List<HexTile> PossibleMovementList;

        //---- Public
        //-----------
        public void NormalizeTiles()
        {
            if(PossibleMovementList == null || PossibleMovementList.Count == 0)
            {
                return;
            }
            for(int i = 0; i < PossibleMovementList.Count; i++)
            {
                PossibleMovementList[i].View.OnPointerExit();
            }
        }

    } // end class
} // end namespace
