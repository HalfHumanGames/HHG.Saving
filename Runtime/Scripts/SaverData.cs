using HHG.Common.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HHG.SaveSystem.Runtime
{
    [Serializable]
    public class SaverData
    {
        public GameObject Prefab => AssetRegistry.GetAsset<GameObject>(PrefabGuid);
        public string Id;
        public string PrefabGuid;
        public string ParentPath;
        public Vector3 Position;
        public Quaternion Rotation;
        public bool IsTileGameObject;
        [SerializeReference] public List<SavableData> Data = new List<SavableData>();
    }
}