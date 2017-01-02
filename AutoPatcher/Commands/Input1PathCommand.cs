using AutoPatcher.Abstractions;
using AutoPatcher.Models;

namespace AutoPatcher.Commands
{
    internal sealed class Input1PathCommand : InputPathCommandBase
    {
        public Input1PathCommand(IAbstraction abstraction, PathInputModel model) : base(abstraction, model) { }

        public override void Execute(object parameter)
        {
            string inputText = base.model.Input1Text;

            this.ExecuteForInput(ref inputText, base.model.Input1RelativePathPrefix, base.model.Input1EnsureExists);

            base.model.Input1Text = inputText;
        }
    }
}
