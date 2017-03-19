using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriceCollector.Properties;
using PriceCollector.Utils;
using PriceCollector.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PriceCollector.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PickSupermarketPage : ContentPage
    {
        private PickSupermarketViewModel _viewModel;
        public PickSupermarketPage(ILoginManager ilm)
        {
            InitializeComponent();
            _viewModel = new PickSupermarketViewModel(this, ilm);
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            await _viewModel.LoadAsync();
            base.OnAppearing();
        }

        private void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            var market = e.Item as Model.SupermarketsCompetitors;
            if (market == null)
                return;

            _viewModel.SetSupermarketToWillCollectedProdutcs(market);
        }
    }
}
