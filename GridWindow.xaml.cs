using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LevelEditor
{
    /*This is read only?*/
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GridWindow : Window
    {
        public GridWindow()
        {
            InitializeComponent();
            CreateGrid(5,5,dynamicGrid, 70);
        }

        /*CreateGrid
         * Pass in the number of rows and columns
         * you would like to have in your grid level
         */
        public void CreateGrid( int cols, int rows, Grid parentGrid, int tilesize )
        {
            ColumnDefinition c0 = new ColumnDefinition();
            c0.Width = new GridLength(1, GridUnitType.Star);
            parentGrid.ColumnDefinitions.Add(c0);
            for (int i = 0; i < cols; i++)
            {
                ColumnDefinition tempcol = new ColumnDefinition();
                tempcol.Width = new GridLength(tilesize, GridUnitType.Pixel);
                parentGrid.ColumnDefinitions.Add(tempcol);
            }
            ColumnDefinition c2 = new ColumnDefinition();
            c2.Width = new GridLength(1, GridUnitType.Star);
            parentGrid.ColumnDefinitions.Add(c2);

            RowDefinition r0 = new RowDefinition();
            r0.Height = new GridLength(1, GridUnitType.Star);
            parentGrid.RowDefinitions.Add(r0);
            for (int i = 0; i < rows; i++)
            {
                RowDefinition temprow = new RowDefinition();
                temprow.Height = new GridLength(tilesize, GridUnitType.Pixel);
                parentGrid.RowDefinitions.Add(temprow);
            }
            RowDefinition r2 = new RowDefinition();
            r2.Height = new GridLength(1, GridUnitType.Star);
            parentGrid.RowDefinitions.Add(r2);

            Image image = new Image();
            image.Height = tilesize;
            image.Width = tilesize;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(@"/LevelEditor;/Images/grass.png", UriKind.Relative);
            bitmapImage.DecodePixelHeight = tilesize;
            bitmapImage.DecodePixelWidth = tilesize;
            bitmapImage.EndInit();
            image.Source = bitmapImage;

            Grid.SetColumn(image, 1);
            Grid.SetRow(image, 1);
            parentGrid.Children.Add(image);


            /*POPULATE ALL CELLS (with a button in this case)*/
            //for (int i = 1; i <= cols; i++)
            //{
            //    for (int j = 1; j <= rows; j++)
            //    {
            //        Button b1 = new Button
            //        {
            //            HorizontalAlignment = HorizontalAlignment.Stretch,
            //            VerticalAlignment = VerticalAlignment.Stretch,
            //            Content = ("b" + i.ToString() + " " + j.ToString())
            //        };
            //        Grid.SetColumn(b1, i);
            //        Grid.SetRow(b1, j);
            //        parentGrid.Children.Add(b1);
            //    }
            //}
        }
    }
}
