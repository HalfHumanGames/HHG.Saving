using HHG.Common.Runtime;
using System;
using UnityEngine;

namespace HHG.SaveSystem.Runtime
{
    [RequireComponent(typeof(Saver))]
    public class SaveTransform : MonoBehaviour, ISavable
    {
        [Serializable]
        public class Data : SavableData
        {
            public Vector3 Position;
            public Quaternion Rotation;
            public Vector3 Scale;
        }

        public SavableData Save()
        {
            return new Data
            {
                Position = transform.position,
                Rotation = transform.rotation,
                Scale = transform.lossyScale
            };
        }

        public void Load(SavableData saveData)
        {
            Data data = saveData as Data;
            transform.position = data.Position;
            transform.rotation = data.Rotation;
            transform.SetGlobalScale(data.Scale);
        }
    }
}