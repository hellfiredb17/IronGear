using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HexBoardCreator))]
public class HexBoardCreatorEditor : Editor
{
    //---- Variables
    //--------------
    private HexBoardCreator _hexBoard;
    private int _cols;
    private int _rows;
    private Vector3 _scale = Vector3.zero;
    private string _boardName;

    //---- Functions
    //--------------
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (!_hexBoard)
        {
            _hexBoard = (HexBoardCreator)target;
        }

        GUILayout.BeginVertical();
        {
            GUILayout.Space(10);     
            GUIBoardCreation();
            GUILayout.Space(10);
            GUIBoardEditor();
            
        }
        GUILayout.EndVertical();
    }

    //---- Private
    //------------
    void GuiLine(int i_height = 1)
    {
        Rect rect = EditorGUILayout.GetControlRect(false, i_height);
        rect.height = i_height;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }   

    private void GUIBoardCreation()
    {
        GuiLine(1);
        GUILayout.Label("Board Layout", EditorStyles.boldLabel);

        // board name
        _boardName = EditorGUILayout.DelayedTextField(_boardName);

        // size
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Columns");
            _cols = EditorGUILayout.IntField(_cols);
            
            GUILayout.Label("Rows");
            _rows = EditorGUILayout.IntField(_rows);
        }
        GUILayout.EndHorizontal();

        // scale
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("X");
            _scale.x = EditorGUILayout.FloatField(_scale.x);

            GUILayout.Label("Y");
            _scale.y = EditorGUILayout.FloatField(_scale.y);

            GUILayout.Label("Z");
            _scale.z = EditorGUILayout.FloatField(_scale.z);
        }
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Create Board"))
        {
            _hexBoard.ClearBoard();
            _hexBoard.CreateBoard(_cols, _rows, _scale);
        }
    }

    private void GUIBoardEditor()
    {
        GuiLine(1);
        GUILayout.Label("Board Editor", EditorStyles.boldLabel);
        if (GUILayout.Button("Save Board"))
        {
            if(string.IsNullOrEmpty(_boardName))
            {
                Debug.LogError("Board name is null, need to have a name");
                return;
            }

            // 
            
        }
    }
}
