#if CLIENT
using UnityEngine;
#endif
namespace HexWorld
{
    //---- Vector 2
    //-------------
    [System.Serializable]
    public class Point2
    {
        public float X;
        public float Y;

        public Point2() { }

        public Point2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Point2(Point2 other)
        {
            X = other.X;
            Y = other.Y;
        }

        public override string ToString()
        {
            return "(" + X + "," + Y + ")";
        }
    }

    //---- Vector 3
    //-------------
    [System.Serializable]
    public class Point3 : Point2
    {        
        public float Z;

        public Point3(float x, float y, float z) : base(x, y)
        {
            Z = z;
        }

        public Point3(Point3 other) : base(other.X, other.Y)
        {
            Z = other.Z;
        }

        public override string ToString()
        {
            return "(" + X + "," + Y + "," + Z + ")";
        }
    }

#if CLIENT
    //---- Extensions
    //---------------
    public static class PointExtensions
    {
        public static Vector2 Convert(this Point2 pt)
        {
            return new Vector2(pt.X, pt.Y);
        }

        public static Vector3 Convert(this Point3 pt)
        {
            return new Vector3(pt.X, pt.Y, pt.Z);
        }

        public static Point2 Convert(this Vector2 pt)
        {
            return new Point2(pt.x, pt.y);
        }

        public static Point3 Convert(this Vector3 pt)
        {
            return new Point3(pt.x, pt.y, pt.z);
        }
    }
#endif
} // end namespace
