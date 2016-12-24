namespace AutoPatcher.Abstractions
{
    internal interface IErrorDialogs
    {
        void WarningDialog(string message);

        void ErrorDialog(string message);

        void InformationDialog(string message);
    }
}
