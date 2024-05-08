namespace HHG.SaveSystem.Runtime
{
    public interface ISavable
    {
        public string Id { get; }
        SavableData Save();
        void Load(SavableData data);
    }
}