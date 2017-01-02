using AutoPatcher.Engine.Abstractions;

namespace AutoPatcher.Abstractions
{
    // TODO: Currently, this settings manager uses the default Dot Net app settings,
    // which requires all settings to be predefined. Consider switching to a config
    // or XML file so that this is no longer necessary and is more maintainable.
    internal sealed class SettingsManager : ISettingsManager
    {
        public void CommitChanges()
        {
            Properties.Settings.Default.Save();
        }

        public string GetSettingsValue(string key)
        {
            return Properties.Settings.Default[key] as string;
        }

        public void SetSettingsValue(string key, string val)
        {
            Properties.Settings.Default[key] = val;
        }
    }
}
