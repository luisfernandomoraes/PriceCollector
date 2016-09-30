using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriceCollector.ViewModel;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace PriceCollector.View
{
    public partial class SearchResultPage: PopupPage
    {
        private SearchResultViewModel _searchResultViewModel;
        public SearchResultPage(string barcode)
        {
            InitializeComponent();
            _searchResultViewModel = new SearchResultViewModel(barcode);

            Task.Run(async () => {
                await _searchResultViewModel.InitializationAsync();
            });

            BindingContext = _searchResultViewModel;
        }
    }
}
