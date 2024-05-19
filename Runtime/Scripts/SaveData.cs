using HHG.Common.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HHG.SaveSystem.Runtime
{
    [Serializable]
    public class SaveData : ICloneable<SaveData>
    {
        [SerializeReference] public List<SaverData> Data = new List<SaverData>();
        public List<string> Destroy = new List<string>();

        public void Clear()
        {
            Data.Clear();
            Destroy.Clear();
        }

        public SaveData Clone()
        {
            SaveData clone = new SaveData();
            clone.Data = new List<SaverData>(Data);
            clone.Destroy = new List<string>(Destroy);
            return clone;
        }
    }
}