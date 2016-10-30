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
using Xamarin.Forms;
using ZXing.Mobile;

namespace PriceCollector.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        #region Fields

        private IProductApi _productApi;
        private ObservableCollection<ProductCollected> _products;
        private readonly IToastNotificator _notificator;
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _isBusy;
        private readonly ZXing.Mobile.MobileBarcodeScanner _scanner;
        private bool _isEmpty;

        #endregion

        #region Commands


        #endregion


        public MainPageViewModel()
        {
            _productApi = DependencyService.Get<IProductApi>();
            _notificator = DependencyService.Get<IToastNotificator>();
            _isBusy = false;
            _scanner = new MobileBarcodeScanner();
            _products = new ObservableCollection<ProductCollected>();
            Task.Run(LoadProducts);
        }

        public async Task LoadProducts()
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

                    var qrcode = resultQrCode.Text;
                    _scanner.Cancel();
                    return qrcode;
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
