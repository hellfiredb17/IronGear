using UnityEngine;
using HexWorld;

/// <summary>
/// On Start loads Hexboard
/// </summary>
[RequireComponent(typeof(HexBoard))]
public class HexBoardLoader : MonoBehaviour
{
    //---- Variables
    //--------------    
    public string FileToLoad;
    private HexBoard _hexBoard;
    private HexTilePreferences _preferences;

    //---- Unity
    //----------
    private void Awake()
    {
        _hexBoard = GetComponent<HexBoard>();
    }

    private void Start()
    {
        _preferences = _hexBoard.Preferences;
        HexBoardModel hexBoardModel = JsonLoader.Parse<HexBoardModel>(Application.dataPath + _preferences.HexBoardPath + FileToLoad);
        _hexBoard.Model = hexBoardModel;
        _hexBoard.Controller.CreateBoard();
    }    
}