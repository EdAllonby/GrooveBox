using System.Collections.Generic;
using System.Windows;

namespace GrooveBox.DragDropper
{
    /// <summary>
    /// Helper class to make a <see cref="DependencyObject" /> draggable to a drop target and an adorner.
    /// </summary>
    public static class DragEnabler
    {
        /// <summary>
        /// Make a <see cref="DependencyObject" /> draggable.
        /// </summary>
        /// <param name="dependencyObject">The object to make draggable</param>
        /// <param name="dropTargets">The drop targets names.</param>
        /// <param name="dragDropAdorner">The drag drop adorner implementation to use.</param>
        /// <param name="adornerLayerName">The adorner view layer.</param>
        public static void MakeObjectDraggable(DependencyObject dependencyObject, IEnumerable<string> dropTargets, DragDropAdorner dragDropAdorner, string adornerLayerName)
        {
            string targets = string.Join(",", dropTargets);

            dependencyObject.SetValue(DragDropHelper.IsDragSourceProperty, true);
            dependencyObject.SetValue(DragDropHelper.DropTargetsProperty, targets);
            dependencyObject.SetValue(DragDropHelper.AdornerLayerProperty, adornerLayerName);
            dependencyObject.SetValue(DragDropHelper.DragDropControlProperty, dragDropAdorner);
        }
    }
}