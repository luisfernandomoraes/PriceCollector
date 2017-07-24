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
            // First check,

            
            // Check if bar code's collected is equal with the passed bar code. 
            var barcode = await _targetProductsViewModel.StartBarCodeScannerAsync();
            if (barcode != product.BarCode)
            {
                await _notificator.Notify(ToastNotificationType.Error, Utils.Constants.AppName,
                        $"O código de barras lido {barcode}, não confere com o produto {product.Name}, por favor tente novamente", TimeSpan.FromSeconds(5));
            }

            //If yes, continue with price collect.
            var searchResultPage = new SearchResultPage(barcode);
            await PopupNavigation.PushAsync(searchResultPage);
        }
        
    }
}
