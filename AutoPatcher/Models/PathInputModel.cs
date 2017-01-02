using System.Diagnostics;
using System.Windows.Input;
using AutoPatcher.Abstractions;
using AutoPatcher.Commands;

namespace AutoPatcher.Models
{
    internal sealed class PathInputModel : ModelBase
    {
        private readonly IAbstraction abstraction;
        private string input0Text;
        private string input1Text;

        public PathInputModel(
            IAbstraction abstraction,
            string title,
            string input0Label,
            string input1Label = null,
            string input0RelativePathPrefix = null,
            string input1RelativePathPrefix = null,
            bool input0EnsureExists = true,
            bool input1EnsureExists = true,
            bool openFolderInsteadOfFile = false)
        {
            Debug.Assert(abstraction != null);

            this.abstraction = abstraction;

            this.Title = title;
            this.Input0Label = input0Label;
            this.Input1Label = input1Label;
            this.Input0RelativePathPrefix = input0RelativePathPrefix;
            this.Input1RelativePathPrefix = input1RelativePathPrefix;
            this.Input0EnsureExists = input0EnsureExists;
            this.Input1EnsureExists = input1EnsureExists;
            this.IsInput1Enabled = input1Label != null;
            this.OpenFolderInsteadOfFile = openFolderInsteadOfFile;

            this.Input0PathCommand = new Input0PathCommand(this.abstraction, this);
            this.Input1PathCommand = new Input1PathCommand(this.abstraction, this);
        }

        public string Title { get; }

        public string Input0Label { get; }

        public string Input0Text
        {
            get
            {
                return this.input0Text;
            }

            set
            {
                if (this.input0Text != value)
                {
                    this.input0Text = value;
                    DispatchPropertyChanged(nameof(this.Input0Text));
                }
            }
        }

        public string Input0RelativePathPrefix { get; }

        public string Input1RelativePathPrefix { get; }

        public bool Input0EnsureExists { get; }

        public bool Input1EnsureExists { get; }

        public bool IsInput1Enabled { get; }

        public string Input1Label { get; }

        public string Input1Text
        {
            get
            {
                return this.input1Text;
            }

            set
            {
                if (this.input1Text != value)
                {
                    this.input1Text = value;
                    DispatchPropertyChanged(nameof(this.Input1Text));
                }
            }
        }

        public bool OpenFolderInsteadOfFile { get; }

        public ICommand Input0PathCommand { get; }

        public ICommand Input1PathCommand { get; }
    }
}
