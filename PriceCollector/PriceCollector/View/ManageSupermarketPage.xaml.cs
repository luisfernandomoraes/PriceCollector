using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriceCollector.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PriceCollector.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageSupermarketPage : ContentPage
    {
        private ViewModel.ManageSupermarketViewModel _viewModel;
        public ManageSupermarketPage(Model.SupermarketsCompetitors supermarketsCompetitors)
        {
            InitializeComponent();
            _viewModel = new ManageSupermarketViewModel(supermarketsCompetitors,this);
            BindingContext = _viewModel;
        }
    }
}
