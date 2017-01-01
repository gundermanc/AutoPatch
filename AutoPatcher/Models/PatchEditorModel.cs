using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using AutoPatcher.Abstractions;
using AutoPatcher.Commands;
using AutoPatcher.Engine.Repository;

namespace AutoPatcher.Models
{
    internal sealed class PatchEditorModel : ModelBase
    {
        private readonly IAbstraction abstraction;
        private ObservableCollection<SourceItem> sourceItems;
        private BuildArtifact previouslySelectedBuildArtifact;
        private BuildArtifact selectedBuildArtifact;
        private SourceItem selectedSourceItem;

        public PatchEditorModel(
            IAbstraction abstraction,
            IEnumerable<BuildArtifact> buildArtifacts)
        {
            this.abstraction = abstraction;
            this.BuildArtifacts = new ObservableCollection<BuildArtifact>(buildArtifacts);

            this.AddBuildArtifactCommand = new AddBuildArtifactCommand(this.abstraction, this);
            this.EditBuildArtifactCommand = new EditBuildArtifactCommand(this.abstraction, this);
            this.RemoveBuildArtifactCommand = new RemoveBuildArtifactCommand(this.abstraction, this);

            this.AddSourceItemCommand = new AddSourceItemCommand(this.abstraction, this);
            this.EditSourceItemCommand = new EditSourceItemCommand(this.abstraction, this);
            this.RemoveSourceItemCommand = new RemoveSourceItemCommand(this.abstraction, this);

            this.PropertyChanged += PatchEditorModel_PropertyChanged;
        }

        public ICommand AddBuildArtifactCommand { get; }

        public ICommand EditBuildArtifactCommand { get; }

        public ICommand RemoveBuildArtifactCommand { get; }

        public ICommand AddSourceItemCommand { get; }

        public ICommand EditSourceItemCommand { get; }

        public ICommand RemoveSourceItemCommand { get; }

        public ObservableCollection<BuildArtifact> BuildArtifacts { get; }

        public ObservableCollection<SourceItem> SourceItems
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

        public BuildArtifact SelectedBuildArtifact
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

        public SourceItem SelectedSourceItem
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
                    this.SourceItems = new ObservableCollection<SourceItem>(this.SelectedBuildArtifact.SourceItems);
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

                foreach (var item in this.SourceItems)
                {
                    this.previouslySelectedBuildArtifact.SourceItems.Add(item);
                }
            }
        }
    }
}
