using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PriceCollector.Annotations;
using PriceCollector.Model;

namespace PriceCollector.ViewModel
{
    public class MainPageViewModel:INotifyPropertyChanged
    {
        #region Fields

        private ObservableCollection<Product> _products;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion


        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set
            {
                if (Equals(value, _products)) return;
                _products = value;
                OnPropertyChanged();
            }
        }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}