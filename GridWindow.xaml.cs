using LevelEditor.ViewModels;
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
        private PaletteViewModel _paletteViewModel = new PaletteViewModel();
        public int LevelWidth { set; get; }
        public int LevelHeight { set; get; }
        public int PaletteWidth { set; get; }
        public int PaletteHeight { set; get; }
        public int TileSize { set; get; }
        public int PaletteSize { set; get; }
        public Dictionary<int, Uri> TextureDictionary = new Dictionary<int, Uri>();
        public int BrushTipID_paletteRow { set; get; } //TODO Make left click in palette update which brush is referenced by functions calling palette Dictionary
        public int BrushTipID_paletteCol { set; get; }
        public int[,] LevelGridCompanionArray { set; get; }

        //TODO test image merging
        //output a 1d array with each index as the x-coordinate of the start of the texture on
        //the output merged image
        Merge imageMerge = new Merge();

        public GridWindow()
        {
            InitializeComponent();
            TileSize = 32;
            PaletteSize = 64;
            LevelWidth = 5;
            LevelHeight = 5;
            PaletteWidth = 1;
            PaletteHeight = 10;
            
            this._paletteViewModel = new PaletteViewModel();
            CreateGrid(LevelWidth,LevelHeight, levelGrid, TileSize);
            InitializePalette(PaletteWidth, PaletteHeight, dynamicPalette, PaletteSize);
            LevelGridCompanionArray = new int[LevelWidth,LevelHeight];
        }

        /*CreateGrid
         * Pass in the number of rows and columns
         * you would like to have in your grid level
         */
        public void CreateGrid( int cols, int rows, Grid levelGrid, int tilesize )
        {
            MouseButtonEventHandler leftclick = new MouseButtonEventHandler((e, args) => {
                HandleLeftClickOnGrid(levelGrid, args, tilesize, rows, cols);
            });

            levelGrid.PreviewMouseLeftButtonDown += leftclick;

            /*UNCOMMENT these if you would like the grid to be centered*/
            ColumnDefinition c0 = new ColumnDefinition();
            c0.Width = new GridLength(1, GridUnitType.Star);
            levelGrid.ColumnDefinitions.Add(c0);
            for (int i = 0; i < cols; i++)
            {
                ColumnDefinition tempcol = new ColumnDefinition();
                tempcol.Width = new GridLength(tilesize, GridUnitType.Pixel);
                levelGrid.ColumnDefinitions.Add(tempcol);
            }
            ColumnDefinition c2 = new ColumnDefinition();
            c2.Width = new GridLength(1, GridUnitType.Star);
            levelGrid.ColumnDefinitions.Add(c2);

            RowDefinition r0 = new RowDefinition();
            r0.Height = new GridLength(1, GridUnitType.Star);
            levelGrid.RowDefinitions.Add(r0);
            for (int i = 0; i < rows; i++)
            {
                RowDefinition temprow = new RowDefinition();
                temprow.Height = new GridLength(tilesize, GridUnitType.Pixel);
                levelGrid.RowDefinitions.Add(temprow);
            }
            RowDefinition r2 = new RowDefinition();
            r2.Height = new GridLength(1, GridUnitType.Star);
            levelGrid.RowDefinitions.Add(r2);
            
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
                        Grid.SetColumn(newRect, i); Grid.SetRow(newRect, j); levelGrid.Children.Add(newRect);
                    }
                    else if (i % 2 == 1 && j % 2 == 0)
                    {
                        Rectangle newRect = GetRectangle(0X77, 0xA9, 0xA9, 0xA9);
                        Grid.SetColumn(newRect, i); Grid.SetRow(newRect, j); levelGrid.Children.Add(newRect);
                    }
                    else if (i % 2 == 1 && j % 2 == 1)
                    {
                        Rectangle newRect = GetRectangle(0XFF, 0xF8, 0xF8, 0xF8);
                        Grid.SetColumn(newRect, i); Grid.SetRow(newRect, j); levelGrid.Children.Add(newRect);
                    }
                    else if (i % 2 == 0 && j % 2 == 1)
                    {
                        Rectangle newRect = GetRectangle(0X77, 0xA9, 0xA9, 0xA9);
                        Grid.SetColumn(newRect, i); Grid.SetRow(newRect, j); levelGrid.Children.Add(newRect);
                    }
                }
            }
        }

        public void InitializePalette(int cols, int rows, Grid paletteGrid, int tilesize)
        {
            MouseButtonEventHandler leftclick = new MouseButtonEventHandler((e, args) => {
                HandleLeftClickOnPaletteGrid(paletteGrid, args, tilesize, rows, cols);
            });

            paletteGrid.PreviewMouseLeftButtonDown += leftclick;
            
            //Init one and only column
            ColumnDefinition c0 = new ColumnDefinition();
            c0.Width = new GridLength(1, GridUnitType.Star);
            paletteGrid.ColumnDefinitions.Add(c0);
            
            //Init number of rows, should be one to start in a new project
            //but for a loaded project it might have to be more
            for (int i = 0; i < rows; i++)
            {
                RowDefinition temprow = new RowDefinition();
                temprow.Height = new GridLength(tilesize, GridUnitType.Pixel);
                paletteGrid.RowDefinitions.Add(temprow);
            }
            
            /*Now we must fill each grid cell with the appropriate tile
             in a new project this will be empty until the user import new textures.
             In a loaded project we will have to iterate through and add them based on
             some kind of saved state values. For now, we deal with new projects only
             */
            BrushConverter bc = new BrushConverter();
            /*POPULATE ALL CELLS with a rect*/
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    //Set the grid space cell indicators in a checkerboard-like pattern
                    if (i % 2 == 0 && j % 2 == 0)
                    {
                        Rectangle newRect = GetRectangle(0XFF, 0xF8, 0xF8, 0xF8);
                        Grid.SetColumn(newRect, i); Grid.SetRow(newRect, j); paletteGrid.Children.Add(newRect);
                    }
                    else if (i % 2 == 1 && j % 2 == 0)
                    {
                        Rectangle newRect = GetRectangle(0X77, 0xA9, 0xA9, 0xA9);
                        Grid.SetColumn(newRect, i); Grid.SetRow(newRect, j); paletteGrid.Children.Add(newRect);
                    }
                    else if (i % 2 == 1 && j % 2 == 1)
                    {
                        Rectangle newRect = GetRectangle(0XFF, 0xF8, 0xF8, 0xF8);
                        Grid.SetColumn(newRect, i); Grid.SetRow(newRect, j); paletteGrid.Children.Add(newRect);
                    }
                    else if (i % 2 == 0 && j % 2 == 1)
                    {
                        Rectangle newRect = GetRectangle(0X77, 0xA9, 0xA9, 0xA9);
                        Grid.SetColumn(newRect, i); Grid.SetRow(newRect, j); paletteGrid.Children.Add(newRect);
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

        public void HandleLeftClickOnGrid(Grid levelGrid, MouseButtonEventArgs args, int tilesize, int rows, int cols)
        {
            if (args.ClickCount == 1)
            {
                var point = args.GetPosition(levelGrid);
                //The variable row/col at the top/left that keeps things centered
                var paddingRowHeight = levelGrid.RowDefinitions[0].ActualHeight;
                var paddingColWidth = levelGrid.ColumnDefinitions[0].ActualWidth;
                int rowclicked = (int)(point.Y - paddingRowHeight) / tilesize;
                int colclicked = (int)(point.X - paddingColWidth) / tilesize;
                if (!(rowclicked > rows || rowclicked < 0 || colclicked > cols || colclicked < 0))
                {
                    System.Diagnostics.Debug.WriteLine("Detected a LEFT mouse click => X: " + point.X + ", Y: " + point.Y);
                    System.Diagnostics.Debug.WriteLine("Cell was => ROW: " + rowclicked + ", COL: " + colclicked);
                    System.Diagnostics.Debug.WriteLine("Padding was => ROW: " + paddingRowHeight + ", COL: " + paddingColWidth);
                    System.Diagnostics.Debug.WriteLine(" ");
                    Image image = GetCurrrentBrushTip();
                    Grid.SetColumn(image, colclicked + 1);
                    Grid.SetRow(image, rowclicked + 1);
                    levelGrid.Children.Add(image);
                }

                //TODO Make the level editor correspond to an output fi
            }
        }

        /* When a texture is imported, it is added both to the paletteGrid, and the paletteDictionary simultaneously
         * The coordinate pair, ie (0,0) is the first texture, (0,1..x) is the 1st to xth texture etc.
         * 
         * The textureGrid is drawn based on the textureDictionary contents. (textureDictionary handled with import texture)
         * 
         * When clicked, we have a co-ordinate. We use that co-ordinate to reference the value present in the key-value
         dictionary ie; Dictionary<coord, image>. We then set the "OnBrushTip" value to the image referenced by the coordinate*/
        public void HandleLeftClickOnPaletteGrid(Grid paletteGrid, MouseButtonEventArgs args, int tilesize, int rows, int cols)
        {
            if (args.ClickCount == 1)
            {
                var point = args.GetPosition(paletteGrid);
                int rowclicked = (int)(point.Y) / tilesize;
                int colclicked = (int)(point.X) / tilesize;
                if (!(rowclicked > rows || rowclicked < 0 || colclicked > cols || colclicked < 0))
                {
                    System.Diagnostics.Debug.WriteLine("PALETTE GRID: Cell Clicked was => ROW: " + rowclicked + ", COL: " + colclicked);
                    System.Diagnostics.Debug.WriteLine(" ");
                    UpdateBrushTip(rowclicked, colclicked);
                }
            }
        }

        public void UpdatePaletteGrid(Grid paletteGrid)
        {
            Print("UpdatePaletteGrid() => Updating Palette Grid by creating new image and adding to TextureDictionary list");
            try
            {
                Image image = GetNewImage(PaletteSize, TextureDictionary.ElementAt(TextureDictionary.Count - 1).Value);
                Grid.SetColumn(image, 0);
                Grid.SetRow(image, TextureDictionary.Count-1);
                paletteGrid.Children.Add(image);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }

        private void ImportTexture(object sender, RoutedEventArgs e)
        {
            string textureUri = null;
            System.Diagnostics.Debug.WriteLine("IMPORT TEXTURE clicked");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            //openFileDialog.InitialDirectory = @"c:\";
            /*Uses the dictory last used*/
            openFileDialog.RestoreDirectory = true;
            if ( (bool)openFileDialog.ShowDialog() )
            {
                textureUri = CopyFileToProjectDirectory(openFileDialog);
            }

            if (textureUri != null)
            {
                Print("textureUri of imported image is now in: " + textureUri);
            }

            /*Not sure if this works to add an _newImage to the Palette scrollview as a button in the UI*/
            TextureDictionary.Add(TextureDictionary.Count, new Uri(textureUri, UriKind.Absolute));

            //UPDATE paletteGrid
            UpdatePaletteGrid(dynamicPalette);
        }

        public Image GetCurrrentBrushTip()
        {
            return GetNewImage(TileSize, TextureDictionary.ElementAt(BrushTipID_paletteRow).Value);
        }

        public void UpdateBrushTip(int row, int col)
        {
            BrushTipID_paletteRow = row;
            BrushTipID_paletteCol = col;
        }

        public Image GetNewImage(int size, Uri uriSource)
        {
            Image image = new Image();
            image.Height = size;
            image.Width = size;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = uriSource;
            bitmapImage.DecodePixelHeight = size;
            bitmapImage.DecodePixelWidth = size;
            bitmapImage.EndInit();
            image.Source = bitmapImage;
            return image;
        }

        private string CopyFileToProjectDirectory(OpenFileDialog dialog)
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
                string copyToFolder = "\\Images\\";
                thisProgramDirectory += copyToFolder;
                System.Diagnostics.Debug.WriteLine("Got this program dir is: " + thisProgramDirectory);

                //Finally copy the file
                File.Copy(System.IO.Path.Combine(filePathOnly, filename), System.IO.Path.Combine(thisProgramDirectory, filename), true);
                return thisProgramDirectory + filename;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error copying texture to destination: " + e.ToString());
                return null;
            }
        }

        private void Print(string str)
        {
            System.Diagnostics.Debug.WriteLine(str);
        }
    }
}
