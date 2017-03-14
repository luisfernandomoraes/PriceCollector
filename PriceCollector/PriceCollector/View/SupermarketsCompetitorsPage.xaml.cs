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
        public SupermarketsCompetitorsPage()
        {
            InitializeComponent();
            BindingContext = new SupermarketsCompetitorsViewModel(this);
        }

        private void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
