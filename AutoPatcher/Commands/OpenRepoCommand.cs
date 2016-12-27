using AutoPatcher.Config;
using AutoPatcher.Models;
using AutoPatcher.Properties;
using System.Threading.Tasks;
using System.Windows;

namespace AutoPatcher.Commands
{
    internal sealed class OpenRepoCommand : ModifyRepoStateCommandBase
    {
        public OpenRepoCommand(MainWindowModel model) : base(model)
        {
        }

        public override void Execute(object parameter)
        {
            var configPath = model.FileDialogs.OpenFileDialog(
                Application.Current.MainWindow,
                Resources.StringOpenRepoDialogTitle,
                AppConfigurationLoader.ConfigurationFileFilter);

            if (configPath != null)
            {
                Task.Run(() => model.LoadAppConfigurationAsync(configPath));
            }
        }
    }
}
