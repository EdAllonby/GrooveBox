using System.Windows.Input;

namespace GrooveBox
{
    /// <summary>
    /// Interaction logic for GrooveTagItem.xaml
    /// </summary>
    public partial class GrooveTagItem
    {
        public GrooveTagItem(string tagDescriptor)
        {
            InitializeComponent();

            TagName.Content = tagDescriptor;
            TagName.Foreground = ApplicationColourPalette.ItemColour;
        }

        public string TagDescriptor
        {
            get { return (string) TagName.Content; }
        }

        public bool IsTicked { get; private set; }

        private void GrooveTagItem_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsTicked = !IsTicked;

            if (IsTicked)
            {
                GrooveTagItemBorder.Background = ApplicationColourPalette.HighlightColour;
                TagName.Foreground = ApplicationColourPalette.DarkColour;
            }
            else
            {
                GrooveTagItemBorder.Background = ApplicationColourPalette.Transparent;
                TagName.Foreground = ApplicationColourPalette.ItemColour;
            }
        }
    }
}