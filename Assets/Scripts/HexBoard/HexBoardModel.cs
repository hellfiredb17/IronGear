using System.Collections;
using System.Collections.Generic;

namespace HexWorld
{
    /// <summary>
    /// Class to hold data for a Hex board
    /// </summary>
    [System.Serializable]
    public class HexBoardModel
    {
        //---- Variables
        //--------------
        public string ID;
        public List<HexTileModel> HexTileModels;
        public Point2 Size; // height x width for board size
        public Point3 Position;
        public Point3 Scale; // Hex scale
        public HexLayout Layout;

        //---- Ctor
        //---------
        public HexBoardModel()
        {
            ID = "";
            Size = new Point2(0, 0);
            Position = new Point3(0, 0, 0);
            Scale = new Point3(0, 0, 0);            
        }

        public HexBoardModel(int columns, int rows)
        {
            Size = new Point2(columns, rows);            
            HexTileModels = new List<HexTileModel>(columns * rows);
        }

        public HexBoardModel(HexBoardModel other)
        {
            ID = other.ID;
            HexTileModels = new List<HexTileModel>(other.HexTileModels);
            Size = new Point2(other.Size);
            Position = new Point3(other.Position);
            Scale = new Point3(other.Scale);
            Layout = other.Layout;
        }

        //---- Properties
        //---------------
        public int HexTileCount => HexTileModels == null ? 0 : HexTileModels.Count;

        //---- Public
        //-----------
        public void AddHexTile(HexTileModel hexData)
        {
            if(HexTileModels == null)
            {
                HexTileModels = new List<HexTileModel>();
            }
            HexTileModels.Add(hexData);
        }

        public void RemoveHexTile(int index)
        {
            if(HexTileModels == null || HexTileModels.Count == 0)
            {
                return;
            }
            HexTileModels.RemoveAt(index);
        }

        public Point3 HexPosition(int index)
        {
            return HexTileModels[index].Position;
        }

        public void Clear()
        {
            HexTileModels.Clear();
            Size = null;            
        }
    }
}
