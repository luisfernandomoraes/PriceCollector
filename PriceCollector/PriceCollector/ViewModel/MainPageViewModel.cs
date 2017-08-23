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
using PriceCollector.Controls;
using PriceCollector.DB;
using PriceCollector.Model;
using PriceCollector.View;
using PriceCollector.ViewModel.Services;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using Scandit.BarcodePicker.Unified;
using Barcode = Rb.Forms.Barcode.Pcl.Barcode;

namespace PriceCollector.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged, IReloadDataViewModel
    {
        #region Fields

        private IProductApi _productApi;
        private ObservableCollection<ProductCollected> _products;
        private readonly IToastNotificator _notificator;
        public event PropertyChangedEventHandler PropertyChanged;
        private CancellationTokenSource _cancellationTokenSourceTask;
        private ScannerPage _scannerPage;
        private bool _isBusy;
        private bool _isEmpty;
        private INavigationService _navigation;
        private MainPage _mainPage;

        #endregion

        #region Commands
        public ICommand StartScannerCommand => new Command(StartScanner);

        private async void StartScanner(object obj)
        {
            await ScanditService.BarcodePicker.StartScanningAsync(true);
        }

        public ICommand SyncCollectedProductsCommand => new Command(SyncCollectedProducts);

        private async void SyncCollectedProducts(object obj)
        {
            try
            {
                await _productApi.PostCollectedProducts(String.Empty, DBContext.ProductCollectedDataBase.GetItems().ToList());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await _notificator.Notify(ToastNotificationType.Error, ":(", "Ocorreu um erro no processo de syncronização, porfavor tente mais tarde.", TimeSpan.FromSeconds(3));
            }
        }

        #endregion


        public MainPageViewModel(MainPage mainPage)
        {
            _productApi = DependencyService.Get<IProductApi>();
            _notificator = DependencyService.Get<IToastNotificator>();
            _isBusy = false;
            _products = new ObservableCollection<ProductCollected>();
            _mainPage = mainPage;
            Task.Run(async () =>
            {
                await LoadData();
                _scannerPage = ScannerPageControl.Instance.CreateScannerPage();
            });
            ScanditService.BarcodePicker.DidScan += BarcodePickerOnDidScan;
            MessagingCenter.Subscribe<SearchResultViewModel>(this, "LoadData", async (sender) =>
             {
                 await LoadData();
             });
        }
        private async void BarcodePickerOnDidScan(ScanSession session)
        {
            await ScanditService.BarcodePicker.StopScanningAsync();
            var recognizedCode = session.NewlyRecognizedCodes.LastOrDefault()?.Data;
            Device.BeginInvokeOnMainThread(async () =>
            {

                var searchResultPage = new SearchResultPage(recognizedCode);
                if (PopupNavigation.PopupStack.Count < 1)
                    await PopupNavigation.PushAsync(searchResultPage);
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
                    bool haveImage = await _productApi.HasImage(urlImage);
                    if (haveImage)
                        p.ImageProduct = "NoImagemTarget.png";
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
