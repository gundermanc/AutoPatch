using System.IO;
using System.Threading.Tasks;
using System.Windows;
using AutoPatcher.Config;
using AutoPatcher.Models;
using AutoPatcher.Properties;

namespace AutoPatcher.Commands
{
    internal sealed class NewRepoCommand : ModifyRepoStateCommandBase
    {
        public NewRepoCommand(MainWindowModel model) : base(model)
        {
        }

        public override void Execute(object parameter)
        {
            var configPath = model.FileDialogs.NewFileDialog(
                Resources.StringNewRepoDialogTitle,
                AppConfigurationLoader.ConfigurationFileFilter,
                AppConfigurationLoader.ConfigurationFileName);

            if (!string.IsNullOrWhiteSpace(configPath))
            {
                if (Path.GetFileName(configPath) != AppConfigurationLoader.ConfigurationFileName)
                {
                    this.model.ErrorDialogs.WarningDialog(
                        string.Format(
                            Resources.StringRepoNewInvalidFileName,
                            AppConfigurationLoader.ConfigurationFileName));
                    return;
                }

                Task.Run(() => this.model.CreateAndLoadAppConfigurationAsync(configPath));
            }
        }
    }
}
