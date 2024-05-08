using System;
using System.Collections.Generic;
using UnityEngine;

namespace HHG.SaveSystem.Runtime
{
    [Serializable]
    public class SaveData
    {
        [SerializeReference] public List<SaverData> Data = new List<SaverData>();
        public List<string> Destroy = new List<string>();
    }
}