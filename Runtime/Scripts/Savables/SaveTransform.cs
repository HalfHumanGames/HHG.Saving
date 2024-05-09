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
            public Vector3 LocalPosition;
            public Quaternion LocalRotation;
            public Vector3 LocalScale;
        }

        public void Load(SavableData saveData)
        {
            Data data = saveData as Data;
            transform.localPosition = data.LocalPosition;
            transform.localRotation = data.LocalRotation;
            transform.localScale = data.LocalScale;
        }

        public SavableData Save()
        {
            return new Data
            {
                LocalPosition = transform.localPosition,
                LocalRotation = transform.localRotation,
                LocalScale = transform.localScale
            };
        }
    }
}