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
using ZXing.Mobile;

namespace PriceCollector.ViewModel
{
    public class TargetProductsViewModel:INotifyPropertyChanged
    {
        #region Fields
        private IProductApi _productApi;
        private readonly IToastNotificator _notificator;
        private bool _isBusy;
        private ObservableCollection<Product> _products;
        private readonly ZXing.Mobile.MobileBarcodeScanner _scanner;

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
            _scanner = new MobileBarcodeScanner();
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
            try
            {
                const int timeout = 1000 * 15;

                var task = _scanner.Scan();


                // Criação do token de cancelamento.
                var tokenSource = new CancellationTokenSource();
                CancellationToken ct = tokenSource.Token;

                await Task.Factory.StartNew(async () =>
                {

                    // Were we already canceled?
                    ct.ThrowIfCancellationRequested();

                    //Enquanto a task do scanner não tiver sido completada...
                    while (!task.IsCompleted)
                    {
                        // Poll on this property if you have to do
                        // other cleanup before throwing.
                        if (ct.IsCancellationRequested)
                        {
                            // Clean up here, then...
                            ct.ThrowIfCancellationRequested();
                        }
                        await Task.Delay(TimeSpan.FromSeconds(2));
                        _scanner.AutoFocus();
                    }
                }, tokenSource.Token); // Pass same token to StartNew.




                if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
                {
                    tokenSource.Cancel();
                    var resultQrCode = task.Result;
                    if (resultQrCode == null)
                    {
                        await _notificator.Notify(ToastNotificationType.Error, nameof(PriceCollector), "Ocorreu um erro ao ralizar o scanneamento.", TimeSpan.FromSeconds(3));

                        return string.Empty;
                    }

                    var barcode = resultQrCode.Text;
                    _scanner.Cancel();
                    return barcode;
                }

                _scanner.Cancel();
                await _notificator.Notify(ToastNotificationType.Error, nameof(PriceCollector), "Ocorreu um erro ao ralizar o scanneamento.", TimeSpan.FromSeconds(3));
                return string.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return string.Empty;
            }
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