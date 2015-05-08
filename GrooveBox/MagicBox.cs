using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GrooveBox
{
    public sealed class MagicBox
    {
        private readonly List<GrooveTagItem> grooveTagItems;

        public MagicBox(Grid magicBoxGrid, List<GrooveTagItem> tagItem, IGridDimensions gridDimensions)
        {
            grooveTagItems = tagItem;

            gridDimensions.CreateDefinitions(magicBoxGrid, GridLength.Auto);

            int count = 0;

            for (int row = 0; row < gridDimensions.Rows; row++)
            {
                for (int column = 0; column < gridDimensions.Columns; column++)
                {
                    count++;

                    if (count > grooveTagItems.Count)
                    {
                        return;
                    }

                    GrooveTagItem grooveTagItem = grooveTagItems[count - 1];

                    Grid.SetRow(grooveTagItem, row);
                    Grid.SetColumn(grooveTagItem, column);
                    magicBoxGrid.Children.Add(grooveTagItem);
                }
            }
        }

        public IEnumerable<string> SelectedTags
        {
            get
            {
                return grooveTagItems.Where(grooveTagItem => grooveTagItem.IsTicked)
                    .Select(grooveTagItem => grooveTagItem.TagDescriptor);
            }
        }
    }
}