using System;
using UnityEngine;

namespace HHG.SaveSystem.Runtime
{
    [RequireComponent(typeof(Saver))]
    public class SaveGameObjectActive : MonoBehaviour, ISavable
    {
        [Serializable]
        public class Data : SavableData
        {
            public bool Active;
        }

        public string Id => id;

        [SerializeField] private string id = Guid.NewGuid().ToString();

        public void Load(SavableData saveData)
        {
            Data data = saveData as Data;
            gameObject.SetActive(data.Active);
        }

        public SavableData Save()
        {
            return new Data
            {
                Id = id,
                Active = gameObject.activeSelf
            };
        }
    }
}