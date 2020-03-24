using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using HexWorld;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Editor window to create a hex board
/// </summary>
public class HexBoardEditor : EditorWindow
{
    //---- Enum
    //---------
    private enum State
    {
        Loading,
        MainMenu,
        New,
        Edit
    }

    //---- Variables
    //--------------
    private static string SCENE_PATH = "Assets/Scenes/BoardEditor/BoardEditor.unity";
    private static string DEFAULT_MATERIAL = "Solid";
    private static string DEFAULT_TEXTURE = "Water";
    private static Vector2 MINSIZE = new Vector2(1920/2, 1080/2);
    private static HexBoardEditor _window;
    private static BoardEditorTool _boardTool;
    private static HexTilePreferences _hexTilePreferences;

    private string _jsonPath;
    private string _fileName;
    private State _state;
    private int _fileToLoadIndex;

    private List<GUIContent> _hexBoardFiles;
    private HexBoardModel _model;
    private bool _initBoard;    

    //---- Unity
    //----------
    [MenuItem("IronGears/Design/Board Editor")]
    public static void Init()
    {
        LoadSceneAndPlay();
        LoadContent();
        _window = (HexBoardEditor)EditorWindow.GetWindow(typeof(HexBoardEditor));
        _window.minSize = MINSIZE;
        _window.Show();
    }

    private void OnEnable()
    {        
        LoadHexBoardFiles();        
        _state = State.MainMenu;
    }

    private void OnDisable()
    {        
        if(_boardTool)
        {
            _boardTool.Clean();
        }
        // Stop the editor
        if (EditorApplication.isPlaying)
        {
            EditorApplication.ExitPlaymode();
        }
    }

    private static void LoadSceneAndPlay()
    {
        // Open scene
        Scene scene = EditorSceneManager.OpenScene(SCENE_PATH);

        // Play the editor
        if (!EditorApplication.isPlaying)
        {
            EditorApplication.EnterPlaymode();
        }
    }

    //---- GUI
    //--------
    private void OnGUI()
    {   
        switch(_state)
        {
            case State.MainMenu:
                MainMenu();
                break;
            case State.New:
                NewBoard();
                break;
            case State.Edit:
                EditBoard();
                break;
        }
    }

