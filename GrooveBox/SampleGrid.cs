using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Utilities;

namespace GrooveBox
{
    public sealed class SampleGrid
    {
        private readonly List<GrooveContainer> grooveContainers = new List<GrooveContainer>();

        public IEnumerable<GrooveContainer> GrooveContainers
        {
            get { return grooveContainers.AsReadOnly(); }
        }

        public GrooveContainer GrooveContainerAtBar(int bar)
        {
            return grooveContainers.Find(container => container.BarPosition.Equals(bar));
        }

        public void CreateGrooveBuilderGrid(FrameworkElement context, Grid grid, IGridDimensions gridDimensions)
        {
            ResetGrid(context, grid);

            gridDimensions.CreateDefinitions(grid, new GridLength(1, GridUnitType.Star));

            int loopPosition = 1;

            // Traverse the columns and rows, populating each with borders.
            for (int row = 0; row < gridDimensions.Rows; row++)
            {
                for (int column = 0; column < gridDimensions.Columns; column++)
                {
                    if (gridDimensions.IsPositionAllowed(row, column))
                    {
                        GrooveContainer grooveContainer = CreateGrooveContainer(context, loopPosition);

                        Grid.SetRow(grooveContainer, row);
                        Grid.SetColumn(grooveContainer, column);
                        grid.Children.Add(grooveContainer);

                        loopPosition++;
                    }
                }
            }
        }

        public void ResetGrooveContainerBackgrounds()
        {
            foreach (var grooveContainer in grooveContainers)
            {
                grooveContainer.GrooveBackground = ApplicationColourPalette.Transparent;
            }
        }

        private GrooveContainer CreateGrooveContainer(FrameworkElement context, int loopPosition)
        {
            var grooveContainer = new GrooveContainer(loopPosition);
            context.SafelyRegisterControl(grooveContainer);
            grooveContainers.Add(grooveContainer);

            return grooveContainer;
        }

        private void ResetGrid(FrameworkElement context, Grid grid)
        {
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();
            UnregisterGrooveContainers(context);
            grooveContainers.Clear();
        }

        private void UnregisterGrooveContainers(FrameworkElement context)
        {
            foreach (GrooveContainer grooveContainer in grooveContainers)
            {
                context.SafelyUnregisterControl(grooveContainer);
            }
        }
    }
}