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
        private CancellationTokenSource _cancellationTokenSourceTask;

        public MainPage()
        {
            _mainPageViewModel = new MainPageViewModel(this);
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
            list.SelectedItem = null;
            var searchResultPage = new SearchResultEditPage(product);
            await PopupNavigation.PushAsync(searchResultPage);

        }



    }
}
