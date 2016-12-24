using System.Windows;

namespace AutoPatcher.Abstractions
{
    internal interface IFileDialogs
    {
        string NewFileDialog(Window owner, string title, string filter, string fileName);

        string OpenFileDialog(Window owner, string title, string filter);
    }
}