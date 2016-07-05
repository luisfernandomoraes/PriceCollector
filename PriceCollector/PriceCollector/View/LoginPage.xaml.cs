using PriceCollector.Utils;
using PriceCollector.ViewModel;
using Xamarin.Forms;

namespace PriceCollector.View
{
    public partial class LoginPage : ContentPage
    {
        
        public LoginPage(ILoginManager ilm)
        {
            InitializeComponent();
            BindingContext = new LoginViewModel(ilm);
        }
    }
}
