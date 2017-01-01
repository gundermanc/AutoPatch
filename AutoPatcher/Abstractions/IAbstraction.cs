using AutoPatcher.Engine.Abstractions;

namespace AutoPatcher.Abstractions
{
    internal interface IAbstraction
    {
        IErrorDialogs ErrorDialogs { get; }

        IFileDialogs FileDialogs { get; }

        ISettingsManager SettingsManager { get; }
    }
}
