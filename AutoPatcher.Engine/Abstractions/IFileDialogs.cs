namespace AutoPatcher.Engine.Abstractions
{
    public interface IFileDialogs
    {
        string NewFileDialog(string title, string filter, string initialDirectory);

        string OpenFileDialog(string title, string filter, string initialDirectory, bool ensureExists);

        string OpenFolderDialog();
    }
}