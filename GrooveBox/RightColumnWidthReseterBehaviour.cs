using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace GrooveBox
{
    public class RightColumnWidthReseterBehaviour : Behavior<Expander>
    {
        private Grid parentGrid;

        public int TargetGridRowIndex { get; set; }

        protected override void OnAttached()
        {
            AssociatedObject.Expanded += (s, e) => ResetColumnDefinitions();
            AssociatedObject.Collapsed += (s, e) => ResetColumnDefinitions();
            FindParentGrid();
        }

        private void FindParentGrid()
        {
            DependencyObject parent = LogicalTreeHelper.GetParent(AssociatedObject);
            while (parent != null)
            {
                if (parent is Grid)
                {
                    parentGrid = parent as Grid;
                    return;
                }
                parent = LogicalTreeHelper.GetParent(AssociatedObject);
            }
        }

        private void ResetColumnDefinitions()
        {
            parentGrid.ColumnDefinitions[TargetGridRowIndex].Width = new GridLength(1.0, GridUnitType.Star);

            parentGrid.ColumnDefinitions[TargetGridRowIndex + 1].Width = GridLength.Auto;
        }
    }
}