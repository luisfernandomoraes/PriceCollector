using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriceCollector.Model;
using PriceCollector.ViewModel;
using PriceCollector.ViewModel.Services;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace PriceCollector.View
{
    public partial class SearchResultPage: PopupPage
    {
        private ProductCollected product;
        private SearchResultViewModel _searchResultViewModel;


        /// <summary>
        /// Ctor utilizado para cadastrar o produto coletado.
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="mainPageReloadDataViewModel"></param>
        public SearchResultPage(string barcode)
        {
            InitializeComponent();
            _searchResultViewModel = new SearchResultViewModel(barcode);
            BindingContext = _searchResultViewModel;
        }

        protected override async void OnAppearing()
        {
            await _searchResultViewModel.LoadAsync();
            PriceEntry.Focus();
            base.OnAppearing();
        }
    }
}
