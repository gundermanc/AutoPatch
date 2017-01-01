using AutoPatcher.Abstractions;
using AutoPatcher.Models;

namespace AutoPatcher.Commands
{
    internal sealed class Input0PathCommand : InputPathCommandBase
    {
        public Input0PathCommand(IAbstraction abstraction, PathInputModel model) : base(abstraction, model) { }

        public override void Execute(object parameter)
        {
            string inputText = base.model.Input0Text;

            this.ExecuteForInput(ref inputText, base.model.Input0RelativePathPrefix, base.model.Input0EnsureExists);

            base.model.Input0Text = inputText;
        }
    }
}
