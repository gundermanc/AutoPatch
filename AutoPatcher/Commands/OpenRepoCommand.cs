using System.Threading.Tasks;
using System.Windows;
using AutoPatcher.Config;
using AutoPatcher.Properties;

namespace AutoPatcher.Commands
{
    internal sealed class OpenRepoCommand : ModifyRepoStateCommandBase
    {
        public OpenRepoCommand(AppModel model) : base(model)
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
