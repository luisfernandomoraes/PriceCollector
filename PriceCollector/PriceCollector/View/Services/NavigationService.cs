using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriceCollector.ViewModel.Services;
using Xamarin.Forms;

namespace PriceCollector.View.Services
{
    public class NavigationService : INavigationService
    {
        public async Task NavigateToAsync(Page page)
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(page);
        }

        public async Task PopPageAsync()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
