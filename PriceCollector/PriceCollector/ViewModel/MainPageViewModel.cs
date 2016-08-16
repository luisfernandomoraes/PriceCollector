using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Toasts;
using PriceCollector.Annotations;
using PriceCollector.Api.WebAPI.Products;
using PriceCollector.Model;
using Xamarin.Forms;

namespace PriceCollector.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        #region Fields

        private IProductApi _productApi;
        private ObservableCollection<Product> _products;
        private readonly IToastNotificator _notificator;
        public event PropertyChangedEventHandler PropertyChanged;


        #endregion


        public MainPageViewModel()
        {
            _productApi = DependencyService.Get<IProductApi>();
            _notificator = DependencyService.Get<IToastNotificator>();
            Task.Run(LoadProducts);
        }

        private async Task LoadProducts()
        {
            try
            {
                var result = await _productApi.GetProductsToCollect("http://www.acats.scannprice.srv.br/api/");
                if (result.Success)
                {
                    
                    foreach (var p in result.CollectionResult)
                    {
                        var urlImage = $@"http://imagens.scannprice.com.br/Produtos/{p.BarCode}.jpg";
                        if (await _productApi.HasImage(urlImage))
                            p.ImageProduct = "NoImagemTarge.png";
                        else
                            p.ImageProduct = urlImage;
                    }

                    Products = new ObservableCollection<Product>(result.CollectionResult);

                }
            }
            catch (Exception e)
            {
                await _notificator.Notify(ToastNotificationType.Error, @"PriceCollector","Houve um erro ao carregar os produtos", TimeSpan.FromSeconds(3));
                Debug.WriteLine(e);
            }
        }


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
