using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Toasts;
using PriceCollector.Annotations;
using PriceCollector.Api.WebAPI.Products;
using PriceCollector.Api.WebAPI.Responses;
using PriceCollector.DB;
using PriceCollector.Model;
using PriceCollector.ViewModel.Interfaces;
using Xamarin.Forms;

namespace PriceCollector.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged,IReloadDataViewModel
    {
        #region Fields

        private IProductApi _productApi;
        private ObservableCollection<ProductCollected> _products;
        private readonly IToastNotificator _notificator;
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _isBusy;
        private bool _isEmpty;

        #endregion

        #region Commands


        #endregion


        public MainPageViewModel()
        {
            _productApi = DependencyService.Get<IProductApi>();
            _notificator = DependencyService.Get<IToastNotificator>();
            _isBusy = false;
            _products = new ObservableCollection<ProductCollected>();
            Task.Run(LoadData);
            MessagingCenter.Subscribe<SearchResultViewModel>(this,"LoadData", async (sender) =>
            {
               await LoadData();
            });
        }

        public async Task LoadData()
        {
            try
            {
                IsBusy = true;

                var products = DBContext.ProductCollectedDataBase.GetItems();
                var productCollecteds = products as ProductCollected[] ?? products.ToArray();
                if (!productCollecteds.Any())
                {
                    IsEmpty = true;
                    return;
                }

                foreach (var p in productCollecteds)
                {
                    var urlImage = $@"http://imagens.scannprice.com.br/Produtos/{p.BarCode}.jpg";
                    if (await _productApi.HasImage(urlImage))
                        p.ImageProduct = "NoImagemTarge.png";
                    else
                        p.ImageProduct = urlImage;
                }

                Products = new ObservableCollection<ProductCollected>(productCollecteds);
                IsEmpty = false;
                IsBusy = false;
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





        #region Properties
        public bool IsEmpty
        {
            get { return _isEmpty; }
            set
            {
                if (value == _isEmpty) return;
                _isEmpty = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ProductCollected> Products
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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<string> StartBarCodeScannerAsync()
        {
            return string.Empty;
        }

        public async Task AddProductCollected(ProductCollected productCollected)
        {
            var urlImage = $@"http://imagens.scannprice.com.br/Produtos/{productCollected.BarCode}.jpg";
            if (await _productApi.HasImage(urlImage))
                productCollected.ImageProduct = "NoImagemTarge.png";
            else
                productCollected.ImageProduct = urlImage;

            Products.Add(productCollected);
            IsEmpty = false;
        }
    }
}
