using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriceCollector.ViewModel;
using PriceCollector.ViewModel.Interfaces;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace PriceCollector.View
{
    public partial class SearchResultPage: PopupPage
    {
        public SearchResultViewModel SearchResultViewModel { get; }

        public SearchResultPage(string barcode, IReloadDataViewModel mainPageReloadDataViewModel)
        {
            InitializeComponent();
            SearchResultViewModel = new SearchResultViewModel(barcode,mainPageReloadDataViewModel);
            BindingContext = SearchResultViewModel;
        }
    }
}
