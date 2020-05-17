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
    public CameraEditorBehaviour CameraBehaviour;
    public InputController InputController;

    private HexBoardModel _boardModel;
    private List<HexTile> _tiles;

    private HexTileModel _selectedHexData;
    private HexTile _currentHoverHexTile;
    private HexTile _lastHexTileClick;

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
        CameraBehaviour.OnMouseHoverHexTile += OnHexTileHoverEnter;
        CameraBehaviour.OnMouseClickHexTile += OnHexTileClick;
    }

    private void OnDestroy()
    {        
        OnEditorStopPlaying?.Invoke();        
        CameraBehaviour.OnMouseHoverHexTile -= OnHexTileHoverEnter;
        CameraBehaviour.OnMouseClickHexTile -= OnHexTileClick;
    }

    //---- Public
    //-----------
    public void BuildHexBoardFromModel(HexBoardModel model)
    {   
        if(_tiles != null && _tiles.Count > 0)
        {
            Clean();
        }

        _boardModel = model;
        _tiles = new List<HexTile>(model.HexTileModels.Count);        
        for(int i = 0; i < _boardModel.HexTileModels.Count; i++)
        {
            HexTileModel modelData = _boardModel.HexTileModels[i];
            Texture texture = Preferences.Textures[modelData.TextureName];
            Material material = Preferences.Materials[modelData.MaterialName];
            HexTile hex = Instantiate<HexTile>(Preferences.Prefab, Board.transform);
            hex.name = "Tile_" + i;
            hex.Model = modelData;
            hex.View.SetMaterial(material);
            hex.View.SetTexture(texture);            
            hex.transform.localPosition = modelData.Position.Convert();
            hex.transform.localRotation = Quaternion.Euler(modelData.Rotation.Convert());
            hex.transform.localScale = modelData.Scale.Convert();
            _tiles.Add(hex);
        }

        CenterCamera();
    }    

    public void Clean()
    {
        for(int i = 0; i < _tiles.Count; i++)
        {
            DestroyImmediate(_tiles[i].gameObject);
        }
        _tiles.Clear();
    }

    public void OnModelSelectionChange(HexTileModel hexModel)
    {
        _selectedHexData = hexModel;
    }    

    //---- Private
    //------------
    private void CenterCamera()
    {
        // Find closest center        
        Vector3 pos = (_tiles[_tiles.Count - 1].transform.position - _tiles[0].transform.position) * 0.5f;
        pos.y = CameraBehaviour.transform.position.y;
        CameraBehaviour.JumpTo(pos);
    }

    private void OnHexTileHoverEnter(HexTile hexTile)
    {
        if(_currentHoverHexTile && hexTile != _currentHoverHexTile)
        {
            _currentHoverHexTile.View.OnPointerExit();
        }

        _currentHoverHexTile = hexTile;
        if (hexTile == null)
        {            
            return;
        }       
        
        _currentHoverHexTile.View.OnPointerEnter();
    }

    private void OnHexTileClick(HexTile hexTile)
    {
        if(hexTile == null || _selectedHexData == null || 
            _lastHexTileClick == hexTile)
        {
            _lastHexTileClick = null;
            return;
        }
        _lastHexTileClick = hexTile;
        UpdateHexTileData(hexTile, _selectedHexData);
    }

    private void UpdateHexTileData(HexTile tile, HexTileModel model)
    {
        // Find model index
        HexTileModel hexModel = null;
        for (int i = 0; i < _tiles.Count; i++)
        {
            if (tile == _tiles[i])
            {
                hexModel = _boardModel.HexTileModels[i];
                break;
            }
        }

        if(hexModel == null)
        {
            Debug.LogError("Unable to update hext tile data, could not be found: " + tile.name);
            return;
        }
        hexModel.MaterialName = model.MaterialName;
        hexModel.TextureName = model.TextureName;
        hexModel.MovementCost = model.MovementCost;
        hexModel.DefenseRating = model.DefenseRating;
        hexModel.Scale = model.Scale;

        Texture texture = Preferences.Textures[model.TextureName];

        //tile.Model = model;
        tile.View.SetTexture(texture);
        tile.transform.localScale = model.Scale.Convert();
    }
}
