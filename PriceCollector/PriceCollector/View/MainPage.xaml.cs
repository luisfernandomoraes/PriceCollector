using System;
using System.Threading.Tasks;
using PriceCollector.ViewModel;
using Xamarin.Forms;
using Plugin.Toasts;
using System.Diagnostics;
using ZXing.Mobile;
using Xamarin.Forms.Xaml;
using System.Threading;
using Rg.Plugins.Popup.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PriceCollector.View
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel _mainPageViewModel;
        private readonly IToastNotificator _notificator;

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

        private async void OnStartScann(object sender, EventArgs evt)
        {
            try
            {
                var barcode = await _mainPageViewModel.StartBarCodeScannerAsync();
                if (string.IsNullOrEmpty(barcode))
                    return;

                var searchResultPage = new SearchResultPage(barcode);
                searchResultPage.Disappearing += SearchResultPage_Disappearing;
                await PopupNavigation.PushAsync(searchResultPage);
                
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
