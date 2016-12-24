using System.Windows;
using AutoPatcher.Properties;

namespace AutoPatcher.Abstractions
{
    internal sealed class ErrorDialogs : IErrorDialogs
    {
        public void WarningDialog(string message)
        {
            MessageBox.Show(message, Resources.StringWarningDialogTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void ErrorDialog(string message)
        {
            MessageBox.Show(message, Resources.StringErrorDialogTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void InformationDialog(string message)
        {
            MessageBox.Show(message, Resources.StringInformationDialogTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
