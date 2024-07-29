using HHG.Common.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HHG.SaveSystem.Runtime
{
    public partial class Saver
    {
        public static bool IsLoading => isLoading;

        private static Dictionary<string, Saver> savers = new Dictionary<string, Saver>();
        private static List<string> destroy = new List<string>();
        private static bool hasSceneLoaded;
        private static bool isLoading;

        public static event Action BeforeSave;
        public static event Action AfterSave;
        public static event Action BeforeLoad;
        public static event Action AfterLoad;

        static Saver()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            hasSceneLoaded = true;
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            hasSceneLoaded = false;
            savers.Clear();
            destroy.Clear();
        }

        public static SaveData SaveAll()
        {
            BeforeSave?.Invoke();

            SaveData data = new SaveData
            {
                Data = savers.Values.Select(s => s.Save()).ToList(),
                Destroy = new List<string>(destroy)
            };

            AfterSave?.Invoke();

            return data;
        }

        public static void LoadAll(SaveData saveData)
        {
            CoroutineUtil.StartCoroutine(LoadAllAsync(saveData));
        }

        private static IEnumerator LoadAllAsync(SaveData saveData)
        {
            isLoading = true;
            BeforeLoad?.Invoke();

            foreach (string id in saveData.Destroy)
            {
                if (savers.TryGetValue(id, out Saver saver))
                {
                    Destroy(saver.gameObject);
                }
            }

            foreach (SaverData data in saveData.Data.Where(s => !s.IsTileGameObject))
            {
                if (savers.TryGetValue(data.Id, out Saver saver))
                {
                    saver.Load(data);
                }
                else
                {
                    GameObject parentGO = GameObjectUtil.FindOrCreate(data.ParentPath);
                    Transform parent = parentGO == null ? null : parentGO.transform;
                    GameObject go = Instantiate(data.Prefab, data.Position, data.Rotation, parent);
                    saver = go.GetComponent<Saver>();
                    saver.Initialize(data.Id);
                    saver.Load(data);
                }
            }

            // Need to wait for the end of the next frame to make sure
            // that Saver.Start gets called for tilemap game objects,
            // but this happens at the end of the next frame, so that's
            // why we yield two WaitForEndOfFrames
            yield return WaitFor.EndOfFrame;
            // Saver.Start has still not yet been called at this point
            yield return WaitFor.EndOfFrame;

            foreach (SaverData data in saveData.Data.Where(s => s.IsTileGameObject))
            {
                if (savers.TryGetValue(data.Id, out Saver saver))
                {
                    saver.Load(data);
                }
            }

            AfterLoad?.Invoke();
            isLoading = false;
        }
    }
}