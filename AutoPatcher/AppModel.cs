using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using AutoPatcher.Config;

namespace AutoPatcher
{
    internal sealed class AppModel : MainWindowModelBase
    {
        public AppModel()
        {
#if DEBUG
            // Disallow model initialization in design view.
            if (!DesignerProperties.GetIsInDesignMode(Application.Current.MainWindow))
            {
#endif
                BeginInitialize();
#if DEBUG
            }
#endif
        }

        #region App Submodels

        private AppConfiguration AppConfig { get; set; }

        private IErrorDialogs ErrorDialogs { get; } = new ErrorDialogs();

        #endregion

        #region Window Properties

        public override string Title
        {
            get
            {
                return Properties.Resources.StringMainWindowTitle;
            }
        }

        #endregion

        #region Initialization

        private void BeginInitialize()
        {
            Task.Run(async () =>
            {
                this.IsBusy = true;

                if (!await TryInitializeAsync())
                {
                    Application.Current.Dispatcher.Invoke(() => Application.Current.Shutdown());
                }

                this.IsBusy = false;
            });
        }

        private async Task<bool> TryInitializeAsync()
        {
            var appConfig = await Task.Run(() => AppConfigurationFactory.CreateAppConfiguration(this.ErrorDialogs));

            if ((this.AppConfig = appConfig) == null)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
