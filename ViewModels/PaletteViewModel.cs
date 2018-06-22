using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LevelEditor.ViewModels
{
    public class PaletteViewModel
    {
        private ExtendedObservableCollection<Image> _imageList = new ExtendedObservableCollection<Image>();
        //public event PropertyChangedEventHandler PropertyChanged;
        //public ObservableCollection<Image> ImageList
        //{
        //    get
        //    {
        //        return this._imageList;
        //    }
        //    set
        //    {
        //        if (value != this._imageList)
        //        {
        //            this._imageList = value;
        //            foreach (Image i in this._imageList)
        //            {
        //                System.Diagnostics.Debug.WriteLine("ImageList Contents: " + i.Source.ToString());
        //            }
        //            OnPropertyChanged("ImageListChanged");
        //        }
        //    }
            
        //}
        //public PaletteViewModel() {/*...*/}
        //protected void OnPropertyChanged(string _propertyName)
        //{
        //    PropertyChangedEventHandler handler = PropertyChanged;
        //    if (handler != null)
        //    {
        //        System.Diagnostics.Debug.WriteLine("PROPERTY CHANGED: " + _propertyName);
        //        handler(this, new PropertyChangedEventArgs(_propertyName));
        //    }
        //}
    }

    public sealed class ExtendedObservableCollection<T> : ObservableCollection<T>
        where T : INotifyPropertyChanged
    {
        public ExtendedObservableCollection()
        {
            CollectionChanged += FullObservableCollectionChanged;
        }

        public ExtendedObservableCollection( IEnumerable<T> pItems ) : this()
        {
            foreach (var item in pItems)
            {
                this.Add(item);
            }
        }

        private void FullObservableCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Object item in e.NewItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged += ItemPropertyChanged;
                }
            }
            if (e.OldItems != null)
            {
                foreach (Object item in e.OldItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged -= ItemPropertyChanged;
                }
            }
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, sender, sender, IndexOf((T)sender));
            OnCollectionChanged(args);
        }
    }
}
