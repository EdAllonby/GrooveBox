using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace GrooveBox.DragDropper
{
    public class DragDropHelper
    {
        private const string CanDropResourceName = "CanDrop";
        private const string CannotDropResourceName = "CannotDrop";
        private static DragDropHelper instance;

        /// <summary>
        /// Flag the element as a drag source.
        /// </summary>
        public static readonly DependencyProperty IsDragSourceProperty =
            DependencyProperty.RegisterAttached("IsDragSource", typeof (bool), typeof (DragDropHelper),
                new UIPropertyMetadata(false, IsDragSourceChanged));

        /// <summary>
        /// Flag the element as a drag drop control.
        /// </summary>
        public static readonly DependencyProperty DragDropControlProperty =
            DependencyProperty.RegisterAttached("DragDropControl", typeof (UIElement), typeof (DragDropHelper),
                new UIPropertyMetadata(null));

        /// <summary>
        /// Flag the adorner.
        /// </summary>
        public static readonly DependencyProperty AdornerLayerProperty =
            DependencyProperty.RegisterAttached("AdornerLayer", typeof (string), typeof (DragDropHelper),
                new UIPropertyMetadata(null));

        /// <summary>
        /// Flag the drop targets. Delimit these with a comma character (',')
        /// </summary>
        public static readonly DependencyProperty DropTargetsProperty =
            DependencyProperty.RegisterAttached("DropTargets", typeof (string), typeof (DragDropHelper),
                new UIPropertyMetadata(string.Empty));

        private readonly List<UIElement> dropTargets = new List<UIElement>();
        private DragDropAdorner adorner;
        private Canvas adornerLayer;
        private Point delta;
        private Rect dropBoundingBox;
        private Point initialMousePosition;
        private FrameworkElement lastDraggedElement;
        private UIElement lastDroppedElement;
        private bool mouseCaptured;
        private Point scrollTarget;
        private Window topWindow;

        private static DragDropHelper Instance
        {
            get { return instance ?? (instance = new DragDropHelper()); }
        }

        /// <summary>
        /// Event when an item has been dropped
        /// </summary>
        public static event EventHandler<DragDropEventArgs> ItemDropped;

        /// <summary>
        /// Whether the dragged source is allowed to be dragged.
        /// </summary>
        /// <param name="obj">The object trying to get dragged.</param>
        /// <returns>If the object can get dragged.</returns>
        public static bool GetIsDragSource(DependencyObject obj)
        {
            return (bool) obj.GetValue(IsDragSourceProperty);
        }

        /// <summary>
        /// Set the object's draggability.
        /// </summary>
        /// <param name="obj">The object to set if draggable.</param>
        /// <param name="value">If the object can be dragged.</param>
        public static void SetIsDragSource(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDragSourceProperty, value);
        }

        public static UIElement GetDragDropControl(DependencyObject obj)
        {
            return (UIElement) obj.GetValue(DragDropControlProperty);
        }

        public static void SetDragDropControl(DependencyObject obj, UIElement value)
        {
            obj.SetValue(DragDropControlProperty, value);
        }

        public static string GetDropTargets(DependencyObject obj)
        {
            return (string) obj.GetValue(DropTargetsProperty);
        }

        public static void SetDropTargets(DependencyObject obj, string value)
        {
            obj.SetValue(DropTargetsProperty, value);
        }

        public static string GetAdornerLayer(DependencyObject obj)
        {
            return (string) obj.GetValue(AdornerLayerProperty);
        }

        public static void SetAdornerLayer(DependencyObject obj, string value)
        {
            obj.SetValue(AdornerLayerProperty, value);
        }

        private static void IsDragSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var dragSource = obj as UIElement;
            if (dragSource != null)
            {
                if (Equals(e.NewValue, true))
                {
                    dragSource.PreviewMouseLeftButtonDown += Instance.DragSource_PreviewMouseLeftButtonDown;
                    dragSource.PreviewMouseLeftButtonUp += Instance.OnDragSourcePreviewMouseLeftButtonUp;
                    dragSource.PreviewMouseMove += Instance.OnDragSourcePreviewMouseMove;
                }
                else
                {
                    dragSource.PreviewMouseLeftButtonDown -= Instance.DragSource_PreviewMouseLeftButtonDown;
                    dragSource.PreviewMouseLeftButtonUp -= Instance.OnDragSourcePreviewMouseLeftButtonUp;
                    dragSource.PreviewMouseMove -= Instance.OnDragSourcePreviewMouseMove;
                }
            }
        }

        private static FrameworkElement FindAncestor(Type ancestorType, Visual visual)
        {
            while (visual != null && !ancestorType.IsInstanceOfType(visual))
            {
                visual = (Visual) VisualTreeHelper.GetParent(visual);
            }
            return visual as FrameworkElement;
        }

        private static bool IsMovementBigEnough(Point initialMousePosition, Point currentPosition)
        {
            return (Math.Abs(currentPosition.X - initialMousePosition.X) >=
                    SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(currentPosition.Y - initialMousePosition.Y) >= SystemParameters.MinimumVerticalDragDistance);
        }

        private void DragSource_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Visual visual = e.OriginalSource as Visual;
                topWindow = (Window) FindAncestor(typeof (Window), visual);
                initialMousePosition = e.GetPosition(topWindow);

                string adornerLayerName = GetAdornerLayer(sender as DependencyObject);
                adornerLayer = (Canvas) topWindow.FindName(adornerLayerName);

                string dropTargetName = GetDropTargets(sender as DependencyObject);

                string[] targetNames = dropTargetName.Split(',');

                PopulateDropTargets(targetNames);

                FrameworkElement frameworkElement = sender as FrameworkElement;

                if (frameworkElement != null)
                {
                    lastDraggedElement = frameworkElement;
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("Exception in DragDropHelper: " + exc.InnerException);
            }
        }

        private void PopulateDropTargets(IEnumerable<string> targetNames)
        {
            dropTargets.Clear();

            foreach (string targetName in targetNames)
            {
                var dropTargetElement = (UIElement) topWindow.FindName(targetName);

                if (dropTargetElement != null)
                {
                    dropTargets.Add(dropTargetElement);
                }
            }
        }

        // Drag = mouse down + move by a certain amount
        private void OnDragSourcePreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseCaptured && lastDraggedElement != null)
            {
                // Only drag when user moved the mouse by a reasonable amount.
                if (IsMovementBigEnough(initialMousePosition, e.GetPosition(topWindow)))
                {
                    adorner = (DragDropAdorner) GetDragDropControl(sender as DependencyObject);
                    adorner.DataContext = lastDraggedElement.DataContext;
                    adorner.Opacity = 0.7;

                    adornerLayer.Visibility = Visibility.Visible;
                    adornerLayer.Children.Add(adorner);
                    mouseCaptured = Mouse.Capture(adorner);

                    Canvas.SetLeft(adorner, initialMousePosition.X - 20);
                    Canvas.SetTop(adorner, initialMousePosition.Y - 15);

                    adornerLayer.PreviewMouseMove += AdornerMouseMove;
                    adornerLayer.PreviewMouseUp += AdornerMouseUp;
                }
            }
        }

        private void AdornerMouseMove(object sender, MouseEventArgs e)
        {
            Point currentPoint = e.GetPosition(topWindow);
            currentPoint.X = currentPoint.X - 20;
            currentPoint.Y = currentPoint.Y - 15;

            delta = new Point(initialMousePosition.X - currentPoint.X, initialMousePosition.Y - currentPoint.Y);
            scrollTarget = new Point(initialMousePosition.X - delta.X, initialMousePosition.Y - delta.Y);

            Canvas.SetLeft(adorner, scrollTarget.X);
            Canvas.SetTop(adorner, scrollTarget.Y);

            adorner.AdornerDropState = DropState.CannotDrop;

            if (dropTargets != null)
            {
                foreach (UIElement dropTarget in dropTargets)
                {
                    GeneralTransform transform = dropTarget.TransformToVisual(adornerLayer);
                    dropBoundingBox =
                        transform.TransformBounds(new Rect(0, 0, dropTarget.RenderSize.Width,
                            dropTarget.RenderSize.Height));

                    if (e.GetPosition(adornerLayer).X > dropBoundingBox.Left &&
                        e.GetPosition(adornerLayer).X < dropBoundingBox.Right &&
                        e.GetPosition(adornerLayer).Y > dropBoundingBox.Top &&
                        e.GetPosition(adornerLayer).Y < dropBoundingBox.Bottom)
                    {
                        adorner.AdornerDropState = DropState.CanDrop;
                        lastDroppedElement = dropTarget;
                    }
                }
            }
        }

        private void AdornerMouseUp(object sender, MouseEventArgs e)
        {
            switch (adorner.AdornerDropState)
            {
                case DropState.CanDrop:
                    DropElement();
                    break;

                case DropState.CannotDrop:
                    DoNotDropElement();
                    break;
            }

            lastDraggedElement = null;
            adornerLayer.PreviewMouseMove -= AdornerMouseMove;
            adornerLayer.PreviewMouseUp -= AdornerMouseUp;

            if (adorner != null)
            {
                adorner.ReleaseMouseCapture();
            }

            adorner = null;
            mouseCaptured = false;
        }

        private void DropElement()
        {
            ((Storyboard) adorner.Resources[CanDropResourceName]).Completed += (s, args) =>
            {
                adornerLayer.Children.Clear();
                adornerLayer.Visibility = Visibility.Collapsed;
            };

            ((Storyboard) adorner.Resources[CanDropResourceName]).Begin(adorner);

            if (ItemDropped != null)
            {
                ItemDropped(adorner, new DragDropEventArgs(lastDraggedElement, lastDroppedElement));
            }
        }

        private void DoNotDropElement()
        {
            Storyboard storyboard = adorner.Resources[CannotDropResourceName] as Storyboard;

            if (storyboard != null)
            {
                DoubleAnimation aniX = storyboard.Children[0] as DoubleAnimation;

                if (aniX != null)
                {
                    aniX.To = delta.X;
                }

                DoubleAnimation aniY = storyboard.Children[1] as DoubleAnimation;

                if (aniY != null)
                {
                    aniY.To = delta.Y;
                }

                storyboard.Completed += (s, args) =>
                {
                    adornerLayer.Children.Clear();
                    adornerLayer.Visibility = Visibility.Collapsed;
                };

                storyboard.Begin(adorner);
            }
        }

        private void OnDragSourcePreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            lastDraggedElement = null;
            mouseCaptured = false;

            if (adorner != null)
            {
                adorner.ReleaseMouseCapture();
            }
        }
    }
}