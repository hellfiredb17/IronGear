namespace HexWorld
{
    /// <summary>
    /// The Data of a Hex Tile
    /// </summary>
    [System.Serializable]
    public class HexTileModel
    {
        //---- Variables
        //--------------        
        public Point3 Position;
        public Point3 Scale;
        public Point3 Rotation;
        public string MaterialName;
        public string TextureName;
        public int MovementCost;
        public int DefenseRating;
        public TerrainType TerrainType;        

        //---- Ctor
        //---------
        public HexTileModel() { }

        public HexTileModel(HexTileModel other)
        {
            Position = new Point3(other.Position);
            Scale = new Point3(other.Scale);
            MaterialName = other.MaterialName;
            TextureName = other.TextureName;
            MovementCost = other.MovementCost;
            DefenseRating = other.DefenseRating;
            TerrainType = other.TerrainType;            
        }        
    }

    /// <summary>
    /// Extension methods for HexTileModel
    /// </summary>
    public static class HexTileModelExtensions
    {
        public static void Copy(this HexTileModel copy, HexTileModel origin)
        {
            copy.Position = new Point3(origin.Position);
            copy.Scale = new Point3(origin.Scale);
            copy.MaterialName = origin.MaterialName;
            copy.TextureName = origin.TextureName;
            copy.MovementCost = origin.MovementCost;
            copy.DefenseRating = origin.DefenseRating;
            copy.TerrainType = origin.TerrainType;            
        }
    }
} // end namespace
