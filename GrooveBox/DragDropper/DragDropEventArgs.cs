using System;
using System.Windows;

namespace GrooveBox.DragDropper
{
    /// <summary>
    /// An event argument for when an element is dropped onto another.
    /// </summary>
    public class DragDropEventArgs : EventArgs
    {
        public readonly UIElement DraggedElement;
        public readonly UIElement DroppedOn;

        public DragDropEventArgs(UIElement draggedElement, UIElement droppedOn)
        {
            DraggedElement = draggedElement;
            DroppedOn = droppedOn;
        }
    }
}