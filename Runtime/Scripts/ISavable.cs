namespace HHG.SaveSystem.Runtime
{
    public interface ISavable
    {
        SavableData Save();
        void Load(SavableData saveData);
    }
}