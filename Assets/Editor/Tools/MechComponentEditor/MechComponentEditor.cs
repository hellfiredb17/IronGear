using UnityEngine;
using UnityEditor;
using Rigs;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Base Editor tool for creating Mech base components
/// Results are saved out into json
/// </summary>
public abstract class MechComponentEditor : EditorWindow
{
    //---- Variables
    //--------------
    protected static Vector2 MINSIZE = new Vector2(500, 500);
    protected static Vector2 PREVIEW_WINDOW = new Vector2(250, 250);
    protected static float HEIGHT_SPACE = 20.0f;
    protected static float LINE_SPACE = HEIGHT_SPACE + 5.0f;
    protected static float SMALL_WIDTH = 20.0f;
    protected static float MEDIUM_WIDTH = 100.0f;
    protected static float LARGE_WIDTH = 200.0f;

    protected MechComponentPreferences _preferences;
    protected List<GUIContent> _assetContent;
    protected int _assetSelection;
    protected PreviewRenderUtility _renderUtils;
    protected Camera _previewCamera;
    protected Rect _rect;
    protected bool _dirty;

    //---- Protected
    //--------------
    protected virtual void OnEnable()
    {
        LoadPreference();
        SetupPreview();
        SetupAssetContent();
        _rect = new Rect(0, 0, this.position.width, this.position.height);
    }

    protected virtual void SetupPreview()
    {
        _renderUtils = new PreviewRenderUtility();

        // Add camera into scene
        _renderUtils.camera.farClipPlane = 100;
        _renderUtils.camera.transform.position = new Vector3(0, 5, 15);
        _previewCamera = _renderUtils.camera;
        AddObjectToPreviewScene();
    }    

    protected virtual void OnDisable()
    {
        _renderUtils.Cleanup();
    }    

    protected virtual void DisplayBaseInformation(MechComponentModel model)
    {
        _rect.width = MEDIUM_WIDTH;
        EditorGUI.LabelField(_rect, "ID");
        _rect.x += MEDIUM_WIDTH;
        _rect.width = MEDIUM_WIDTH;
        string id = EditorGUI.DelayedTextField(_rect, model.Id);
        if (id != model.Id)
        {
            model.Id = id;
            _dirty = true;
        }
        NextLine();

        _rect.width = MEDIUM_WIDTH;
        EditorGUI.LabelField(_rect, "Display Name");
        _rect.x += MEDIUM_WIDTH;
        string displayName = EditorGUI.DelayedTextField(_rect, model.DisplayName);
        if (displayName != model.DisplayName)
        {
            model.DisplayName = displayName;
            _dirty = true;
        }
        NextLine();

        _rect.width = MEDIUM_WIDTH;
        EditorGUI.LabelField(_rect, "Hit Points");
        _rect.x += MEDIUM_WIDTH;
        _rect.width = MEDIUM_WIDTH;
        int hitPoints = EditorGUI.DelayedIntField(_rect, model.HitPoints);
        if (hitPoints != model.HitPoints)
        {
            model.HitPoints = hitPoints;
            _dirty = true;
        }
        NextLine();

        _rect.width = MEDIUM_WIDTH;
        EditorGUI.LabelField(_rect, "Weight");
        _rect.x += MEDIUM_WIDTH;
        _rect.width = MEDIUM_WIDTH;
        int weight = EditorGUI.DelayedIntField(_rect, model.Weight);
        if (weight != model.Weight)
        {
            model.Weight = weight;
            _dirty = true;
        }
        NextLine();

        // 3d Model to use
        _rect.width = MEDIUM_WIDTH;
        EditorGUI.LabelField(_rect, "Asset");
        _rect.x += MEDIUM_WIDTH;
        int selection = EditorGUI.Popup(_rect, _assetSelection, _assetContent.ToArray());
        if (selection != _assetSelection)
        {
            _assetSelection = selection;
            model.ModelAsset = _assetContent[_assetSelection].text;
            UpdateAssetPreview(model.ModelAsset);
            _dirty = true;
        }
        NextLine();
    }

    protected virtual void DrawWindow()
    {
        _rect.width = PREVIEW_WINDOW.x;
        _rect.height = PREVIEW_WINDOW.y;
        _rect.x += 5;
        _renderUtils.BeginPreview(_rect, EditorStyles.helpBox);
        _renderUtils.Render();
        _renderUtils.EndAndDrawPreview(_rect);
        NextLine();
    }    

    protected void NextLine()
    {
        _rect.x = 0;
        _rect.y += LINE_SPACE;
    }

    protected void LoadPreference()
    {
        string path = "Assets/Preferences/MechComponentPreferences.asset";
        _preferences = Instantiate(AssetDatabase.LoadAssetAtPath<MechComponentPreferences>(path));
        if (_preferences == null)
        {
            Debug.LogError("Unable to load Preferences @" + path);
        }
    }

    //---- Abstract
    //-------------
    protected abstract void SetupAssetContent();
    protected abstract void UpdateAssetPreview(string fileName);
    protected abstract void DisplayInformation();
    protected abstract void AddObjectToPreviewScene();
    protected abstract void SaveComponent();
}
