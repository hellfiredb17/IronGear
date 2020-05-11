using UnityEngine;
using UnityEditor;
using HexWorld;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Editor window to create a hex tile
/// </summary>
public class HexTileEditor : EditorWindow
{
    //---- Variables
    //--------------
    private static Vector2 MINSIZE = new Vector2(500, 500);
    private static Vector2 PREVIEW_WINDOW = new Vector2(250, 250);

    private static HexTileEditor _window;    
    private static HexTilePreferences _hexTilePreferences;    
    private static string _jsonPath;

    private PreviewRenderUtility _renderUtils;
    private HexTile _hexTile;
    private Rect _previewWindow;

    private string _filename = "Hex";

    private GUIContent[] _jsonContent;
    private int _jsonSelection;

    private GUIContent[] _textureContent;
    private int _textureIndex;

    private int _movementCost;
    private int _defenseRating;
    private HexWorld.TerrainType _terrainType;
    private Vector3 _scale = new Vector3(1, 1, 1);
    private Vector3 _rotation = new Vector3(315f, 315f, 0f);

    private bool _canSave = false;
    private string _output = "";

    //---- Menu
    //---------
    [MenuItem("IronGears/Design/Hex Editor")]
    static void Init()
    {
        // Load Data Context
        LoadContext();

        // Open / get open window for editing hex tiles
        _window = (HexTileEditor)EditorWindow.GetWindow(typeof(HexTileEditor));
        _window.minSize = MINSIZE;        
        _window.Show();        
    }

    //---- Enable/Disable/Destroy
    //---------------------------
    private void OnEnable()
    {
        _renderUtils = new PreviewRenderUtility();

        // Create hex tile
        _hexTile = Instantiate<HexTile>(_hexTilePreferences.Prefab);
        _renderUtils.AddSingleGO(_hexTile.gameObject);
        _renderUtils.camera.farClipPlane = 100;
        _renderUtils.camera.transform.position = new Vector3(0, 0, -10);
        _renderUtils.camera.transform.LookAt(_hexTile.transform, Vector3.up);

        // Get texture contents
        GetTexturesContent();
        _textureIndex = 0;

        // Updates
        UpdateRotation();
        UpdateScale();
        UpdateJsonOptions();
        _hexTile.View.SetMaterial(_hexTilePreferences.Materials["default"]);
        UpdateTexture(_textureContent[_textureIndex].text);
        _filename = "Hex_Temp";
    }

    private void OnDisable()
    {
        _renderUtils.Cleanup();        
    }

    //---- GUI
    //--------
    private void OnGUI()
    {
        GUILayout.BeginVertical();
        {
            GUILayout.Label("Hex Tile Editor", EditorStyles.boldLabel);
            GUILayout.Space(10);
            PreviewName();            
            GUILayout.Space(5);
            PreviewTexture();
            GUILayout.Space(5);
            PreviewDetails();
            GUILayout.Space(5);
            PreviewScale();
            GUILayout.Space(5);
            PreviewRotations();
            GUILayout.Space(5);
            SaveButton();
        }
        GUILayout.EndVertical();

        DrawWindow();
    }    

    private void PreviewName()
    {
        GUIHorizontalGroup(() =>
        {
            GUILayout.Label("File Name");
            int selection = EditorGUILayout.Popup(_jsonSelection, _jsonContent);
            if (selection != _jsonSelection)
            {
                _jsonSelection = selection;
                if (_jsonSelection != 0)
                {
                    HexTileModel data = LoadHexTileModel(_jsonContent[_jsonSelection].text);
                    if (data != null)
                    {
                        _textureIndex = IndexOfTexture(data.TextureName);
                        UpdateTexture(_textureContent[_textureIndex].text);
                        _movementCost = data.MovementCost;
                        _defenseRating = data.DefenseRating;
                        _terrainType = data.TerrainType;
                        _scale = data.Scale.Convert();
                        UpdateScale();
                    }
                }
                _filename = "Hex_" + _textureContent[_textureIndex].text + "_" + _scale.y;
            }

            EditorGUILayout.LabelField(_filename, EditorStyles.textArea);                    
        });
    }

