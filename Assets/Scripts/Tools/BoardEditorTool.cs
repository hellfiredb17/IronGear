using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexWorld;
using UnityEditor;
using System;

public class BoardEditorTool : MonoBehaviour
{
    //---- Variables
    //--------------
    public static bool EditorWindowOpen;
    public static Action OnEditorStopPlaying;
    
    public Light DirectionalLight;
    public GameObject Board;
    public HexTilePreferences Preferences;
    public CameraEditorBehaviour cameraBehaviour;
    public InputController InputController;

    private HexBoardModel _model;
    private List<HexTile> _tiles;
    private HexTileModel _selectedHexData;

    //---- Unity
    //----------
    private static void OpenEditorWindow()
    {
        EditorApplication.ExecuteMenuItem("IronGears/Design/Board Editor");
    }

    private void Awake()
    {
        if(!EditorWindowOpen)
        {
            OpenEditorWindow();
        }
    }

    private void Start()
    {
        Debug.Log("Tool started -- Window open: " + EditorWindowOpen);
    }

    private void OnDestroy()
    {        
        OnEditorStopPlaying?.Invoke();
        Debug.Log("Tool ended -- Window open: " + EditorWindowOpen);
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
            hex.name = "Tile_" + i;
            hex.Model = tile;
            hex.View.SetMaterial(material);
            hex.View.SetTexture(texture);            
            hex.transform.localPosition = tile.Position.Convert();
            hex.View.transform.localRotation = Quaternion.Euler(tile.Rotation.Convert());
            hex.transform.localScale = tile.Scale.Convert();
            _tiles.Add(hex);
        }       
    }

    public void SetHexTileModel(HexTileModel data)
    {
        _selectedHexData = data;
    }

    public void Clean()
    {
        for(int i = 0; i < _tiles.Count; i++)
        {
            DestroyImmediate(_tiles[i].gameObject);
        }
        _tiles.Clear();
    }

    public void UpdateTile(HexTile tile, HexTileModel model)
    {
        if (tile == null || model == null)
        {
            return;
        }

        // Find model index
        for(int i = 0; i < _tiles.Count; i++)
        {
            if(tile == _tiles[i])
            {                
                _model.HexTileModels[i].MaterialName = model.MaterialName;
                _model.HexTileModels[i].TextureName = model.TextureName;
                _model.HexTileModels[i].MovementCost = model.MovementCost;
                _model.HexTileModels[i].DefenseRating = model.DefenseRating;
            }
        }

        Texture texture = Preferences.Textures[Preferences.GetTypeEnum(model.TextureName)];
        Material material = Preferences.Materials[HexMaterial.Solid];        
        
        tile.Model = model;
        tile.View.SetTexture(texture);                
    }
}
