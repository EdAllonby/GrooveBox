using System.Windows;
using System.Windows.Media;
using GrooveBox.DragDropper;

namespace GrooveBox
{
    /// <summary>
    /// Interaction logic for TextDragDropAdorner.xaml
    /// </summary>
    public partial class TextDragDropAdorner
    {
        public TextDragDropAdorner()
        {
            InitializeComponent();
        }

        protected override void StateChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextDragDropAdorner myclass = (TextDragDropAdorner) d;

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