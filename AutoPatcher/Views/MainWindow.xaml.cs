using System.Linq;
using System.Windows;
using AutoPatcher.Models;

namespace AutoPatcher.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isSelectionUpdateInProgress;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var multiSelectable = ((IMultiSelectable)this.DataContext);
            multiSelectable.ModelChangedSelection += MultiSelectable_ModelChangedSelection;
        }

        private void MultiSelectable_ModelChangedSelection(object sender, System.EventArgs e)
        {
            // HACK: update view from model.

            this.isSelectionUpdateInProgress = true;

            var multiSelectable = ((IMultiSelectable)this.DataContext);
            this.BuildArtifactsListBox.SelectedItems.Clear();

            foreach (var selectedItem in multiSelectable.Selected)
            {
                this.BuildArtifactsListBox.SelectedItems.Add(selectedItem);
            }

            this.BuildArtifactsListBox.Focus();

            this.isSelectionUpdateInProgress = false;
        }

        private void BuildArtifactsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (!isSelectionUpdateInProgress)
            {
                // HACK: update model from view.
                var selectedArtifacts = ((IMultiSelectable)this.DataContext).Selected;

                selectedArtifacts.Clear();

                foreach (var selection in this.BuildArtifactsListBox.SelectedItems)
                {
                    selectedArtifacts.Add(selection);
                }
            }
        }
    }
}
