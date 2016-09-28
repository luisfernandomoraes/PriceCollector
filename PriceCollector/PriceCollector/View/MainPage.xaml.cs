using System;
using System.Threading.Tasks;
using PriceCollector.ViewModel;
using Xamarin.Forms;
using Plugin.Toasts;
using System.Diagnostics;
using ZXing.Mobile;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PriceCollector.View
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel _mainPageViewModel;
        private readonly IToastNotificator _notificator;
        private readonly ZXing.Mobile.MobileBarcodeScanner _scanner;
                
        public MainPage()
        {
            _mainPageViewModel = new MainPageViewModel();
            InitializeComponent();
            BindingContext = _mainPageViewModel;
            _notificator = DependencyService.Get<IToastNotificator>();
            _scanner = new MobileBarcodeScanner();
            
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
            var barcode = await StartBarCodeScannerAsync();

            //Caso sim, prosseguimos com a coleta de preço.
            if (barcode == product.BarCode)
            {
                await _notificator.Notify(ToastNotificationType.Success,nameof(PriceCollector),$"Produto {product.Name} coletado",TimeSpan.FromSeconds(3));
            }
            else
            {
                await _notificator.Notify(ToastNotificationType.Warning, nameof(PriceCollector), "O código de barras não bate com o produto selecionado.", TimeSpan.FromSeconds(3));
            }

        }

        private async void OnStartScann(object sender, EventArgs evt)
        {

        }

        private async Task<string> StartBarCodeScannerAsync()
        {
            try
            {

                string qrcode;
                const int timeout = 1000 * 15;

                var task = _scanner.Scan();


                if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
                {
                    var resultQrCode = task.Result;
                    if (resultQrCode == null)
                    {
                        await _notificator.Notify(ToastNotificationType.Error, nameof(PriceCollector), "Ocorreu um erro ao ralizar o scanneamento.", TimeSpan.FromSeconds(3));

                        return string.Empty;
                    }
                    else
                    {
                        qrcode = resultQrCode.Text;
                        _scanner.Cancel();
                        return qrcode;
                    }
                }
                else
                {
                    _scanner.Cancel();
                    await _notificator.Notify(ToastNotificationType.Error, nameof(PriceCollector), "Ocorreu um erro ao ralizar o scanneamento.", TimeSpan.FromSeconds(3));
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return string.Empty;
            }
        }
          
    }
}
