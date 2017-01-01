namespace AutoPatcher.Engine.Abstractions
{
    public interface IFileDialogs
    {
        string NewFileDialog(string title, string filter, string fileName);

        string OpenFileDialog(string title, string filter);

        string OpenFolderDialog();
    }
}