using Microsoft.Win32;
using AutoPatcher.Engine.Abstractions;

namespace AutoPatcher.Abstractions
{
    internal sealed class FileDialogs : IFileDialogs
    {
        public string NewFileDialog(string title, string filter, string initialDirectory)
        {
            var sfd = new SaveFileDialog()
            {
                Title = title,
                Filter = filter,
                AddExtension = true,
                DereferenceLinks = true,
                ValidateNames = true,
                RestoreDirectory = true,
                InitialDirectory = initialDirectory
            };

            // Why on earth is this Nullable<bool>?? Assume failure if null.
            return (sfd.ShowDialog(null) ?? false) ? sfd.FileName : null;
        }

        public string OpenFileDialog(string title, string filter, string initialDirectory, bool ensureExists)
        {
            var ofd = new OpenFileDialog()
            {
                Title = title,
                Filter = filter,
                CheckFileExists = ensureExists,
                Multiselect = false,
                DereferenceLinks = true,
                ValidateNames = true,
                RestoreDirectory = true,
                InitialDirectory = initialDirectory
            };

            // Why on earth is this Nullable<bool>?? Assume failure if null.
            return (ofd.ShowDialog(null) ?? false) ? ofd.FileName : null;
        }

        public string OpenFolderDialog()
        {
            var ofd = new System.Windows.Forms.FolderBrowserDialog();

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return ofd.SelectedPath;
            }
            else
            {
                return null;
            }
        }
    }
}
