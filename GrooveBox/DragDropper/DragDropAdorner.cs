using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GrooveBox.DragDropper
{
    /// <summary>
    /// Interaction logic for DragDropAdornerBase.xaml
    /// </summary>
    public abstract class DragDropAdorner : UserControl
    {
        protected DragDropAdorner()
        {
            ScaleTransform scale = new ScaleTransform(1f, 1f);
            SkewTransform skew = new SkewTransform(0f, 0f);
            RotateTransform rotate = new RotateTransform(0f);
            TranslateTransform trans = new TranslateTransform(0f, 0f);
            TransformGroup transGroup = new TransformGroup();
            transGroup.Children.Add(scale);
            transGroup.Children.Add(skew);
            transGroup.Children.Add(rotate);
            transGroup.Children.Add(trans);

            RenderTransform = transGroup;
        }

        public DropState AdornerDropState
        {
            get { return (DropState) GetValue(AdornerDropStateProperty); }
            set { SetValue(AdornerDropStateProperty, value); }
        }

        private static void DropStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DragDropAdorner myclass = (DragDropAdorner) d;
            myclass.StateChangedHandler(d, e);
        }

        protected abstract void StateChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e);
        // Using a DependencyProperty as the backing store for DropState.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AdornerDropStateProperty =
            DependencyProperty.Register("AdornerDropState", typeof (DropState), typeof (DragDropAdorner),
                new UIPropertyMetadata(DropStateChanged));
    }
}