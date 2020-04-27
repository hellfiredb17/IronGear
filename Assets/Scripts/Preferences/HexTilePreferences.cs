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
    public MaterialDictionary Materials;

    //---- Textures
    //------------- 
    [HideInInspector]
    public TextureDictionary Textures;

    //---- Public Tile Functions
    //--------------------------
    public HexTile GetHexTile(HexType type)
    {
        return GetHexTile(HexMaterial.Solid, type);
    }

    public HexTile GetHexTile(HexMaterial material, HexType type)
    {
        HexTile tile = Instantiate<HexTile>(Prefab);
        tile.Model.MaterialName = material.ToString();
        tile.Model.TextureName = type.ToString();
        tile.View.SetMaterial(Materials[material]);
        tile.View.SetTexture(Textures[type]);
        return tile;
    }

    public HexMaterial GetMaterialEnum(string materialName)
    {
        foreach(var mat in Materials)
        {
            if(mat.Key.ToString() == materialName)
            {
                return mat.Key;
            }
        }
        return 0;
    }

    public HexType GetTypeEnum(string textureName)
    {
        foreach (var text in Textures)
        {
            if (text.Key.ToString() == textureName)
            {
                return text.Key;
            }
        }
        return 0;
    }
}
