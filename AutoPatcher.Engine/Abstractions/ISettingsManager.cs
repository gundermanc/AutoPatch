namespace AutoPatcher.Engine.Abstractions
{
    public interface ISettingsManager
    {
        string GetSettingsValue(string key);

        void SetSettingsValue(string key, string val);

        void CommitChanges();
    }
}
