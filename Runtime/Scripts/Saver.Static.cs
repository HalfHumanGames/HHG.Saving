using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HHG.SaveSystem.Runtime
{
    public partial class Saver
    {
        private static Dictionary<string, Saver> savers = new Dictionary<string, Saver>();
        private static List<string> destroy = new List<string>();

        static Saver()
        {
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            savers.Clear();
            destroy.Clear();
        }

        public static SaveData SaveAll()
        {
            SaveData data = new SaveData
            {
                Data = savers.Values.Select(s => s.Save()).ToList(),
                Destroy = new List<string>(destroy)
            };

            return data;
        }

        public static void LoadAll(SaveData saveData)
        {
            foreach (string id in saveData.Destroy)
            {
                if (savers.TryGetValue(id, out Saver saver))
                {
                    Destroy(saver.gameObject);
                }
            }

            foreach (SaverData data in saveData.Data)
            {
                Saver saver;
                if (savers.TryGetValue(data.Id, out saver))
                {
                    saver.Load(data);
                }
                else
                {
                    Transform parent = GameObject.Find(data.ParentPath)?.transform;
                    saver = Instantiate(data.Prefab, parent).GetComponent<Saver>();
                    saver.Initialize(data.Id);
                    saver.Load(data);
                }
            }
        }
    }
}