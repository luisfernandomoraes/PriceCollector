using System;
using System.Threading.Tasks;
using PriceCollector.ViewModel;
using Xamarin.Forms;
using Plugin.Toasts;
using System.Diagnostics;
using Xamarin.Forms.Xaml;
using System.Threading;
using PriceCollector.Controls;
using Rb.Forms.Barcode.Pcl;
using Rg.Plugins.Popup.Services;

namespace PriceCollector.View
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel _mainPageViewModel;
        private readonly IToastNotificator _notificator;
        private ScannerPage _scannerPage;
        private bool _isToShowScanner;
        private CancellationTokenSource _cancellationTokenSourceTask;

        public MainPage()
        {
            _mainPageViewModel = new MainPageViewModel();
            InitializeComponent();
            BindingContext = _mainPageViewModel;
            _notificator = DependencyService.Get<IToastNotificator>();

        }


        private async void OnItemSelected(object sender, ItemTappedEventArgs args)
        {
            var product = args.Item as Model.ProductCollected;
            if (product == null)
            {
                list.SelectedItem = null;
                return;
            }
            SearchResultPage searchResultPage = new SearchResultPage(product);
            await PopupNavigation.PushAsync(searchResultPage);
        }


        private async Task StartScannAsync()
        {
            try
            {
                _scannerPage = ScannerPageControl.Instance.CreateScannerPage();
                _scannerPage.BarcodeScannerPage.BarcodeChanged += BarcodeScannerPageOnBarcodeChanged;
                await Navigation.PushAsync(_scannerPage);

                await StartTimeout();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
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
                    BarcodeScannerPageOnBarcodeChanged(new BarcodeScanner { Barcode = null }, null); //Not found or cant read the barcode.
                }
            }, _cancellationTokenSourceTask.Token);
        }

        private async void BarcodeScannerPageOnBarcodeChanged(object sender, BarcodeEventArgs e)
        {

            try
            {

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                });

                var barcode = string.Empty;
                var barcodeScanner = (BarcodeScanner)sender;

                if (barcodeScanner.Barcode == null)
                {
                    await _notificator.Notify(ToastNotificationType.Error, nameof(PriceCollector), "Ocorreu um erro ao ralizar o scanneamento.", TimeSpan.FromSeconds(3));

                }
                else
                {
                    _cancellationTokenSourceTask.Cancel();

                    barcode = barcodeScanner.Barcode.Result;

                    Device.BeginInvokeOnMainThread(async () =>
                    {

                        var searchResultPage = new SearchResultPage(barcode,_mainPageViewModel);
                        await PopupNavigation.PushAsync(searchResultPage);
                    });
                }

            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }

        }

        private async void OnStartScann(object sender, EventArgs evt)
        {
            try
            {
                await StartScannAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async void SearchResultPage_Disappearing(object sender, EventArgs e)
        {
            var searchResultPage = sender as SearchResultPage;
            if (searchResultPage?.SearchResultViewModel.ProductCollected != null)
                await _mainPageViewModel.AddProductCollected(searchResultPage?.SearchResultViewModel.ProductCollected);
        }
    }
}
