using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexWorld;

public class BoardEditorTool : MonoBehaviour
{
    //---- Variables
    //--------------
    public Camera MainCamera;
    public Light DirectionalLight;
    public GameObject Board;
    public HexTilePreferences Preferences;

    private HexBoardModel _model;
    private List<HexTile> _tiles;

    //---- Unity
    //----------
    private void Start()
    {
        Debug.Log("Tool started");
    }

    //---- Public
    //-----------
    public void BuildHexBoardFromModel(HexBoardModel model)
    {   
        if(_tiles != null && _tiles.Count > 0)
        {
            Clean();
        }
        _tiles = new List<HexTile>((int)(model.Size.X * model.Size.Y));
        _model = model;
        for(int i = 0; i < _model.HexTileModels.Count; i++)
        {
            HexTileModel tile = _model.HexTileModels[i];
            Texture texture = Preferences.Textures[Preferences.GetTypeEnum(tile.TextureName)];
            Material material = Preferences.Materials[HexMaterial.Solid];
            HexTile hex = Instantiate<HexTile>(Preferences.Prefab, Board.transform);                      
            hex.Model = tile;
            hex.View.SetSharedTexture(texture);
            hex.View.SetSharedMaterial(material);
            hex.transform.localPosition = tile.Position.Convert();
            hex.View.transform.localRotation = Quaternion.Euler(tile.Rotation.Convert());
            hex.transform.localScale = tile.Scale.Convert();
            _tiles.Add(hex);
        }       
    }

    public void Clean()
    {
        for(int i = 0; i < _tiles.Count; i++)
        {
            Destroy(_tiles[i].gameObject);
        }
        _tiles.Clear();
    }

    //---- Private
    //------------
  
}
