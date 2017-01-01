namespace AutoPatcher.Engine.Abstractions
{
    public interface IErrorDialogs
    {
        void WarningDialog(string message);

        void ErrorDialog(string message);

        void InformationDialog(string message);

        bool QuestionDialog(string message);
    }
}
