using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriceCollector.Model;
using PriceCollector.ViewModel;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PriceCollector.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchResultEditPage : PopupPage
    {
        ViewModel.SearchResultEditViewModel  _viewModel;

        public SearchResultEditPage(ProductCollected product)
        {
            InitializeComponent();
            _viewModel = new ViewModel.SearchResultEditViewModel(product);
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            await _viewModel.LoadAsync();
            base.OnAppearing();
        }
    }
}