    private void MainMenu()
    {
        EditorGUILayout.BeginVertical();
        {
            GUILayout.Space(20);
            // New board
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("New Board", EditorStyles.boldLabel);
                if(GUILayout.Button("Start"))
                {
                    _state = State.New;
                    _model = new HexBoardModel();
                    return;
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(20);
            // Load board
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Load Board", EditorStyles.boldLabel);
                int selected = EditorGUILayout.Popup(_fileToLoadIndex, _hexBoardFiles.ToArray());
                if(selected != _fileToLoadIndex)
                {
                    _fileToLoadIndex = selected;
                    if (_fileToLoadIndex != 0)
                    {
                        string fileName = _hexBoardFiles[_fileToLoadIndex].text;
                        _model = LoadHexBoard(fileName);
                    }
                }

                if(_fileToLoadIndex != 0)
                {
                    if (GUILayout.Button("Load"))
                    {
                        _state = State.Edit;
                        return;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }

    private void NewBoard()
    {
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.LabelField("Create Hex Board", EditorStyles.boldLabel);
            GUILayout.Space(20);

            // Name
            EditorGUILayout.LabelField("Name", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            {                
                string name = EditorGUILayout.DelayedTextField(_model.ID);
                if(name != _model.ID)
                {
                    _model.ID = name;
                }
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(20);

            // Size
            EditorGUILayout.LabelField("Board Size", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            {                
                EditorGUILayout.LabelField("Width:");
                float w = EditorGUILayout.DelayedFloatField(_model.Size.X);
                if(w != _model.Size.X)
                {
                    _model.Size.X = w;
                }
                EditorGUILayout.LabelField("Height:");
                float h = EditorGUILayout.DelayedFloatField(_model.Size.Y);
                if (h != _model.Size.Y)
                {
                    _model.Size.Y = h;
                }
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(20);

            // Scale
            EditorGUILayout.LabelField("Hex Scale", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            {                
                EditorGUILayout.LabelField("X:");
                float x = EditorGUILayout.DelayedFloatField(_model.Scale.X);
                if (x != _model.Scale.X)
                {
                    _model.Scale.X = x;
                }
                EditorGUILayout.LabelField("Y:");
                float y = EditorGUILayout.DelayedFloatField(_model.Scale.Y);
                if (y != _model.Scale.Y)
                {
                    _model.Scale.Y = y;
                }
                EditorGUILayout.LabelField("Z:");
                float z = EditorGUILayout.DelayedFloatField(_model.Scale.Z);
                if (z != _model.Scale.Z)
                {
                    _model.Scale.Z = z;
                }
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(20);

            // Layout
            EditorGUILayout.LabelField("Hex Layout", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            {                
                System.Enum selected = EditorGUILayout.EnumPopup(_model.Layout);
                if ((HexLayout)selected != _model.Layout)
                {
                    _model.Layout = (HexLayout)selected;
                }                
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(20);

            // Edit Button
            if (ValidateModel())
            {
                if(GUILayout.Button("Edit Board"))
                {
                    _state = State.Edit;
                }

                if (GUILayout.Button("Return"))
                {
                    _model = null;
                    _state = State.MainMenu;
                }
            }
            else
            {
                if (GUILayout.Button("Return"))
                {
                    _model = null;                    
                    _state = State.MainMenu;                    
                }
            }
        }
        EditorGUILayout.EndVertical();
    }

    private void EditBoard()
    {
        // Build scene
        if(!_initBoard)
        {
            EnterEdit();
            return;
        }

        GUILayout.BeginVertical();
        {
            GUILayout.Space(10);

            // Board data
            GUILayout.Label("---- Data ----", EditorStyles.boldLabel);
            GUILayout.Label("Name: " + _model.ID);
            GUILayout.Label("Size: " + _model.Size);
            GUILayout.Label("Scale: " + _model.Scale);
            GUILayout.Label("Layout: " + _model.Layout);

            GUILayout.Space(10);

            // Edit
            GUILayout.Label("---- Edit ----", EditorStyles.boldLabel);

            GUILayout.Space(10);

            // Save
            if (ValidateModel())
            {
                if (GUILayout.Button("Save"))
                {
                    string file = _model.ID + ".json";
                    string json = JsonUtility.ToJson(_model);

                    File.WriteAllText(_jsonPath + file, json);
                    AssetDatabase.Refresh();
                    Debug.LogWarning("Json saved: " + _jsonPath + file);
                    LoadHexBoardFiles();
                }

                if (GUILayout.Button("Return"))
                {
                    ExitEdit();
                    _state = State.MainMenu;
                }
            }
            else
            {
                if (GUILayout.Button("Return"))
                {
                    ExitEdit();
                    _state = State.MainMenu;
                }
            }
        }
        GUILayout.EndVertical();
    }

    //---- Edit Functions
    //-------------------
    private void EnterEdit()
    {
        if(_boardTool == null)
        {
            // Get tool object from scene
            Scene scene = SceneManager.GetActiveScene();
            GameObject[] roots = scene.GetRootGameObjects();
            for (int i = 0; i < roots.Length; i++)
            {
                BoardEditorTool tool = roots[i].GetComponent<BoardEditorTool>();
                if (tool != null)
                {
                    _boardTool = tool;
                    break;
                }
            }

            if (_boardTool == null)
            {
                Debug.LogError("Unable to load scene with board tool as root object");
                return;
            }
        }

        // New board - build out default
        if(_model.HexTileModels == null || _model.HexTileModels.Count == 0)
        {
            int total = Mathf.CeilToInt(_model.Size.X * _model.Size.Y);
            _model.HexTileModels = new List<HexTileModel>(total);

            // Build default hex tile
            for (int i = 0; i < total; i++)
            {
                HexTileModel tile = new HexTileModel();
                tile.Scale = _model.Scale;
                tile.MaterialName = DEFAULT_MATERIAL;
                tile.TextureName = DEFAULT_TEXTURE;
                tile.MovementCost = 1;
                tile.DefenseRating = 0;
                tile.TerrainType = TerrainType.Water;

                _model.AddHexTile(tile);
            }

            // Build out the model data
            BuildHexBoard(_model);
        }

        // Send model data to scene to build board
        _boardTool.BuildHexBoardFromModel(_model);

        _initBoard = true;
    }

    private void BuildHexBoard(HexBoardModel model)
    {
        if(_hexTilePreferences == null)
        {
            LoadPreference();
        }
        int size = (int)(model.Size.X * model.Size.Y);
        float w, h;
        if (model.Layout == HexLayout.Flat)
        {
            w = _hexTilePreferences.SizeX * model.Scale.X;
            h = _hexTilePreferences.SizeY * model.Scale.Z;
        }
        else
        {
            w = _hexTilePreferences.SizeY * model.Scale.X;
            h = _hexTilePreferences.SizeX * model.Scale.Z;
        }
        int col = 0;
        int row = 0;
        float offset = 0;        
        for(int i = 0; i < size; i++)
        {
            HexTileModel tile = model.HexTileModels[i];       
            if( model.Layout == HexLayout.Flat)
            {
                BuildFlat(model, tile, w, h, i, ref col, ref row, ref offset);
            }
            else
            {
                BuildPointy(model, tile, w, h, i, ref col, ref row, ref offset);
            }
        }
    }

    private void BuildFlat(HexBoardModel model, HexTileModel tile, float width, float height, int index, ref int col, ref int row, ref float offset)
    {
        if (col >= model.Size.X)
        {
            offset += (height * 2);
            col = 0;
            ++row;
        }

        tile = model.HexTileModels[index];
        tile.Position = (col & 1) == 1 ? new Point3(col * width, 0, offset) :
            new Point3(col * width, 0, offset + height);
        Point3 rotation = new Point3(0, 0, 0);        
        tile.Rotation = rotation;
        ++col;
    }

    private void BuildPointy(HexBoardModel model, HexTileModel tile, float width, float height, int index, ref int col, ref int row, ref float offset)
    {
        if (col >= model.Size.X)
        {
            offset += height;
            col = 0;
            ++row;
        }

        tile = model.HexTileModels[index];
        tile.Position = (row & 1) == 1 ? new Point3((col * width * 2) + width, 0, offset) :
            new Point3(col * width * 2, 0, offset);        
        Point3 rotation = new Point3(0, 30, 0);
        tile.Rotation = rotation;
        ++col;
    }
    private void ExitEdit()
    {
        _boardTool.Clean();
        _initBoard = false;
        _model = null;
    }

    //---- Validate
    //-------------
    private bool ValidateModel()
    {
        if(string.IsNullOrEmpty(_model.ID))
        {            
            return false;
        }

        if(_model.Size.X == 0 || _model.Size.Y == 0)
        {            
            return false;
        }

        if(_model.Scale.X == 0 || _model.Scale.Y == 0 || _model.Scale.Z == 0)
        {            
            return false;
        }
        return true;
    }

    //---- Load Data
    //--------------
    private static void LoadContent()
    {
        LoadPreference();        
    }

    private static void LoadPreference()
    {
        string path = "Assets/Preferences/HexTilePreferences.asset";
        _hexTilePreferences = Instantiate(AssetDatabase.LoadAssetAtPath<HexTilePreferences>(path));
        if (_hexTilePreferences == null)
            Debug.LogError("Unable to load Hex Tile Preferences @" + path);
    }

    private void LoadHexBoardFiles()
    {
        _jsonPath = Application.dataPath + "/Json/HexBoards/";
        _hexBoardFiles = new List<GUIContent>();
        _hexBoardFiles.Add(new GUIContent("None"));

        string[] files = Directory.GetFiles(_jsonPath);
        for(int i = 0; i < files.Length; i++)
        {
            string shortFilename = files[i].Replace(_jsonPath, "");
            if(shortFilename.Contains(".meta"))
            {
                continue;
            }
            shortFilename = shortFilename.Replace(".json", "");
            _hexBoardFiles.Add(new GUIContent(shortFilename));
        }
    }

    private HexBoardModel LoadHexBoard(string jsonFileName)
    {
        if (!File.Exists(_jsonPath + jsonFileName + ".json"))
        {
            Debug.LogError("Unable to find json file: " + _jsonPath + jsonFileName + ".json");
            return null;
        }

        string json = File.ReadAllText(_jsonPath + jsonFileName + ".json");
        return JsonUtility.FromJson<HexBoardModel>(json);
    }
}
