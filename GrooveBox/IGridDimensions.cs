using System.Windows;
using System.Windows.Controls;

namespace GrooveBox
{
    public interface IGridDimensions
    {
        int Rows { get; }
        int Columns { get; }
        int Bars { get; }
        bool IsPositionAllowed(int row, int column);
        void CreateDefinitions(Grid grid, GridLength rowLength);
    }
}