using System;
using System.Collections.Generic;
using UnityEngine;

namespace HexWorld
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        public List<TKey> _keys;
        public List<TValue> _values;        

        public void OnBeforeSerialize()
        {                   
            _keys = new List<TKey>();
            _values = new List<TValue>();
            foreach (var kvp in this)
            {
                _keys.Add(kvp.Key);
                _values.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            this.Clear();
            if(_keys.Count != _values.Count)
            {
                throw new Exception("Serializeable Dictionary Keys and Values do not match");
            }

            for (int i = 0; i < _keys.Count; i++)
            {
                this.Add(_keys[i], _values[i]);
            }
        }
    }
    
    [Serializable]
    public class TextureDictionary : SerializableDictionary<HexType, Texture>
    { }    

    [Serializable]
    public class MaterialDictionary : SerializableDictionary<HexMaterial, Material>
    { }
}// end namespace
