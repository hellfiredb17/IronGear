using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using HexWorld;
using Rigs;

public class TestScene : MonoBehaviour
{
    //---- Variables
    //--------------
    public HexTilePreferences HexPreferences;
    public MechComponentPreferences MechPreferences;
    public GameCamera GameCamera;
    [SerializeField] private HexBoard _hexBoard;

    public string BoardToLoad;
    public string BaseToLoad;
    
    private Mech _mech;

    //---- Unity
    //----------
    private void Start()
    {
        LoadBoard();
        LoadMech();
        _mech.View.PossibleMovementList = CalulateMovementRadius(_mech.Base.Model.Energy, _mech.View.Index);
        GameCamera.SetTarget(_mech.transform);
    }

    private void OnEnable()
    {
        GameCamera.OnTileClick += MoveActorToTile;
    }

    private void OnDisable()
    {
        GameCamera.OnTileClick -= MoveActorToTile;
    }

    //---- Events
    //-----------
    private void MoveActorToTile(HexTile tile)
    {
        // TODO - A* to path for right now just simple move in line
        Queue<HexTile> q = new Queue<HexTile>();
        q.Enqueue(tile);        
        _mech.Controller.MoveTo(q, ()=>
        {
            _mech.View.NormalizeTiles();
            _mech.View.PossibleMovementList = CalulateMovementRadius(_mech.Base.Model.Energy, _mech.View.Index);
        });
    }

    //---- Calulate Movement
    //----------------------
    public List<HexTile> CalulateMovementRadius(int depth, int startIndex)
    {
        // Using 10 for now but that number is really col number
        // This is for flat boards
        //   x+10
        // x-1   x+1
        //     x
        // x-11  x-9
        //   x-10
        HashSet<int> processed = new HashSet<int>();
        HashSet<int> possible = new HashSet<int>() { startIndex };        

        // Get list of all possible movement indexes based on depth
        InternalCalulateMovement(processed, possible, startIndex, ref depth);

        // Filter out different heights
        FilterDifferentHeightTiles(processed, startIndex);

        // Show all processed tiles
        List<HexTile> tiles = new List<HexTile>(processed.Count);
        foreach(int index in processed)
        {
            HexTile tile = _hexBoard.View.HexTiles[index];
            tile.View.OnPointerEnter();
            tiles.Add(tile);
        }
        return tiles;
    }

    private bool EvenRow(int value)
    {   
        return value % 2 == 0;
    }

    private void InternalCalulateMovement(HashSet<int> processed, HashSet<int> possible, int startIndex, ref int depth)
    {
        float height = _hexBoard.View.HexTiles[startIndex].Model.Scale.Y;
        int count = _hexBoard.View.HexTiles.Count - 1;
        HashSet<int> nextPossible = new HashSet<int>();
        foreach(int index in possible)
        {
            if(processed.Contains(index))
            {
                continue;
            }

            bool even = EvenRow(index);

            int p1 = index + 10;
            if(ValidatePositionIndex(p1, processed, possible, count, height))
            {                
                nextPossible.Add(p1);
            }
            int p2 = even ? index + 11 : index + 1;
            if (ValidatePositionIndex(p2, processed, possible, count, height))
            {
                nextPossible.Add(p2);
            }
            int p3 = even ? index + 1 : index - 9;
            if (ValidatePositionIndex(p3, processed, possible, count, height))
            {
                nextPossible.Add(p3);
            }
            int p4 = index - 10;
            if (ValidatePositionIndex(p4, processed, possible, count, height))
            {
                nextPossible.Add(p4);
            }
            int p5 = even ? index - 1 : index - 11;
            if (ValidatePositionIndex(p5, processed, possible, count, height))
            {
                nextPossible.Add(p5);
            }
            int p6 = even ? index + 9 : index - 1;
            if (ValidatePositionIndex(p6, processed, possible, count, height))
            {
                nextPossible.Add(p6);
            }

            processed.Add(index);
        }

        --depth;
        if(depth >= 0)
        {
            // Continue processing
            InternalCalulateMovement(processed, nextPossible, startIndex, ref depth);
        }
    }

    private bool ValidatePositionIndex(int index, HashSet<int> processed, HashSet<int> possible, int count, float height)
    {
        return !processed.Contains(index) && 
            !possible.Contains(index) && 
            index > 0 && index < count && 
            _hexBoard.View.HexTiles[index].Model.Scale.Y == height;
    }

    private void FilterDifferentHeightTiles(HashSet<int> possibleTiles, int start)
    {
        float height = _hexBoard.View.HexTiles[start].Model.Scale.Y;
        List<int> toRemove = new List<int>();
        foreach(int i in possibleTiles)
        {
            HexTile tile = _hexBoard.View.HexTiles[i];
            if(tile.Model.Scale.Y != height)
            {
                toRemove.Add(i);
            }
        }

        for(int i = 0; i < toRemove.Count; i++)
        {
            possibleTiles.Remove(toRemove[i]);
        }
    }

    //---- Private
    //------------
    private void LoadBoard()
    {        
        HexBoardModel hexBoardModel = JsonLoader.Parse<HexBoardModel>(Application.dataPath + HexPreferences.HexBoardPath + BoardToLoad);
        if(hexBoardModel == null)
        {
            Debug.LogError("Unable to load HexBoard at " + Application.dataPath + HexPreferences.HexBoardPath + BoardToLoad);
        }
        _hexBoard.Model = hexBoardModel;
        _hexBoard.Controller.CreateBoard();
    }

    private void LoadMech()
    {
        // Build Mech
        _mech = Instantiate(MechPreferences.MechPrefab);
        MechBaseModel baseModel = JsonLoader.Parse<MechBaseModel>(Application.dataPath + MechPreferences.MechBaseJsonPath + BaseToLoad);
        _mech.Base.Model = baseModel;

        // Load model's asset
        BaseComponent asset = AssetDatabase.LoadAssetAtPath<BaseComponent>("Assets" + MechPreferences.MechBaseContentPath + baseModel.ModelAsset + ".prefab");
        if(asset == null)
        {
            Debug.LogError("Unable to load asset at " + "Assets" + MechPreferences.MechBaseContentPath + baseModel.ModelAsset);
            return;
        }
        asset = Instantiate(asset);
        _mech.Base.View.AttachBaseAsset(asset);

        // Center on board
        HexTile tile = _hexBoard.View.HexTiles[55];
        _mech.transform.position = tile.View.Pos;
        _mech.View.Index = 55;
    }
}
