using HexWorld;

namespace Rigs
{
    [System.Serializable]
    public class MechArmModel : MechComponentModel
    {
        public enum ArmSlot
        {
            Left,
            Right
        }

        //---- Variables
        //--------------
        public string ModelName;
        public int Damage;
        public int Range;
        public ArmSlot Slot;
        public Point3 Offset;
        public Point3 Rotation;
        public Point3 Scale;
    }
}
