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

            await _targetProductsViewModel.StartBarCodeScannerAsync(product);
            
        }
        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            await _targetProductsViewModel.RemoveBarcodeEventHandler();
        }

    }
}
