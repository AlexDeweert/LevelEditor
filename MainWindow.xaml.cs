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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ChildWindow gridWindow = new ChildWindow();
            gridWindow.Owner = this;

            //This won't work....the child window must subscribe to the
            //parent window - alternatively, think about embedding a child
            //window instantiated into a parent window somehow
            //private void OnActivated(object sender, EventArgs eventArgs)
            //{
            //    owned.Owner = this;
            //    Activated -= OnActivated;
            //}
            gridWindow.Show();
        }
    }
}
