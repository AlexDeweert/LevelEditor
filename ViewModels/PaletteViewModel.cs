using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LevelEditor.ViewModels
{
    public class PaletteViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Image> _imageList = new ObservableCollection<Image>();

        public ObservableCollection<Image> ImageList
        {
            get
            {
                return this._imageList;
            }
            set
            {
                System.Diagnostics.Debug.WriteLine("Setting something in image list...");
                this._imageList = value;
                foreach (Image i in this._imageList)
                {
                    System.Diagnostics.Debug.WriteLine("ImageList Contents: " + i.Source.ToString());
                }
                OnPropertyChanged("ImageListChanged");
            }
        }
        public PaletteViewModel() {/*...*/}
        protected void OnPropertyChanged(string _propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                System.Diagnostics.Debug.WriteLine("PROPERTY CHANGED: " + _propertyName);
                handler(this, new PropertyChangedEventArgs(_propertyName));
            }
        }
    }
}
