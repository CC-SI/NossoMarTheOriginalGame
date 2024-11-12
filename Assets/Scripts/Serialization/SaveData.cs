using System;
using System.Collections.Generic;
using UnityEngine;

namespace Serialization
{
    [Serializable]
    public class SaveData
    {
        public Vector3 playerPosition;
        public List<int> ducks = new();

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
    
        public void FromJson(string json)
        {
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }
}