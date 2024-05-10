namespace HHG.SaveSystem.Runtime
{
    public interface ISavable
    {
        bool CanSave() => true;
        SavableData Save();
        void Load(SavableData saveData);
    }
}