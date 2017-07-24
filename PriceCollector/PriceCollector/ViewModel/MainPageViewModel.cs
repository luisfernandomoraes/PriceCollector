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
using Rb.Forms.Barcode.Pcl;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;

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
            try
            {
                _scannerPage = ScannerPageControl.Instance.CreateScannerPage();
                bool isAlreadyRead = false;
                MessagingCenter.Subscribe<ScannerViewModel, Barcode>(this, "BarcodeChanged", (arg1, arg2) =>
                {
                    if (!isAlreadyRead)
                    {
                        isAlreadyRead = true;
                        _cancellationTokenSourceTask.Cancel();
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            if (_scannerPage.IsEnabled)
                            {
                                await _mainPage.Navigation.PopAsync();
                                OnBarcodeChanged(arg2);
                            }
                        });
                    }
                });

                await _mainPage.Navigation.PushAsync(_scannerPage);

                await StartTimeout();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await _notificator.Notify(ToastNotificationType.Error, ":(",
                    "Ocorreu um erro ao inicializar o leitor de códigos de barra.", TimeSpan.FromSeconds(3));
                //throw;
            }
        }

        private async void OnBarcodeChanged(Barcode barcodeResult)
        {
            try
            {

                var barcode = barcodeResult.Result;

                if (string.IsNullOrEmpty(barcode))
                {
                    await _notificator.Notify(ToastNotificationType.Error, nameof(PriceCollector), "Ocorreu um erro ao ralizar o scanneamento.", TimeSpan.FromSeconds(3));

                }
                else
                {
                    _cancellationTokenSourceTask.Cancel();

                    Device.BeginInvokeOnMainThread(async () =>
                    {

                        var searchResultPage = new SearchResultPage(barcode);
                        await PopupNavigation.PushAsync(searchResultPage);
                    });
                }

            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
                await _notificator.Notify(ToastNotificationType.Error, ":(", "Ocorreu um erro ao realizar o scanneamento",
                    TimeSpan.FromSeconds(3));
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

            MessagingCenter.Subscribe<SearchResultViewModel>(this, "LoadData", async (sender) =>
             {
                 await LoadData();
             });
        }
        private async Task StartTimeout()
        {
            _cancellationTokenSourceTask = new CancellationTokenSource();

            var cancellationToken = _cancellationTokenSourceTask.Token;
            await Task.Factory.StartNew(async () =>
            {
                // Were we already canceled?
                cancellationToken.ThrowIfCancellationRequested();

                await Task.Delay(TimeSpan.FromSeconds(15), cancellationToken); // Timeout

                if (!cancellationToken.IsCancellationRequested && _scannerPage.BarcodeScannerPage.IsEnabled)
                {
                    //BarcodeScannerPageOnBarcodeChanged(new BarcodeScanner { Barcode = null }, null); //Not found or cant read the barcode.
                }
            }, _cancellationTokenSourceTask.Token);
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
