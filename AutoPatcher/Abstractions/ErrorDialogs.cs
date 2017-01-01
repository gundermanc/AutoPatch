using System.Windows;
using AutoPatcher.Engine.Abstractions;
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

        public bool QuestionDialog(string message)
        {
            return MessageBox.Show(
                message,
                Resources.StringQuestionDialogTitle,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes;
        }
    }
}
