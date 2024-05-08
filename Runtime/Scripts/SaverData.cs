using HHG.Common.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HHG.SaveSystem.Runtime
{
    [Serializable]
    public class SaverData
    {
        public GameObject Prefab => ResourceDatabase.GetResourceByGuid<GameObject>(PrefabGuid);

        public string Id;
        public string PrefabGuid;
        public string ParentPath;
        [SerializeReference] public List<SavableData> Data = new List<SavableData>();
    }
}