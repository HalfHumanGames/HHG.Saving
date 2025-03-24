using HHG.Common.Runtime;
using UnityEngine;

namespace HHG.Saving.Runtime
{
    [RequireComponent(typeof(Saver), typeof(Health))]
    public class SaveHealth : MonoBehaviour, ISavable
    {
        private Lazy<Health> _health = new Lazy<Health>();
        private Health health => _health.FromComponent(this);

        [System.Serializable]
        public class Data : SavableData
        {
            public float Health;
        }

        public SavableData Save()
        {
            return new Data
            {
                Health = health.HealthValue
            };
        }

        public void Load(SavableData saveData)
        {
            Data data = saveData as Data;
            health.InitializeValue(data.Health);
        }
    }
}