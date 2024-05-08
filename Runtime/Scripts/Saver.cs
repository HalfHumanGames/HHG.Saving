using HHG.Common.Runtime;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Guid = System.Guid;

namespace HHG.SaveSystem.Runtime
{
    public partial class Saver : MonoBehaviour
    {
        [SerializeField] private string id = Guid.NewGuid().ToString();
        [SerializeField] private GameObject prefab;
        [SerializeField] private string prefabGuid;

        private bool instantiated;
        private Lazy<ISavable> _savables = new Lazy<ISavable>();
        private ISavable[] savables => _savables.FromComponents(this);

        private void Awake()
        {
            if (savers.ContainsKey(id))
            {
                id = Guid.NewGuid().ToString();
                instantiated = true;
            }

            savers[id] = this;
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
                ParentPath = instantiated ? transform.parent?.gameObject.GetPath() : null
            };

            foreach (ISavable savable in savables)
            {
                data.Data.Add(savable.Save());
            }

            return data;
        }

        public void Load(SaverData saverData)
        {
            foreach (SavableData data in saverData.Data)
            {
                savables.FirstOrDefault(s => s.Id == data.Id).Load(data);
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

            // Need this otherwise prefab and prefabGuid get unset on enter play mode
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