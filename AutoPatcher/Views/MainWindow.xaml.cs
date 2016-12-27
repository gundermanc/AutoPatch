using System.Windows;
using AutoPatcher.Config;
using AutoPatcher.Models;

namespace AutoPatcher.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BuildArtifactsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // HACK: update model.
            var selectedArtifacts = ((MainWindowModel)this.DataContext).SelectedBuildArtifacts;

            selectedArtifacts.Clear();

            foreach (var selection in this.BuildArtifactsListBox.SelectedItems)
            {
                selectedArtifacts.Add((BuildArtifactData)selection);
            }
        }
    }
}