    private int IndexOfTexture(string textureName)
    {
        int index = -1;
        for(int i = 0; i < _textureContent.Length; i++)
        {
            if(_textureContent[i].text == textureName)
            {
                index = i;
                break;
            }
        }

        // Error check
        if(index == -1)
        {
            Debug.LogError("Unable to get texture index from " + textureName);
            index = 0;
        }
        return index;
    }

    private void PreviewTexture()
    {
        GUIHorizontalGroup(() =>
        {
            GUILayout.Label("Texture");
            int selection = EditorGUILayout.Popup(_textureIndex, _textureContent);
            if (selection != _textureIndex)
            {
                _canSave = true;
                _textureIndex = selection;
                UpdateTexture(_textureContent[_textureIndex].text);
                _filename = "Hex_" + _textureContent[_textureIndex].text + "_" + _scale.y;
            }
        });
    }

    private void PreviewDetails()
    {
        GUILayout.Label("Preview Details");
        GUIHorizontalGroup(() =>
        {
            GUILayout.Label("MovementCost:");
            int movement = EditorGUILayout.DelayedIntField(_movementCost);
            if (movement != _movementCost)
            {
                _canSave = true;
                _movementCost = movement;
                _hexTile.Model.MovementCost = _movementCost;
            }
        });

        GUIHorizontalGroup(() =>
        {
            GUILayout.Label("DefenseRating:");
            int defense = EditorGUILayout.DelayedIntField(_defenseRating);
            if (defense != _defenseRating)
            {
                _canSave = true;
                _defenseRating = defense;
                _hexTile.Model.DefenseRating = _defenseRating;
            }
        });

        GUIHorizontalGroup(() =>
        {
            GUILayout.Label("TerrainType:");
            System.Enum selection = EditorGUILayout.EnumPopup(_terrainType);
            if ((TerrainType)selection != _terrainType)
            {
                _canSave = true;
                _terrainType = (TerrainType)selection;
                _hexTile.Model.TerrainType = _terrainType;
            }
        });
    }

    private void PreviewScale()
    {
        GUILayout.Label("Preview Scale");
        GUIHorizontalGroup(() =>
        {
            GUILayout.Label("X:");
            float x = EditorGUILayout.DelayedFloatField(_scale.x);
            if (x != _scale.x)
            {
                _canSave = true;
                _scale.x = x;
                UpdateScale();
            }
        });

        GUIHorizontalGroup(() =>
        {
            GUILayout.Label("Y:");
            float y = EditorGUILayout.DelayedFloatField(_scale.y);
            if (y != _scale.y)
            {
                _canSave = true;
                _scale.y = y;
                UpdateScale();
                _filename = "Hex_" + _textureContent[_textureIndex].text + "_" + _scale.y;
            }
        });

        GUIHorizontalGroup(() =>
        {
            GUILayout.Label("Z:");
            float z = EditorGUILayout.DelayedFloatField(_scale.z);
            if (z != _scale.z)
            {
                _canSave = true;
                _scale.z = z;
                UpdateScale();
            }
        });
    }

    private void PreviewRotations()
    {
        GUILayout.Label("Preview Rotations");
        GUIHorizontalGroup(() =>
        {
            GUILayout.Label("X:");           
            float x = EditorGUILayout.Slider(_rotation.x, 0, 359);
            if (x != _rotation.x)
            {
                _rotation.x = x;
                UpdateRotation();
            }
        });


        GUIHorizontalGroup(() =>
        {
            GUILayout.Label("Y:");
            float y = EditorGUILayout.Slider(_rotation.y, 0, 359);
            if (y != _rotation.y)
            {
                _rotation.y = y;
                UpdateRotation();
            }
        });

        GUIHorizontalGroup(() =>
        {
            GUILayout.Label("Z:");
            float z = EditorGUILayout.Slider(_rotation.z, 0, 359);
            if (z != _rotation.z)
            {
                _rotation.z = z;
                UpdateRotation();
            }
        });
    }

