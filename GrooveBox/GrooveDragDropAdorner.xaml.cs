using System.Windows;
using System.Windows.Media;
using GrooveBox.Domain;
using GrooveBox.DragDropper;
using Utilities;

namespace GrooveBox
{
    /// <summary>
    /// Interaction logic for GrooveDragDropAdorner.xaml
    /// </summary>
    public partial class GrooveDragDropAdorner
    {
        public GrooveDragDropAdorner(Groove grooveToAdorn)
        {
            InitializeComponent();
            GrooveName.Text = grooveToAdorn.Name;
            GrooveWaveform.Source = ImageUtilities.ImageToImageSource(grooveToAdorn.WaveForm);
        }

        protected override void StateChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GrooveDragDropAdorner myclass = (GrooveDragDropAdorner) d;

            switch ((DropState) e.NewValue)
            {
                case DropState.CanDrop:
                    myclass.Back.Stroke = Application.Current.Resources["canDropBrush"] as SolidColorBrush;
                    myclass.Indicator.Source = Application.Current.Resources["dropIcon"] as DrawingImage;
                    break;
                case DropState.CannotDrop:
                    myclass.Back.Stroke = Application.Current.Resources["solidRed"] as SolidColorBrush;
                    myclass.Indicator.Source = Application.Current.Resources["noDropIcon"] as DrawingImage;
                    break;
            }
        }
    }
}