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
            var product = args.Item as Model.Product;
            if (product == null)
                return;

            _notificator.HideAll();

            list.SelectedItem = null;
            // Primeira verificação,

            // Verificamos se o codigo de barras bate com o codigo de barras informado.
            var barcode = await _mainPageViewModel.StartBarCodeScannerAsync();

            //Caso sim, prosseguimos com a coleta de preço.
            if (barcode == product.BarCode)
            {
                await _notificator.Notify(ToastNotificationType.Success, nameof(PriceCollector), $"Produto {product.Name} coletado", TimeSpan.FromSeconds(3));
            }
            else if (!string.IsNullOrEmpty(barcode))
            {
                await _notificator.Notify(ToastNotificationType.Warning, nameof(PriceCollector), "O código de barras não bate com o produto selecionado.", TimeSpan.FromSeconds(3));
            }

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

                        var searchResultPage = new SearchResultPage(barcode);
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
