namespace HHG.Saving.Runtime
{
    public interface ISavable
    {
        bool CanSave() => true;
        SavableData Save();
        void Load(SavableData saveData);
        void OnBeforeSave() { }
        void OnAfterSave() { }
        void OnBeforeLoad() { }
        void OnAfterLoad() { }
    }
}