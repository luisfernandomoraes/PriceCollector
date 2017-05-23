using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriceCollector.Model;
using PriceCollector.ViewModel;
using PriceCollector.ViewModel.Interfaces;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace PriceCollector.View
{
    public partial class SearchResultPage: PopupPage
    {
        private ProductCollected product;

        public SearchResultViewModel SearchResultViewModel { get; }

        /// <summary>
        /// Ctor utilizado para cadastrar o produto coletado.
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="mainPageReloadDataViewModel"></param>
        public SearchResultPage(string barcode, IReloadDataViewModel mainPageReloadDataViewModel)
        {
            InitializeComponent();
            SearchResultViewModel = new SearchResultViewModel(barcode,mainPageReloadDataViewModel);
            BindingContext = SearchResultViewModel;
        }

        /// <summary>
        /// Ctor utilizado para edição do produto coletado.
        /// </summary>
        /// <param name="product"></param>
        public SearchResultPage(ProductCollected product)
        {
            InitializeComponent();
            SearchResultViewModel = new SearchResultViewModel(product);
            BindingContext = SearchResultViewModel;
        }
    }
}
