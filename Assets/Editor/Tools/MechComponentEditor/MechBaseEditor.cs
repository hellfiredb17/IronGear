using UnityEngine;
using UnityEditor;
using Rigs;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Editor tool for creating Mech base components
/// Results are saved out into json
/// </summary>
public class MechBaseEditor : MechComponentEditor
{
    //---- Variables
    //--------------
    private static MechBaseEditor _window;
    private MechBase _mechBase;

    //---- Unity
    //----------
    [MenuItem("IronGears/Design/Mech Component Base Editor")]
    static void Init()
    {
        // Open / get open window for editing hex tiles
        _window = (MechBaseEditor)EditorWindow.GetWindow(typeof(MechBaseEditor));
        _window.minSize = MINSIZE;
        _window.Show();
    }
    
    private void OnGUI()
    {
        _rect.x = 0;
        _rect.y = 0;

        _rect.width = LARGE_WIDTH;
        _rect.height = HEIGHT_SPACE;
        EditorGUI.LabelField(_rect, "Mech Information");
        NextLine();

        DisplayBaseInformation(_mechBase.Model);
        DisplayInformation();
        DrawWindow();
    }

    //---- Abstract Interface
    //-----------------------    
    protected override void SetupAssetContent()
    {
        // Load all assets for Base component
        _assetContent = new List<GUIContent>();
        _assetContent.Add(new GUIContent("None"));
        string path = Application.dataPath + _preferences.MechBaseContentPath;
        string[] files = Directory.GetFiles(path, "*.prefab");
        if (files == null || files.Length == 0)
        {
            Debug.LogError("Cannot find any files under " + path);
            return;
        }

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Contains(".meta"))
            {
                continue;
            }
            string shortFile = files[i].Replace(path, "");
            shortFile = shortFile.Replace(".prefab", "");
            _assetContent.Add(new GUIContent(shortFile));
        }
        _assetSelection = 0;
    }

    protected override void UpdateAssetPreview(string fileName)
    {
        if (fileName == "None")
        {
            _mechBase.View.RemoveBaseAsset();
            return;
        }

        string path = "Assets" + _preferences.MechBaseContentPath + fileName + ".prefab";
        BaseComponent baseComponent = Instantiate(AssetDatabase.LoadAssetAtPath<BaseComponent>(path));
        _mechBase.View.AttachBaseAsset(baseComponent);
    }

    protected override void DisplayInformation()
    {
        _rect.width = MEDIUM_WIDTH;
        EditorGUI.LabelField(_rect, "Energy");
        _rect.x += MEDIUM_WIDTH;
        _rect.width = MEDIUM_WIDTH;
        int energy = EditorGUI.DelayedIntField(_rect, _mechBase.Model.Energy);
        if (energy != _mechBase.Model.Energy)
        {
            _mechBase.Model.Energy = energy;
            _dirty = true;
        }
        NextLine();

        // Save file
        if (_dirty)
        {
            _rect.width = LARGE_WIDTH;
            if (GUI.Button(_rect, "Save Component"))
            {
                _dirty = false;
                SaveComponent();
            }
            NextLine();
        }
    }

    protected override void AddObjectToPreviewScene()
    {
        _mechBase = Instantiate<MechBase>(_preferences.MechBasePrefab);
        _renderUtils.AddSingleGO(_mechBase.gameObject);
        _renderUtils.camera.transform.LookAt(_mechBase.transform, Vector3.up);
    }

    protected override void SaveComponent()
    {
        string jsonFile = Application.dataPath + _preferences.MechBaseJsonPath + _mechBase.Model.Id + ".json";
        string json = JsonUtility.ToJson(_mechBase.Model, true);
        File.WriteAllText(jsonFile, json);
        AssetDatabase.Refresh();
    }    
}
