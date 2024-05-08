using System;
using System.Collections.Generic;
using UnityEngine;

namespace HHG.SaveSystem.Runtime
{
    [RequireComponent(typeof(Saver))]
    public class SaveMonoBehaviour : MonoBehaviour, ISavable
    {
        [Serializable]
        public class Data : SavableData
        {
            public List<string> Types = new List<string>();
            public List<string> Json = new List<string>();
        }

        public string Id => id;

        [SerializeField] private string id = Guid.NewGuid().ToString();
        [SerializeField] private MonoBehaviour[] monoBehaviours;

        public void Load(SavableData saveData)
        {
            Data data = saveData as Data;

            for (int i = 0; i < data.Types.Count; i++)
            {
                string type = data.Types[i];
                string json = data.Json[i];
                JsonUtility.FromJsonOverwrite(json, GetComponent(type));
            }
        }

        public SavableData Save()
        {
            Data data = new Data
            {
                Id = id
            };

            foreach (MonoBehaviour monoBehaviour in monoBehaviours)
            {
                data.Types.Add(monoBehaviour.GetType().FullName);
                data.Json.Add(JsonUtility.ToJson(monoBehaviour));
            }

            return data;
        }
    }
}