using System.Windows;

namespace AutoPatcher
{
    internal sealed class ErrorDialogs : IErrorDialogs
    {
        public void WarningDialog(string message)
        {
            MessageBox.Show(message, "AutoPatcher Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void ErrorDialog(string message)
        {
            MessageBox.Show(message, "AutoPatcher Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void QueueExitAndErrorDialog(string message)
        {
            ErrorDialog(message);
            Application.Current.Dispatcher.Invoke(() => Application.Current.Shutdown());
        }
    }
}
