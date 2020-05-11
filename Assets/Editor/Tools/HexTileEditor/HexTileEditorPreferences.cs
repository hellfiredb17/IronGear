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
    private StringTextureDictionary _textureDictionary;
    private StringMaterialDictionary _materialDictionary;    
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
        if (_textureDictionary == null)
        {
            _textureDictionary = _preferences.Textures;
        }

        if (_textureDictionary.Count == 0)
        {
            return;
        }        

        EditorGUILayout.BeginVertical();
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Textures", EditorStyles.boldLabel);
            List<string> keys = new List<string>(_textureDictionary.Keys);
            for(int i = 0; i < keys.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                {                    
                    // update text
                    string keyValue = EditorGUILayout.DelayedTextField(keys[i]);
                    if(keyValue != keys[i])
                    {
                        _isDirty = true;
                        Texture texture = _textureDictionary[keys[i]];
                        _textureDictionary.Remove(keys[i]);
                        _textureDictionary.Add(keyValue, texture);
                        return;
                    }
                    // update asset
                    Texture asset = EditorGUILayout.ObjectField(_textureDictionary[keys[i]], typeof(Texture), false) as Texture;
                    if(asset != _textureDictionary[keys[i]])
                    {
                        _isDirty = true;
                        _textureDictionary[keys[i]] = asset;                        
                    }
                    // remove
                    if(GUILayout.Button("-"))
                    {
                        _isDirty = true;
                        _textureDictionary.Remove(keys[i]);
                        return;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            // add
            if (GUILayout.Button("+"))
            {
                _isDirty = true;
                _textureDictionary.Add("", null);
            }
        }
        EditorGUILayout.EndVertical();
    }

    //---- Material Drawer
    //--------------------
    private void DrawMaterialDictionary()
    {        
        if (_materialDictionary == null)
        {
            _materialDictionary = _preferences.Materials;
        }

        if(_materialDictionary.Count == 0)
        {
            return;
        }

        EditorGUILayout.BeginVertical();
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Materials", EditorStyles.boldLabel);
            List<string> keys = new List<string>(_materialDictionary.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    // update text
                    string keyValue = EditorGUILayout.DelayedTextField(keys[i]);
                    if (keyValue != keys[i])
                    {
                        _isDirty = true;
                        Material material = _materialDictionary[keys[i]];
                        _materialDictionary.Remove(keys[i]);
                        _materialDictionary.Add(keyValue, material);
                        return;
                    }
                    // update asset
                    Material asset = EditorGUILayout.ObjectField(_materialDictionary[keys[i]], typeof(Material), false) as Material;
                    if (asset != _materialDictionary[keys[i]])
                    {
                        _isDirty = true;
                        _materialDictionary[keys[i]] = asset;
                    }
                    // remove
                    if (GUILayout.Button("-"))
                    {
                        _isDirty = true;
                        _materialDictionary.Remove(keys[i]);
                        return;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            // add
            if (GUILayout.Button("+"))
            {
                _isDirty = true;
                _materialDictionary.Add("", null);
            }
        }
        EditorGUILayout.EndVertical();
    }

    //---- Private
    //------------    
}
