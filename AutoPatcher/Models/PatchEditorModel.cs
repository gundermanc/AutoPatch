using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using AutoPatcher.Abstractions;
using AutoPatcher.Config;
using AutoPatcher.Commands;

namespace AutoPatcher.Models
{
    internal sealed class PatchEditorModel : ModelBase
    {
        private readonly IErrorDialogs errDialogs;
        private readonly IFileDialogs dialogs;
        private ObservableCollection<SourceItemData> sourceItems;
        private BuildArtifactData previouslySelectedBuildArtifact;
        private BuildArtifactData selectedBuildArtifact;
        private SourceItemData selectedSourceItem;

        public PatchEditorModel(
            IErrorDialogs errDialogs,
            IFileDialogs dialogs,
            IEnumerable<BuildArtifactData> buildArtifacts)
        {
            this.errDialogs = errDialogs;
            this.dialogs = dialogs;
            this.BuildArtifacts = new ObservableCollection<BuildArtifactData>(buildArtifacts);

            this.AddBuildArtifactCommand = new AddBuildArtifactCommand(this.dialogs, this);
            this.EditBuildArtifactCommand = new EditBuildArtifactCommand(this.dialogs, this);
            this.RemoveBuildArtifactCommand = new RemoveBuildArtifactCommand(this.errDialogs, this);

            this.AddSourceItemCommand = new AddSourceItemCommand(this.dialogs, this);
            this.EditSourceItemCommand = new EditSourceItemCommand(this.dialogs, this);
            this.RemoveSourceItemCommand = new RemoveSourceItemCommand(this.errDialogs, this);

            this.PropertyChanged += PatchEditorModel_PropertyChanged;
        }

        public ICommand AddBuildArtifactCommand { get; }

        public ICommand EditBuildArtifactCommand { get; }

        public ICommand RemoveBuildArtifactCommand { get; }

        public ICommand AddSourceItemCommand { get; }

        public ICommand EditSourceItemCommand { get; }

        public ICommand RemoveSourceItemCommand { get; }

        public ObservableCollection<BuildArtifactData> BuildArtifacts { get; }

        public ObservableCollection<SourceItemData> SourceItems
        {
            get
            {
                return this.sourceItems;
            }

            set
            {
                if (this.sourceItems != value)
                {
                    this.sourceItems = value;
                    DispatchPropertyChanged(nameof(this.SourceItems));
                }
            }
        }

        public BuildArtifactData SelectedBuildArtifact
        {
            get
            {
                return this.selectedBuildArtifact;
            }

            set
            {
                if (this.selectedBuildArtifact != value)
                {
                    this.selectedBuildArtifact = value;
                    DispatchPropertyChanged(nameof(this.SelectedBuildArtifact));
                }
            }
        }

        public SourceItemData SelectedSourceItem
        {
            get
            {
                return this.selectedSourceItem;
            }

            set
            {
                if (this.selectedSourceItem != value)
                {
                    this.selectedSourceItem = value;
                    this.DispatchPropertyChanged(nameof(this.SelectedSourceItem));
                }
            }
        }

        private void PatchEditorModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.SelectedBuildArtifact))
            {
                UpdateSourceItemsBackingCollection();

                this.previouslySelectedBuildArtifact = this.SelectedBuildArtifact;

                if (this.SelectedBuildArtifact != null && this.SelectedBuildArtifact.SourceItems != null)
                {
                    this.SourceItems = new ObservableCollection<SourceItemData>(this.SelectedBuildArtifact.SourceItems);
                }
                else
                {
                    this.SourceItems = null;
                }
            }
        }

        public void UpdateSourceItemsBackingCollection()
        {
            // Save changes to list back to the backing data structure.
            if (this.previouslySelectedBuildArtifact != null && this.SourceItems != null)
            {
                this.previouslySelectedBuildArtifact.SourceItems.Clear();
                this.previouslySelectedBuildArtifact.SourceItems.AddRange(this.SourceItems);
            }
        }
    }
}
