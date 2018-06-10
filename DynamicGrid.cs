using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LevelEditor
{
    public class DynamicGrid : Grid
    {

        public static readonly DependencyProperty NumColumnsProperty =
            DependencyProperty.Register("NumColumns", typeof(Int32), typeof(DynamicGrid));

        public Int32 NumColumns
        {
            get { return (Int32)GetValue(NumColumnsProperty); }
            set { SetValue(NumColumnsProperty, value); }
        }


        public static readonly DependencyProperty NumRowsProperty =
            DependencyProperty.Register("NumRows", typeof(Int32), typeof(DynamicGrid));

        public Int32 NumRows
        {
            get { return (Int32)GetValue(NumRowsProperty); }
            set { SetValue(NumRowsProperty, value); }
        }


        private void RecreateGridCells()
        {
            int numRows = NumRows;
            int currentNumRows = RowDefinitions.Count;

            while (numRows > currentNumRows)
            {
                RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                currentNumRows++;
            }

            while (numRows < currentNumRows)
            {
                currentNumRows--;
                RowDefinitions.RemoveAt(currentNumRows);
            }

            int numCols = NumColumns;
            int currentNumCols = ColumnDefinitions.Count;

            while (numCols > currentNumCols)
            {
                ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                currentNumCols++;
            }

            while (numCols < currentNumCols)
            {
                currentNumCols--;
                ColumnDefinitions.RemoveAt(currentNumCols);
            }

            UpdateLayout();

        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            RecreateGridCells();
        }

    }
}
