using System.Windows;
using System.Windows.Media;
using GrooveBox.Domain;
using Utilities;

namespace GrooveBox
{
    /// <summary>
    /// Interaction logic for GrooveContainer.xaml
    /// </summary>
    public partial class GrooveContainer
    {
        public GrooveContainer(int barPosition)
        {
            InitializeComponent();

            BarPosition = barPosition;

            Name = string.Concat("Loop", BarPosition);

            ContainsGroove = false;
        }

        public Brush GrooveBackground
        {
            set { GrooveBorder.Background = value; }
        }

        /// <summary>
        /// The position of the groove container in the groove grid.
        /// </summary>
        public int BarPosition { get; private set; }

        public void HoldNewGroove(Groove groove)
        {
            var name = string.Concat(groove.Genre, " > ", groove.Name);

            if (!string.IsNullOrWhiteSpace(name))
            {
                GrooveNameHolder.Visibility = Visibility.Visible;
                GrooveNameHolder.Content = name;
            }

            GrooveTags.Text = string.Join(", ", groove.Tags);

            GrooveImage.Source = ImageUtilities.ImageToImageSource(groove.WaveForm);

            ContainsGroove = true;
        }

        public bool ContainsGroove { get; private set; }
    }
}