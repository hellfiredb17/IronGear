using UnityEngine;
using HexWorld;

public class HexTile : MonoBehaviour
{
    //---- Variables
    //--------------
    public HexTileView View;
    public HexTileModel Model;
    public HexTileController Controller;
    
    //---- Functions
    //--------------
    public void SetModelData()
    {
        Model.Position = transform.localPosition.Convert();
        Model.Scale = transform.localScale.Convert();
        Model.MaterialName = View.MaterialName;
        Model.TextureName = View.TextureName;        
    }
}
