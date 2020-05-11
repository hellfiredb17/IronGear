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
    private static string DEFAULT_MATERIAL = "default";
    private static string DEFAULT_TEXTURE = "temp";
    
    private static float LARGE_WIDTH = 100;
    private static float MEDIUM_WIDTH = 50;
    private static float SMALL_WIDTH = 20;
    private static float LINE = SMALL_WIDTH + 5.0f;

    private static Vector2 MINSIZE = new Vector2(200, 1080/2);
    private static HexBoardEditor _window;
    private static BoardEditorTool _boardTool;    
    private static HexTilePreferences _hexTilePreferences;

    private State _state;
    private HexBoardModel _boardModel;    
    private string _fileName;
    private string _orginalFileName;
    private int _fileToLoadIndex;

    private List<HexTileModel> _tileModels;
    private List<GUIContent> _hexBoardFiles;
    private List<GUIContent> _tileContent;

    private int _tileSelectedIndex;

    private bool _initBoard;
    private bool _isEdit;

    //---- Unity
    //----------
    public static bool IsOpen => _window == null;

    [MenuItem("IronGears/Design/Board Editor")]
    public static void Init()
    {
        OpenScene();
        PlayScene();
        _window = (HexBoardEditor)EditorWindow.GetWindow(typeof(HexBoardEditor));
        _window.minSize = MINSIZE;        
        _window.Show();        
    }

    private void OnEnable()
    {
        BoardEditorTool.EditorWindowOpen = true;
        BoardEditorTool.OnEditorStopPlaying = OnClose;
        LoadPreference();
        LoadHexBoardFiles();
        LoadHexTiles();
        _state = State.MainMenu;
    }

    private void OnDisable()
    {
        BoardEditorTool.EditorWindowOpen = false;
        if (_boardTool)
        {
            _boardTool.Clean();
        }
        // Stop the editor
        if (EditorApplication.isPlaying)
        {
            EditorApplication.ExitPlaymode();
        }
    }

    private void OnClose()
    {
        if (_window)
        {
            _window.Close();
        }
    }

    private static void OpenScene()
    {
        // Open scene
        if (!EditorApplication.isPlaying)
        {
            Scene scene = EditorSceneManager.OpenScene(SCENE_PATH);
        }
    }

    private static void PlayScene()
    {
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
        GUI.Label(new Rect(0, 0, 100, 20), "New Board");
        if(GUI.Button(new Rect(110, 0, 100, 20), "Build"))
        {
            _state = State.New;
            _boardModel = new HexBoardModel();
            return;
        }

        GUI.Label(new Rect(0, 25, 100, 20), "Load Board");
        int selected = EditorGUI.Popup(new Rect(110, 25, 100, 20), _fileToLoadIndex, _hexBoardFiles.ToArray());
        if (selected != _fileToLoadIndex)
        {
            _fileToLoadIndex = selected;
            if (_fileToLoadIndex != 0)
            {
                string fileName = _hexBoardFiles[_fileToLoadIndex].text;
                _boardModel = LoadHexBoard(fileName);
            }
        }

        if (_fileToLoadIndex != 0)
        {
            if (GUI.Button(new Rect(220, 25, 100, 20), "Load"))
            {
                _fileToLoadIndex = 0;
                _state = State.Edit;
                return;
            }
        }
    }

    private void NewBoard()
    {
        GUI.Label(new Rect(0, 0, 300, 20), "New Board", EditorStyles.boldLabel);

        // Name
        GUI.Label(new Rect(0, 25, 100, 20), "Board ID");
        string name = EditorGUI.DelayedTextField(new Rect(100, 25, 100, 20), _boardModel.ID);
        if (name != _boardModel.ID)
        {
            _boardModel.ID = name;
        }

        // Size
        GUI.Label(new Rect(0, 50, 100, 20), "Size");
        GUI.Label(new Rect(0, 75, 50, 20), "Width:");
        float w = EditorGUI.DelayedFloatField(new Rect(55, 75, 50, 20), _boardModel.Size.X);
        if(w != _boardModel.Size.X)
        {
            _boardModel.Size.X = w;
        }
        GUI.Label(new Rect(110, 75, 50, 20), "Height:");
        float h = EditorGUI.DelayedFloatField(new Rect(165, 75, 50, 20), _boardModel.Size.Y);
        if (h != _boardModel.Size.Y)
        {
            _boardModel.Size.Y = h;
        }        

        // Scale
        GUI.Label(new Rect(0, 100, 100, 20),"Hex Scale");            
        GUI.Label(new Rect(0, 125, 20, 20), "X:");
        float x = EditorGUI.DelayedFloatField(new Rect(21, 125, 50, 20),_boardModel.Scale.X);
        if (x != _boardModel.Scale.X)
        {
            _boardModel.Scale.X = x;
        }
        GUI.Label(new Rect(80, 125, 20, 20),"Y:");
        float y = EditorGUI.DelayedFloatField(new Rect(101, 125, 50, 20),_boardModel.Scale.Y);
        if (y != _boardModel.Scale.Y)
        {
            _boardModel.Scale.Y = y;
        }
        GUI.Label(new Rect(160, 125, 20, 20),"Z:");
        float z = EditorGUI.DelayedFloatField(new Rect(181, 125, 50, 20),_boardModel.Scale.Z);
        if (z != _boardModel.Scale.Z)
        {
            _boardModel.Scale.Z = z;
        }
        
        // Layout
        GUI.Label(new Rect(0, 150, 100, 20), "Hex Layout", EditorStyles.boldLabel);                       
        System.Enum selected = EditorGUI.EnumPopup(new Rect(105, 150, 100, 20),_boardModel.Layout);
        if ((HexLayout)selected != _boardModel.Layout)
        {
            _boardModel.Layout = (HexLayout)selected;
        }  

        // Edit Button
        if (ValidateModel())
        {
            if(GUI.Button(new Rect(0, 175, 100, 20),"Edit Board"))
            {
                _state = State.Edit;
            }

            if (GUI.Button(new Rect(105, 175, 100, 20),"Return"))
            {
                _boardModel = null;
                _state = State.MainMenu;
            }
        }
        else
        {
            if (GUI.Button(new Rect(0, 175, 100, 20), "Return"))
            {
                _boardModel = null;                    
                _state = State.MainMenu;                    
            }
        }
    }

    private void EditBoard()
    {
        // Build scene
        if(!_initBoard)
        {
            EnterEdit();
            return;
        }

        Rect rect = new Rect(0, 0, 100, 20);
        // Board data
        GUI.Label(rect, "Edit Board", EditorStyles.boldLabel);

        // ID
        rect.y += LINE;
        rect.width = SMALL_WIDTH;
        GUI.Label(rect, "ID");
        rect.x += SMALL_WIDTH;
        rect.width = LARGE_WIDTH;
        string strName = EditorGUI.DelayedTextField(rect, _boardModel.ID);
        if(strName != _boardModel.ID)
        {
            _boardModel.ID = strName;
            _isEdit = true;
        }

        // Size
        rect.y += LINE;
        rect.x = 0;
        GUI.Label(rect, "Size");
        rect.y += LINE;
        rect.width = MEDIUM_WIDTH;
        GUI.Label(rect, "Col");
        rect.x += MEDIUM_WIDTH;
        rect.width = MEDIUM_WIDTH;
        float x = EditorGUI.DelayedFloatField(rect, _boardModel.Size.X);
        if (x != _boardModel.Size.X)
        {
            _boardModel.Size.X = x;
            _isEdit = true;
        }
        rect.x += MEDIUM_WIDTH;
        rect.width = MEDIUM_WIDTH;
        GUI.Label(rect, "Row");
        rect.x += MEDIUM_WIDTH;
        rect.width = MEDIUM_WIDTH;
        float y = EditorGUI.DelayedFloatField(rect, _boardModel.Size.Y);
        if (y != _boardModel.Size.Y)
        {
            _boardModel.Size.Y = y;
            _isEdit = true;
        }

        // Scale
        rect.x = 0;
        rect.y += LINE;
        rect.width = LARGE_WIDTH;
        GUI.Label(rect, "Scale");
        rect.y += LINE;
        rect.width = SMALL_WIDTH;
        GUI.Label(rect, "X");
        rect.x += SMALL_WIDTH;
        rect.width = MEDIUM_WIDTH;
        x = EditorGUI.DelayedFloatField(rect, _boardModel.Scale.X);
        if(x != _boardModel.Scale.X)
        {
            _boardModel.Scale.X = x;
            _isEdit = true;
        }
        rect.x += MEDIUM_WIDTH;
        rect.width = SMALL_WIDTH;
        GUI.Label(rect, "Y");
        rect.x += SMALL_WIDTH;
        rect.width = MEDIUM_WIDTH;
        y = EditorGUI.DelayedFloatField(rect, _boardModel.Scale.Y);
        if (y != _boardModel.Scale.Y)
        {
            _boardModel.Scale.Y = y;
            _isEdit = true;
        }
        rect.x += MEDIUM_WIDTH;
        rect.width = SMALL_WIDTH;
        GUI.Label(rect, "Z");
        rect.x += SMALL_WIDTH;
        rect.width = MEDIUM_WIDTH;
        float z = EditorGUI.DelayedFloatField(rect, _boardModel.Scale.Z);
        if (z != _boardModel.Scale.Z)
        {
            _boardModel.Scale.Z = z;
            _isEdit = true;
        }

        // Layout        
        rect.x = 0;
        rect.y += LINE;
        rect.width = MEDIUM_WIDTH;
        GUI.Label(rect, "Layout");
        rect.x += MEDIUM_WIDTH;
        rect.width = LARGE_WIDTH;
        HexLayout value = (HexLayout) EditorGUI.EnumPopup(rect, _boardModel.Layout);
        if(value != _boardModel.Layout)
        {
            _boardModel.Layout = value;
            _isEdit = true;
        }

        // Rebuild with new data        
        if(_isEdit)
        {
            rect.y += LINE;
            rect.x = 0;
            if (GUI.Button(rect, "Rebuild"))
            {
                _boardTool.Clean();
                _boardModel.HexTileModels.Clear();
                _initBoard = false;
                _isEdit = false;
                return;
            }
        }

        rect.y += LINE + LINE;
        rect.x = 0;
        GUI.Label(rect, "Edit Tile", EditorStyles.boldLabel);
        rect.y += LINE;
        rect.width = MEDIUM_WIDTH;
        GUI.Label(rect, "Tile");
        rect.x += MEDIUM_WIDTH;
        rect.width = LARGE_WIDTH;
        int selection = EditorGUI.Popup(rect, _tileSelectedIndex, _tileContent.ToArray());
        if(selection != _tileSelectedIndex)
        {
            _tileSelectedIndex = selection;
            _boardTool.OnModelSelectionChange(_tileModels[_tileSelectedIndex]);            
        }      
        
        // Save
        if (ValidateModel())
        {
            rect.y += LINE;
            rect.x = 0;
            rect.width = LARGE_WIDTH;
            if (GUI.Button(rect,"Save"))
            {
                string jsonPath = Application.dataPath + "/Json/HexBoards/";
                // Remove old name first
                if (_boardModel.ID != _orginalFileName)
                {
                    string oldFile = jsonPath + _orginalFileName + ".json";
                    if (File.Exists(oldFile))
                    {
                        File.Delete(oldFile);
                        Debug.LogWarning("File " + oldFile + " removed");
                    }
                }

                // Save data
                string file = _boardModel.ID + ".json";
                string json = JsonUtility.ToJson(_boardModel);

                File.WriteAllText(jsonPath + file, json);
                AssetDatabase.Refresh();
                Debug.LogWarning("Json saved: " + jsonPath + file);
                LoadHexBoardFiles();
            }

            rect.x += LARGE_WIDTH;
            if (GUI.Button(rect,"Return"))
            {
                ExitEdit();
                _state = State.MainMenu;
            }
        }
        else
        {
            if (GUI.Button(rect,"Return"))
            {
                ExitEdit();
                _state = State.MainMenu;
            }
        }
    }

    //---- Edit Functions
    //-------------------
    private void EnterEdit()
    {
        if (_boardTool == null)
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
        if(_boardModel.HexTileModels == null || _boardModel.HexTileModels.Count == 0)
        {
            int total = Mathf.CeilToInt(_boardModel.Size.X * _boardModel.Size.Y);
            _boardModel.HexTileModels = new List<HexTileModel>(total);

            // Build default hex tile
            for (int i = 0; i < total; i++)
            {
                HexTileModel tile = new HexTileModel();
                tile.Scale = _boardModel.Scale;
                tile.MaterialName = DEFAULT_MATERIAL;
                tile.TextureName = DEFAULT_TEXTURE;
                tile.MovementCost = 0;
                tile.DefenseRating = 0;
                tile.TerrainType = TerrainType.Water;

                _boardModel.AddHexTile(tile);
            }

            // Build out the model positions data
            BuildHexBoard(_boardModel);
        }

        // Send model data to scene to build board
        _boardTool.BuildHexBoardFromModel(_boardModel);

        // set oringal file name
        _orginalFileName = _boardModel.ID;
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
        _isEdit = false;
        _boardModel = null;
        _orginalFileName = string.Empty;
    }

    //---- Validate
    //-------------
    private bool ValidateModel()
    {
        if(string.IsNullOrEmpty(_boardModel.ID))
        {            
            return false;
        }

        if(_boardModel.Size.X == 0 || _boardModel.Size.Y == 0)
        {            
            return false;
        }

        if(_boardModel.Scale.X == 0 || _boardModel.Scale.Y == 0 || _boardModel.Scale.Z == 0)
        {            
            return false;
        }
        return true;
    }

    //---- Load Data
    //--------------
    private static void LoadPreference()
    {
        string path = "Assets/Preferences/HexTilePreferences.asset";
        _hexTilePreferences = Instantiate(AssetDatabase.LoadAssetAtPath<HexTilePreferences>(path));
        if (_hexTilePreferences == null)
            Debug.LogError("Unable to load Hex Tile Preferences @" + path);
    }

    private void LoadHexBoardFiles()
    {
        string jsonPath = Application.dataPath + "/Json/HexBoards/";
        _hexBoardFiles = new List<GUIContent>();
        _hexBoardFiles.Add(new GUIContent("None"));

        string[] files = Directory.GetFiles(jsonPath);
        for(int i = 0; i < files.Length; i++)
        {
            string shortFilename = files[i].Replace(jsonPath, "");
            if(shortFilename.Contains(".meta"))
            {
                continue;
            }
            shortFilename = shortFilename.Replace(".json", "");
            _hexBoardFiles.Add(new GUIContent(shortFilename));
        }
    }

    private void LoadHexTiles()
    {
        // Load tiles
        _tileModels = new List<HexTileModel>();
        _tileContent = new List<GUIContent>();

        string jsonPath = Application.dataPath + "/Json/HexTiles/";
        string[] files = Directory.GetFiles(jsonPath);
        for (int i = 0; i < files.Length; i++)
        {            
            if (files[i].Contains(".meta"))
            {
                continue;
            }

            string json = File.ReadAllText(files[i]);
            _tileModels.Add(JsonUtility.FromJson<HexTileModel>(json));

            string shortFilename = files[i].Replace(jsonPath, "");
            shortFilename = shortFilename.Replace(".json", "");
            _tileContent.Add(new GUIContent(shortFilename));
        }
    }

    private HexBoardModel LoadHexBoard(string jsonFileName)
    {
        string jsonPath = Application.dataPath + "/Json/HexBoards/";
        if (!File.Exists(jsonPath + jsonFileName + ".json"))
        {
            Debug.LogError("Unable to find json file: " + jsonPath + jsonFileName + ".json");
            return null;
        }

        string json = File.ReadAllText(jsonPath + jsonFileName + ".json");
        return JsonUtility.FromJson<HexBoardModel>(json);
    }
}
