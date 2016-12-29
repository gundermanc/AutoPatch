using System.Windows;

namespace AutoPatcher.Abstractions
{
    internal interface IFileDialogs
    {
        string NewFileDialog(string title, string filter, string fileName);

        string OpenFileDialog(string title, string filter);

        string OpenFolderDialog();
    }
}