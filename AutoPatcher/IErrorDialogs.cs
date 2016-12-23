namespace AutoPatcher
{
    internal interface IErrorDialogs
    {
        void WarningDialog(string message);

        void ErrorDialog(string message);

        void QueueExitAndErrorDialog(string message);
    }
}
