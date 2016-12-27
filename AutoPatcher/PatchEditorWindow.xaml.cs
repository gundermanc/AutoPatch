using System.Windows;

namespace AutoPatcher
{
    /// <summary>
    /// Interaction logic for PatchEditorWindow.xaml
    /// </summary>
    public partial class PatchEditorWindow : Window
    {
        public PatchEditorWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // HACK: update backing collection of currently selected binary artifact.
            ((PatchEditorModel)this.DataContext).UpdateSourceItemsBackingCollection();
            this.DialogResult = true;
            this.Close();
        }
    }
}
