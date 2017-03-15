using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriceCollector.ViewModel;
using Xamarin.Forms;

namespace PriceCollector.View
{
    public partial class SupermarketsCompetitorsPage : ContentPage
    {
        private SupermarketsCompetitorsViewModel _vm;

        public SupermarketsCompetitorsPage()
        {
            InitializeComponent();
            _vm = new SupermarketsCompetitorsViewModel(this);
            BindingContext = _vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _vm.LoadAsync();
        }

        private void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
