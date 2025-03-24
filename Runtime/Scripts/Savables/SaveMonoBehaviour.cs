using System;
using System.Collections.Generic;
using UnityEngine;

namespace HHG.Saving.Runtime
{
    [RequireComponent(typeof(Saver))]
    public class SaveMonoBehaviour : MonoBehaviour, ISavable
    {
        [SerializeField] private MonoBehaviour[] monoBehaviours;

        [Serializable]
        public class Data : SavableData
        {
            public List<string> Types = new List<string>();
            public List<string> Json = new List<string>();
        }

        public SavableData Save()
        {
            Data data = new Data();
            foreach (MonoBehaviour monoBehaviour in monoBehaviours)
            {
                data.Types.Add(monoBehaviour.GetType().FullName);
                data.Json.Add(JsonUtility.ToJson(monoBehaviour));
            }
            return data;
        }

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
    }
}