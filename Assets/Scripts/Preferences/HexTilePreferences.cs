using UnityEngine;
using HexWorld;

[CreateAssetMenu(menuName = "IronGears/HexTilePreference")]
public class HexTilePreferences : ScriptableObject
{
    //---- Models
    //-----------
    [Header("Data Locations")]
    public string HexTilePath = "/Json/HexTiles/";
    public string HexBoardPath = "/Json/HexBoards/";
    public string HexWorldPath = "/Json/HexWorlds/";

    [Header("Models")]
    public HexTile Prefab;
    public float SizeX = 0.75f;
    public float SizeY = 0.433f;

    //---- Materials
    //--------------
    [HideInInspector]
    public StringMaterialDictionary Materials;

    //---- Textures
    //------------- 
    [HideInInspector]
    public StringTextureDictionary Textures;

    //---- Public Tile Functions
    //--------------------------    
    public HexTile CreateDefaultHexTile()
    {
        return GetHexTile("water");
    }

    public HexTile CreateHexTile(string texture)
    {
        return GetHexTile(texture);
    }   

    //---- Private
    //------------
    private HexTile GetHexTile(string texture)
    {
        HexTile tile = Instantiate<HexTile>(Prefab);        
        
        tile.Model.MaterialName = "default";
        tile.Model.TextureName = texture;

        tile.View.SetMaterial(Materials["default"]);
        tile.View.SetTexture(Textures[texture]);
        return tile;
    }
}
