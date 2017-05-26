using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Plugin.Toasts;
using PriceCollector.ViewModel;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace PriceCollector.View
{
    public partial class TargetProductsPage : ContentPage
    {
        private TargetProductsViewModel _targetProductsViewModel;
        private readonly IToastNotificator _notificator;

        public TargetProductsPage()
        {
            InitializeComponent();
            _targetProductsViewModel = new TargetProductsViewModel();
            BindingContext = _targetProductsViewModel;
            _notificator = DependencyService.Get<IToastNotificator>();

        }

        private async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            var product = e.Item as Model.Product;
            if (product == null)
                return;

            _notificator.HideAll();

            list.SelectedItem = null;
            // Primeira verificação,

            // Verificamos se o codigo de barras bate com o codigo de barras informado.
            var barcode = await _targetProductsViewModel.StartBarCodeScannerAsync();
            if (barcode != product.BarCode)
            {
                await _notificator.Notify(ToastNotificationType.Error, Utils.Constants.AppName,
                        $"O código de barras lido {barcode}, não confere com o produto {product.Name}, por favor tente novamente", TimeSpan.FromSeconds(5));
            }
            //Caso sim, prosseguimos com a coleta de preço.
            var searchResultPage = new SearchResultPage(barcode,_targetProductsViewModel);
            await PopupNavigation.PushAsync(searchResultPage);
        }
        
    }
}
