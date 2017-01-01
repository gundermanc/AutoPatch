using AutoPatcher.Engine.Abstractions;

namespace AutoPatcher.Abstractions
{
    internal sealed class Abstraction : IAbstraction
    {
        public IErrorDialogs ErrorDialogs { get; } = new ErrorDialogs();

        public IFileDialogs FileDialogs { get; } = new FileDialogs();

        public ISettingsManager SettingsManager { get; } = new SettingsManager();
    }
}
