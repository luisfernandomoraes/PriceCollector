using System.Threading.Tasks;
using Xamarin.Forms;

namespace PriceCollector.ViewModel.Services
{
    public interface INavigationService
    {

        Task NavigateToAsync(Page page);

        Task PopPageAsync();
    }
}