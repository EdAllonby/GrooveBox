using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GrooveBox
{
    public class NormalGridDimensions : IGridDimensions
    {
        private readonly int maximumColumnCount;

        public NormalGridDimensions(int bars, int maximumColumnCount)
        {
            Bars = bars;

            this.maximumColumnCount = maximumColumnCount;

            Columns = bars <= this.maximumColumnCount ? bars : this.maximumColumnCount;

            Rows = (int) Math.Ceiling(bars/(double) this.maximumColumnCount);
        }

        public int Columns { get; private set; }
        public int Rows { get; private set; }
        public int Bars { get; private set; }

        public bool IsPositionAllowed(int row, int column)
        {
            return column + (row*maximumColumnCount) < Bars;
        }

        public void CreateDefinitions(Grid grid, GridLength rowLength)
        {
            CreateRowDefinitions(grid, rowLength);
            CreateColumnDefinitions(grid);
        }

        private void CreateRowDefinitions(Grid grid, GridLength gridLength)
        {
            for (int row = 0; row < Rows; row++)
            {
                var rowDefinition = new RowDefinition {Height = gridLength};
                grid.RowDefinitions.Add(rowDefinition);
            }
        }

        private void CreateColumnDefinitions(Grid grid)
        {
            foreach (int column in Enumerable.Range(0, Columns))
            {
                var columnDefinition = new ColumnDefinition();
                grid.ColumnDefinitions.Add(columnDefinition);
            }
        }
    }
}