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

        public PathInputModel(IAbstraction abstraction, string title, string input0Label, string input1Label = null, bool openFolderInsteadOfFile = false)
        {
            Debug.Assert(abstraction != null);

            this.abstraction = abstraction;

            this.Title = title;
            this.Input0Label = input0Label;
            this.Input1Label = input1Label;
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
