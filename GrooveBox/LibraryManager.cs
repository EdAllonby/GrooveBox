using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using GrooveBox.Domain;
using GrooveBox.DragDropper;
using GrooveBox.Properties;
using NAudio.Wave;
using Utilities;
using Image = System.Drawing.Image;

namespace GrooveBox
{
    public class LibraryManager
    {
        private readonly List<Groove> allGrooves = new List<Groove>();
        private readonly List<Groove> displayedGrooves = new List<Groove>();

        public LibraryManager(ResourceSet resourceSet)
        {
            int currentGrooveId = 1;

            XElement root = XElement.Parse(Resources.SampleLibrary);

            IEnumerable<XElement> genres = root.Elements("Genre");

            foreach (XElement genreElement in genres)
            {
                string genre = genreElement.Attribute("genre").Value;

                foreach (XElement grooveElement in genreElement.Elements())
                {
                    Groove groove = CreateGroove(resourceSet, grooveElement, currentGrooveId, genre);

                    allGrooves.Add(groove);

                    currentGrooveId++;
                }
            }
        }

        public List<Groove> DisplayedGrooves
        {
            get { return displayedGrooves; }
        }

        public IEnumerable<GrooveTagItem> InitialiseMagicBoxTags()
        {
            IEnumerable<string> allGrooveTags = allGrooves.SelectMany(x => x.Tags).Distinct().ToList();

            return allGrooveTags.Select(tagDescriptor => new GrooveTagItem(tagDescriptor));
        }

        public void FullyPopulateTree(TreeView grooveTree)
        {
            PopulateTree(grooveTree, string.Empty);
        }

        public void PopulateTreeWithSearchTerm(TreeView grooveTree, string searchTerm)
        {
            PopulateTree(grooveTree, searchTerm);
        }

        public Groove GetRandomGroove(List<string> tags)
        {
            if (!tags.Any())
            {
                return allGrooves.RandomElement();
            }

            List<Groove> grooves = allGrooves.Where(groove => groove.Tags.Intersect(tags).Any()).ToList();

            return grooves.RandomElement();
        }

        private void PopulateTree(ItemsControl grooveTree, string searchTerm)
        {
            grooveTree.Items.Clear();
            displayedGrooves.Clear();

            foreach (string genre in allGrooves.Select(x => x.Genre).Distinct())
            {
                TreeViewItem genreTree = CreateGenreTree(genre);

                foreach (Groove groove in allGrooves.Where(groove => groove.Genre.Equals(genre)))
                {
                    if (IsNamePartOfSearchTerm(searchTerm, groove) || IsGenrePartOfSearchTerm(searchTerm, groove))
                    {
                        TreeViewItem grooveTreeItem = CreateGrooveTreeItem(groove);

                        genreTree.Items.Add(grooveTreeItem);

                        displayedGrooves.Add(groove);
                    }
                }

                if (genreTree.Items.Count > 0)
                {
                    grooveTree.Items.Add(genreTree);
                }
            }
        }

        private static bool IsNamePartOfSearchTerm(string searchTerm, Groove groove)
        {
            return groove.Name.ToLower().Contains(searchTerm.ToLower());
        }

        private static bool IsGenrePartOfSearchTerm(string searchTerm, Groove groove)
        {
            return groove.Genre.ToLower().Contains(searchTerm.ToLower());
        }

        private static TreeViewItem CreateGenreTree(string genre)
        {
            var genreTree = new TreeViewItem
            {
                Name = genre,
                Header = genre,
                Foreground = ApplicationColourPalette.ItemColour,
                IsExpanded = true
            };

            return genreTree;
        }

        private static TreeViewItem CreateGrooveTreeItem(Groove groove)
        {
            var grooveItem = new TreeViewItem
            {
                Tag = groove.Id,
                Name = groove.Name,
                Header = groove.Name,
                DataContext = groove.Genre + " > " + groove.Name,
                Foreground = ApplicationColourPalette.ItemColour,
                IsExpanded = true,
                ToolTip = CreateGrooveTreeItemToolTip(groove.Tags)
            };

            var droppableElements = new List<string>();

            for (var loopId = 1; loopId <= 16; loopId++)
            {
                droppableElements.Add(string.Concat("Loop", loopId));
            }

            DragEnabler.MakeObjectDraggable(grooveItem, droppableElements, new GrooveDragDropAdorner(groove), "adornerLayer");

            return grooveItem;
        }

        private static StackPanel CreateGrooveTreeItemToolTip(List<string> tags)
        {
            StackPanel toolTipStackPanel = new StackPanel();

            TextBlock tagTitleTextBlock = new TextBlock
            {
                Text = "Tags",
                FontWeight = FontWeights.Bold
            };

            toolTipStackPanel.Children.Add(tagTitleTextBlock);

            TextBlock tagsTextBlock = new TextBlock
            {
                Text = string.Join(", ", tags)
            };

            toolTipStackPanel.Children.Add(tagsTextBlock);

            return toolTipStackPanel;
        }

        private static Groove CreateGroove(IEnumerable resourceSet, XContainer grooveElement, int currentGrooveId,
            string genre)
        {
            string grooveTitle = grooveElement.Elements("Title").Select(x => x.Value).FirstOrDefault();
            string grooveAudio = grooveElement.Elements("Audio").Select(x => x.Value).First();
            IEnumerable<string> grooveTags = grooveElement.Elements("Tags").First().Elements().Select(x => x.Value);

            Image image = null;
            Audio audio = null;

            foreach (DictionaryEntry resource in resourceSet)
            {
                var resourceName = (string) resource.Key;

                if (IsResourceAudio(resourceName, grooveAudio))
                {
                    IAudioVisualiser audioFileVisualiser = new AudioFileVisualiser();

                    string samplePath = Path.Combine("SampleLibrary", "Audio", (string) resource.Key);

                    using (var reader = new AudioFileReader(string.Concat(samplePath, ".wav")))
                    {
                        image = audioFileVisualiser.AudioFileToImage(reader);
                    }

                    byte[] audioData = ((Stream) resource.Value).StreamToBytes();

                    audio = new Audio(audioData, 100);
                }
            }

            return new Groove(currentGrooveId, genre, grooveTitle, image, audio, grooveTags.ToList());
        }

        private static bool IsResourceAudio(string resourceName, string grooveAudio)
        {
            return resourceName.Equals(grooveAudio);
        }
    }
}