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
        public SearchResultViewModel SearchResultViewModel { get; }

        public SearchResultPage(string barcode)
        {
            InitializeComponent();
            SearchResultViewModel = new SearchResultViewModel(barcode);
            BindingContext = SearchResultViewModel;
        }
    }
}
