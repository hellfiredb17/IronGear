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

    private HexBoardModel _model;

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
        _model = model;

        // TODO - build the board for each hex tile
        Debug.Log("TODO - build out the hex board!");
    }

    public void Clean()
    {
        // TODO - remove all objects
        Debug.Log("TODO - clean the hex board!");
    }

    //---- Private
    //------------    
}
