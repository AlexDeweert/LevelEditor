using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
            CreateGrid(5,5,dynamicGrid, 32);
        }

        /*CreateGrid
         * Pass in the number of rows and columns
         * you would like to have in your grid level
         */
        public void CreateGrid( int cols, int rows, Grid parentGrid, int tilesize )
        {
            MouseButtonEventHandler leftclick = new MouseButtonEventHandler((e, args) => {
                HandleLeftClickOnGrid(parentGrid, args, tilesize, rows, cols);
            });

            parentGrid.PreviewMouseLeftButtonDown += leftclick;
            //parentGrid.Background = new SolidColorBrush(Colors.Transparent);

            /*UNCOMMENT these if you would like the grid to be centered*/
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

            //Image image = new Image();
            //image.Height = tilesize;
            //image.Width = tilesize;
            //BitmapImage bitmapImage = new BitmapImage();
            //bitmapImage.BeginInit();
            ////URI: "Uniform Resource Identifiers"
            //bitmapImage.UriSource = new Uri("/Images/grassMid.png", UriKind.Relative);
            //bitmapImage.DecodePixelHeight = tilesize;
            //bitmapImage.DecodePixelWidth = tilesize;
            //bitmapImage.EndInit();
            //image.Source = bitmapImage;
            //Grid.SetColumn(image, 1);
            //Grid.SetRow(image, 1);
            //parentGrid.Children.Add(image);
            
        BrushConverter bc = new BrushConverter();
            /*POPULATE ALL CELLS with a rect*/
            for (int i = 1; i <= cols; i++)
            {
                for (int j = 1; j <= rows; j++)
                {
                    //Set the grid space cell indicators in a checkerboard-like pattern
                    if (i % 2 == 0 && j % 2 == 0 )
                    {
                        Rectangle newRect = GetRectangle(0XFF, 0xF8, 0xF8, 0xF8);
                        Grid.SetColumn(newRect, i); Grid.SetRow(newRect, j); parentGrid.Children.Add(newRect);
                    }
                    else if (i % 2 == 1 && j % 2 == 0)
                    {
                        Rectangle newRect = GetRectangle(0X77, 0xA9, 0xA9, 0xA9);
                        Grid.SetColumn(newRect, i); Grid.SetRow(newRect, j); parentGrid.Children.Add(newRect);
                    }
                    else if (i % 2 == 1 && j % 2 == 1)
                    {
                        Rectangle newRect = GetRectangle(0XFF, 0xF8, 0xF8, 0xF8);
                        Grid.SetColumn(newRect, i); Grid.SetRow(newRect, j); parentGrid.Children.Add(newRect);
                    }
                    else if (i % 2 == 0 && j % 2 == 1)
                    {
                        Rectangle newRect = GetRectangle(0X77, 0xA9, 0xA9, 0xA9);
                        Grid.SetColumn(newRect, i); Grid.SetRow(newRect, j); parentGrid.Children.Add(newRect);
                    }
                }
            }
        }

        public Rectangle GetRectangle(byte transparency, byte red, byte green, byte blue)
        {
            return new Rectangle()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Fill = new SolidColorBrush(Color.FromArgb(transparency, red, green, blue))
            };
        }

        public void LoadAllTextures()
        {
            ResourceDictionary resourceDictionary = new ResourceDictionary();
            resourceDictionary.Source = new Uri("/Images/", UriKind.Relative);
        }

        public void HandleLeftClickOnGrid(Grid parentGrid, MouseButtonEventArgs args, int tilesize, int rows, int cols)
        {
            if (args.ClickCount == 1)
            {
                var point = args.GetPosition(parentGrid);
                //The variable row/col at the top/left that keeps things centered
                var paddingRowHeight = parentGrid.RowDefinitions[0].ActualHeight;
                var paddingColWidth = parentGrid.ColumnDefinitions[0].ActualWidth;
                int rowclicked = (int)(point.Y - paddingRowHeight) / tilesize;
                int colclicked = (int)(point.X - paddingColWidth) / tilesize;
                if (!(rowclicked > rows || rowclicked < 0 || colclicked > cols || colclicked < 0))
                {
                    System.Diagnostics.Debug.WriteLine("Detected a LEFT mouse click => X: " + point.X + ", Y: " + point.Y);
                    System.Diagnostics.Debug.WriteLine("Cell was => ROW: " + rowclicked + ", COL: " + colclicked);
                    System.Diagnostics.Debug.WriteLine("Padding was => ROW: " + paddingRowHeight + ", COL: " + paddingColWidth);
                    System.Diagnostics.Debug.WriteLine(" ");
                    Image image = new Image();
                    image.Height = tilesize;
                    image.Width = tilesize;
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    //URI: "Uniform Resource Identifiers"
                    bitmapImage.UriSource = new Uri("/Images/grassMid.png", UriKind.Relative);
                    bitmapImage.DecodePixelHeight = tilesize;
                    bitmapImage.DecodePixelWidth = tilesize;
                    bitmapImage.EndInit();
                    image.Source = bitmapImage;
                    Grid.SetColumn(image, colclicked + 1);
                    Grid.SetRow(image, rowclicked + 1);
                    parentGrid.Children.Add(image);
                }

            }
        }

        private void ImportTexture(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("IMPORT TEXTURE clicked");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            //openFileDialog.InitialDirectory = @"c:\";
            /*Uses the dictory last used*/
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == true)
            {
                CopyFileToProjectDirectory(openFileDialog);
            }

            //TODO After importing the texture, making the resource available to the program thereafter
            //ResourceManager?
        }

        private void CopyFileToProjectDirectory(OpenFileDialog dialog)
        {
            try
            {
                //Opens the dialog for the user to select and stores the result as a variable
                var dialogFilename = dialog.FileName;

                //Get the filename
                string filename = System.IO.Path.GetFileName(dialogFilename);
                System.Diagnostics.Debug.WriteLine("Got import filename of: " + filename);

                //Get the full file path only
                string filePathOnly = System.IO.Path.GetDirectoryName(dialogFilename);
                System.Diagnostics.Debug.WriteLine("Got import filePathOnly of: " + filePathOnly);

                //Get the destination directory path
                string thisProgramDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                string copyToFolder = "\\images\\";
                thisProgramDirectory += copyToFolder;
                System.Diagnostics.Debug.WriteLine("Got this program dir is: " + thisProgramDirectory);

                //Finally copy the file
                File.Copy(System.IO.Path.Combine(filePathOnly, filename), System.IO.Path.Combine(thisProgramDirectory, filename), true);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error copying texture to destination: " + e.ToString());
            }
        }
    }
}
