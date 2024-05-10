using HHG.Common.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HHG.SaveSystem.Runtime
{
    public partial class Saver : MonoBehaviour
    {
        [SerializeField] private string id = Guid.NewGuid().ToString();
        [SerializeField] private GameObject prefab;
        [SerializeField] private string prefabGuid;

        private bool instantiated;
        private bool isTileGameObject;
        private Dictionary<string, ISavable> _savables;
        private Dictionary<string, ISavable> savables {
            get
            {
                if (_savables == null)
                {
                    _savables = GetComponents<ISavable>().ToDictionary(s => s.GetType().FullName, s => s);
                }
                return _savables;
            }
        }

        private void Awake()
        {
            if (savers.ContainsKey(id))
            {
                id = Guid.NewGuid().ToString();
                instantiated = true;
            }

            savers[id] = this;

        }

        private void Start()
        {
            // transform.parent is null in Awake for tile game objects,
            // so we check isTileGameObject here in Start instead
            isTileGameObject = GetComponentInParent<Tilemap>(true);
        }

        public void Initialize(string guid)
        {
            if (savers.ContainsKey(id))
            {
                savers.Remove(id);
            }

            id = guid;

            savers[id] = this;
        }

        public SaverData Save()
        {
            SaverData data = new SaverData
            {
                Id = id,
                PrefabGuid = instantiated ? prefabGuid : null,
                ParentPath = instantiated ? transform.parent?.gameObject.GetPath() : null,
                IsTileGameObject = isTileGameObject
            };

            foreach (ISavable savable in savables.Values)
            {
                SavableData savableData = savable.Save();
                savableData.Type = savable.GetType().FullName;
                data.Data.Add(savableData);
            }

            return data;
        }

        public void Load(SaverData saverData)
        {
            foreach (SavableData data in saverData.Data)
            {
                if (savables.TryGetValue(data.Type, out ISavable savable))
                {
                    savable.Load(data);
                }
            }
        }

        private void OnDestroy()
        {
            if (savers.ContainsKey(id))
            {
                savers.Remove(id);

                if (!instantiated)
                {
                    destroy.Add(id);
                }
            }
        }

        private void OnValidate()
        {
            id = GuidUtil.EnsureUnique(this, s => s.id);

            // The prefab and prefabGuid fields get unset on enter play mode
            // So we only want to update them when the application is not playing
            if (!Application.isPlaying)
            {
#if UNITY_EDITOR
                prefab = PrefabUtility.GetCorrespondingObjectFromSource(gameObject);
                prefabGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetOrScenePath(prefab));
#endif
            }
        }
    }
}