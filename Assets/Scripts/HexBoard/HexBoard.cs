using UnityEngine;
using HexWorld;

/// <summary>
/// Unity object container for Hexboard model, view, controller
/// </summary>
public class HexBoard : MonoBehaviour
{
    //---- Variables
    //--------------
    public HexBoardModel Model;
    public HexBoardView View;
    public HexBoardController Controller;
    public HexTilePreferences Preferences;

    //---- Unity
    //----------
    private void Awake()
    {
        Controller.HexBoard = this;
    }
}