    private void SaveButton()
    {
        if (_canSave)
        {
            if (GUILayout.Button("Save Model"))
            {
                HexWorld.HexTileModel model = _hexTile.Model;
                model.MaterialName = "default";
                model.TextureName = _textureContent[_textureIndex].text;
                string file = _filename + ".json";
                string json = JsonUtility.ToJson(model);

                File.WriteAllText(_jsonPath + file, json);
                AssetDatabase.Refresh();                
                _output = "File " + file + " saved";
                UpdateJsonOptions();
                _canSave = false;
            }
        }

        if(!string.IsNullOrEmpty(_output))
        {
            GUILayout.Label(_output, EditorStyles.miniLabel);
        }
    }

    private void DrawWindow()
    {
        Vector2 size = _window.position.size;
        _previewWindow = new Rect((size.x) - PREVIEW_WINDOW.x - 5, (size.y) - PREVIEW_WINDOW.y - 5, PREVIEW_WINDOW.x, PREVIEW_WINDOW.y);
        _renderUtils.BeginPreview(_previewWindow, EditorStyles.helpBox);
        _renderUtils.Render();
        _renderUtils.EndAndDrawPreview(_previewWindow);
    }

    //---- Update Gameobject
    //----------------------
    public void UpdateTexture(string texture)
    {
        _hexTile.View.Renderer.sharedMaterial.SetTexture("_MainTex", _hexTilePreferences.Textures[texture]);
    }

    public void UpdateRotation()
    {
        _hexTile.transform.rotation = Quaternion.Euler(_rotation);
    }

    public void UpdateScale()
    {
        _hexTile.transform.localScale = _scale;
        _hexTile.Model.Scale = _scale.Convert();
    }

    public void UpdateJsonOptions()
    {   
        // Get files and names
        List<string> jsonFileNames = new List<string>();
        string[] files = Directory.GetFiles(_jsonPath, "*.json");
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Contains(".meta"))
                continue;
            string name = files[i].Replace(_jsonPath, "");
            name = name.Replace(".json", "");
            jsonFileNames.Add(name);
        }

        // Create GUIContent for each file
        _jsonContent = new GUIContent[jsonFileNames.Count + 1];
        _jsonContent[0] = new GUIContent("New");
        for(int i = 0; i < jsonFileNames.Count; i++)
        {
            _jsonContent[i+1] = new GUIContent(jsonFileNames[i]);
        }        
    }

    //---- Load Data 
    //--------------
    public static void LoadContext()
    {
        LoadPreference();
        _jsonPath = Application.dataPath + "/Json/HexTiles/";        
    }
    
    private static void LoadPreference()
    {
        string path = "Assets/Preferences/HexTilePreferences.asset";
        _hexTilePreferences = Instantiate(AssetDatabase.LoadAssetAtPath<HexTilePreferences>(path));
        if(_hexTilePreferences == null)        
            Debug.LogError("Unable to load Hex Tile Preferences @" + path);
    }

    private HexTileModel LoadHexTileModel(string jsonFileName)
    {
        if(!File.Exists(_jsonPath + jsonFileName + ".json"))
        {
            Debug.LogError("Unable to find json file: " + _jsonPath + jsonFileName + ".json");
            return null;
        }

        string json = File.ReadAllText(_jsonPath + jsonFileName + ".json");
        return JsonUtility.FromJson<HexTileModel>(json);        
    }

    private void GetTexturesContent()
    {
        _textureContent = new GUIContent[_hexTilePreferences.Textures.Count];
        int i = 0;
        foreach(string key in _hexTilePreferences.Textures.Keys)
        {
            _textureContent[i] = new GUIContent(key);
            i++;
        }
    }

    //---- Layout Helpers
    //-------------------
    private void GUIHorizontalGroup(System.Action action)
    {
        GUILayout.BeginHorizontal();
        action();
        GUILayout.EndHorizontal();
    }
}

