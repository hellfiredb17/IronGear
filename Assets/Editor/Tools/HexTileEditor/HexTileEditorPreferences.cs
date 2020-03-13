using UnityEngine;
using UnityEditor;
using HexWorld;
using System.Collections.Generic;

[CustomEditor(typeof(HexTilePreferences))]
public class HexTilePreferencesEditor : Editor
{
    //---- Variables
    //--------------
    private HexTilePreferences _preferences;
    private TextureDictionary _textureDictionary;
    private MaterialDictionary _materialDictionary;
    private System.Array _enumTextureValues;    
    private System.Array _enumMaterialValues;
    private bool _isDirty;

    //---- Functions
    //--------------   
    public override void OnInspectorGUI()
    {
        if(_preferences == null)
        {
            _preferences = target as HexTilePreferences;
        }

        base.OnInspectorGUI();
        if(_textureDictionary == null)
        {            
            _textureDictionary = _preferences.Textures;
        }
        if (_materialDictionary == null)
        {
            _materialDictionary = _preferences.Materials;
        }
        DrawTextureDictionary();
        DrawMaterialDictionary();

        // Set dirty so can save out        
        if(_isDirty)
        {
            _isDirty = false;            
            EditorUtility.SetDirty(_preferences);
            AssetDatabase.SaveAssets();
        }        
    }     

    //---- Texture Drawer
    //-------------------
    private void DrawTextureDictionary()
    {
        if (_enumTextureValues == null)
        {
            _enumTextureValues = System.Enum.GetValues(typeof(HexType));
        }
        if (_textureDictionary.Count != _enumTextureValues.Length)
        {
            SetTextureDefaults();
        }

        EditorGUILayout.BeginVertical();
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Textures", EditorStyles.boldLabel);
            List<HexType> keys = new List<HexType>(_textureDictionary.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    System.Enum type = EditorGUILayout.EnumPopup(keys[i]);
                    Texture asset = EditorGUILayout.ObjectField(_textureDictionary[keys[i]] ? _textureDictionary[keys[i]] : null, typeof(Texture), false) as Texture;
                    if ((asset != null && _textureDictionary[keys[i]] == null) ||
                        (asset != null && asset.name != _textureDictionary[keys[i]].name))
                    {
                        _textureDictionary[keys[i]] = asset;
                        _isDirty = true;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndVertical();
    }

    private void SetTextureDefaults()
    {
        foreach (var value in _enumTextureValues)
        {
            if (!_textureDictionary.ContainsKey((HexType)value))
            {
                _textureDictionary.Add((HexType)value, null);
                _isDirty = true;
            }
        }
    }

    //---- Material Drawer
    //--------------------
    private void DrawMaterialDictionary()
    {
        // Defaults
        if (_materialDictionary == null)
        {
            _materialDictionary = _preferences.Materials;
        }
        if (_enumMaterialValues == null)
        {
            _enumMaterialValues = System.Enum.GetValues(typeof(HexMaterial));
        }
        if (_materialDictionary.Count != _enumMaterialValues.Length)
        {
            SetDefaults();
        }

        EditorGUILayout.BeginVertical();
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Materials", EditorStyles.boldLabel);
            List<HexMaterial> keys = new List<HexMaterial>(_materialDictionary.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    System.Enum type = EditorGUILayout.EnumPopup(keys[i]);
                    Material asset = EditorGUILayout.ObjectField(_materialDictionary[keys[i]] ? _materialDictionary[keys[i]] : null, typeof(Material), false) as Material;
                    if ((asset != null && _materialDictionary[keys[i]] == null) ||
                        (asset != null && asset.name != _materialDictionary[keys[i]].name))
                    {
                        _materialDictionary[keys[i]] = asset;
                        _isDirty = true;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndVertical();
    }

    //---- Private
    //------------
    private void SetDefaults()
    {
        foreach (var value in _enumMaterialValues)
        {
            if (!_materialDictionary.ContainsKey((HexMaterial)value))
            {
                _materialDictionary.Add((HexMaterial)value, null);
                _isDirty = true;
            }
        }
    }
}
