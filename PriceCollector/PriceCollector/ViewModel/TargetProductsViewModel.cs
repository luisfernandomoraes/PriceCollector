using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Toasts;
using PriceCollector.Annotations;
using PriceCollector.Api.WebAPI.Products;
using PriceCollector.Model;
using Xamarin.Forms;

namespace PriceCollector.ViewModel
{
    public class TargetProductsViewModel:INotifyPropertyChanged
    {
        #region Fields
        private IProductApi _productApi;
        private readonly IToastNotificator _notificator;
        private bool _isBusy;
        private ObservableCollection<Product> _products;

        #endregion

        #region Properties

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

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (Equals(value, _isBusy)) return;
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Ctor

        public TargetProductsViewModel()
        {
            _productApi = DependencyService.Get<IProductApi>();
            _notificator = DependencyService.Get<IToastNotificator>();
            _isBusy = false;
            Task.Run(LoadProducts);
        }

        #endregion

        #region Methods

        private async Task LoadProducts()
        {
            try
            {
                IsBusy = true;
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
                    IsBusy = false;
                }
            }
            catch (Exception e)
            {
                await _notificator.Notify(ToastNotificationType.Error, @"PriceCollector", "Houve um erro ao carregar os produtos", TimeSpan.FromSeconds(3));
                Debug.WriteLine(e);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task<string> StartBarCodeScannerAsync()
        {
            return string.Empty;
        }


        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}