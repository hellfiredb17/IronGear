using UnityEngine;
using System.IO;
using HexWorld;

/// <summary>
/// Looks up and loads json file and returns object
/// </summary>
public static class JsonLoader
{    
    public static T Parse<T>(string file)
    {
        if(!file.Contains(".json"))
        {
            file += ".json";
        }       
        
        if (!File.Exists(file))
        {
            Debug.LogError("Unable to find json file: " + file);
            return default;
        }

        string json = File.ReadAllText(file);
        return JsonUtility.FromJson<T>(json);
    }
}
