using System.Windows;
using Microsoft.Win32;

namespace AutoPatcher.Abstractions
{
    internal sealed class FileDialogs : IFileDialogs
    {
        public string NewFileDialog(Window owner, string title, string filter, string fileName)
        {
            var sfd = new SaveFileDialog()
            {
                Title = title,
                Filter = filter,
                AddExtension = true,
                DereferenceLinks = true,
                ValidateNames = true,
                RestoreDirectory = true,
                FileName = fileName
            };

            // Why on earth is this Nullable<bool>?? Assume failure if null.
            return (sfd.ShowDialog(owner) ?? false) ? sfd.FileName : null;
        }

        public string OpenFileDialog(Window owner, string title, string filter)
        {
            var ofd = new OpenFileDialog()
            {
                Title = title,
                Filter = filter,
                CheckFileExists = true,
                Multiselect = false,
                DereferenceLinks = true,
                ValidateNames = true,
                RestoreDirectory = true
            };

            // Why on earth is this Nullable<bool>?? Assume failure if null.
            return (ofd.ShowDialog(owner) ?? false) ? ofd.FileName : null;
        }
    }
}
