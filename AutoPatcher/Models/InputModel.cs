namespace AutoPatcher.Models
{
    internal sealed class InputModel : ModelBase
    {
        public InputModel(string title, string input0Label) : this(title, input0Label, null)
        {
        }

        public InputModel(string title, string input0Label, string input1Label)
        {
            this.Title = title;
            this.Input0Label = input0Label;
            this.Input1Label = input1Label;
            this.IsInput1Enabled = input1Label != null;
        }

        public string Title { get; }

        public string Input0Label { get; }

        public string Input0Text { get; set; }

        public bool IsInput1Enabled { get; }

        public string Input1Label { get; }

        public string Input1Text { get; set; }
    }
}
