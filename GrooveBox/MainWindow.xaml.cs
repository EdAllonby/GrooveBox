using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using GrooveBox.Domain;
using GrooveBox.DragDropper;
using GrooveBox.Player;

namespace GrooveBox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly GroovePlayer groovePlayer;
        private readonly LibraryManager libraryManager;
        private readonly MagicBox magicBox;
        private readonly SampleGrid sampleGrid = new SampleGrid();

        public MainWindow()
        {
            InitializeComponent();

            int defaultBars = SpecifiedBars.Value ?? 8;

            groovePlayer = new GroovePlayer(defaultBars);

            sampleGrid.CreateGrooveBuilderGrid(this, Builder, new NormalGridDimensions(defaultBars, 4));

            groovePlayer.NewBarStarted += OnNewBarStarted;

            libraryManager = new LibraryManager(GetResources());

            libraryManager.FullyPopulateTree(GroovesTree);

            List<GrooveTagItem> grooveTagItems = libraryManager.InitialiseMagicBoxTags().ToList();

            magicBox = new MagicBox(MagicBoxGrid, grooveTagItems, new NormalGridDimensions(grooveTagItems.Count, 4));

            DragDropHelper.ItemDropped += OnItemDropped;
        }

        private void OnNewBarStarted(object sender, CurrentBarEventArgs e)
        {
            sampleGrid.ResetGrooveContainerBackgrounds();

            sampleGrid.GrooveContainerAtBar(e.CurrentBar).GrooveBackground = ApplicationColourPalette.HighlightColour;
        }

        private static ResourceSet GetResources()
        {
            return Properties.Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true);
        }

        private void PlayClicked(object sender, RoutedEventArgs e)
        {
            groovePlayer.Play();
        }

        private void StopClicked(object sender, RoutedEventArgs e)
        {
            groovePlayer.Stop();
            ResetGrid();
        }

        private void ResetGrid()
        {
            sampleGrid.ResetGrooveContainerBackgrounds();
        }

        private void OnItemDropped(object sender, DragDropEventArgs e)
        {
            var droppedOnto = e.DroppedOn as GrooveContainer;
            var itemDragged = e.DraggedElement as TreeViewItem;

            if (droppedOnto != null && itemDragged != null)
            {
                GrooveContainer grooveContainer = sampleGrid.GrooveContainerAtBar(droppedOnto.BarPosition);

                Groove grooveDragged = libraryManager.DisplayedGrooves.Find(DoesItemDraggedMatchGroove(itemDragged));

                if (grooveContainer != null && grooveDragged != null)
                {
                    SetNewGroove(grooveContainer, grooveDragged);
                }
            }
        }

        private static Predicate<Groove> DoesItemDraggedMatchGroove(FrameworkElement itemDragged)
        {
            if (itemDragged.Tag == null)
            {
                return groove => false;
            }

            return groove => groove.Id.ToString().Equals(itemDragged.Tag.ToString());
        }

        private void OnRandomiseClicked(object sender, RoutedEventArgs e)
        {
            PopulateWholeGrid(new List<string>());
        }

        private void OnFillRemainingClicked(object sender, RoutedEventArgs e)
        {
            foreach (GrooveContainer grooveContainer in sampleGrid.GrooveContainers.Where(grooveContainer => !grooveContainer.ContainsGroove))
            {
                SetRandomGroove(new List<string>(), grooveContainer);
            }
        }

        private void OnMagicBoxCreateClicked(object sender, RoutedEventArgs e)
        {
            List<string> selectedTags = magicBox.SelectedTags.ToList();

            PopulateWholeGrid(selectedTags);
        }

        private void PopulateWholeGrid(List<string> tags)
        {
            foreach (GrooveContainer grooveContainer in sampleGrid.GrooveContainers)
            {
                SetRandomGroove(tags, grooveContainer);
            }
        }

        private void SetRandomGroove(List<string> tags, GrooveContainer grooveContainer)
        {
            Groove randomGroove = libraryManager.GetRandomGroove(tags);

            SetNewGroove(grooveContainer, randomGroove);
        }

        private void SetNewGroove(GrooveContainer grooveContainer, Groove groove)
        {
            grooveContainer.HoldNewGroove(groove);

            groovePlayer.ChangeAudio(groove.Audio, grooveContainer.BarPosition);
        }

        private void OnSpecifiedBarValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (groovePlayer != null)
            {
                int totalBars = (int) e.NewValue;
                sampleGrid.CreateGrooveBuilderGrid(this, Builder, new NormalGridDimensions(totalBars, 4));
                groovePlayer.TotalBars = totalBars;
                groovePlayer.ResetGrooves();
            }
        }

        private void OnSearchTermsTextChanged(object sender, TextChangedEventArgs e)
        {
            string newSearch = GrooveLibrarySearch.Text;

            libraryManager.PopulateTreeWithSearchTerm(GroovesTree, newSearch);
        }

        private void OnExportClicked(object sender, RoutedEventArgs e)
        {
            string fileName = SaveFileHelper.SaveFile("Wav File|*.wav", ".wav");

            if (fileName != null)
            {
                groovePlayer.ExportCurrentGrooves(fileName);
            }
        }

        private void ToggleButton_OnUnchecked(object sender, RoutedEventArgs e)
        {
            groovePlayer.Repeat = false;
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            groovePlayer.Repeat = true;
        }
    }
}