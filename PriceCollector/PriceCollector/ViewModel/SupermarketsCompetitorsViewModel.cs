using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Toasts;
using PriceCollector.Model;
using PriceCollector.Properties;
using PriceCollector.View;
using Xamarin.Forms;

namespace PriceCollector.ViewModel
{
    public class SupermarketsCompetitorsViewModel : INotifyPropertyChanged
    {
        private ICollection<SupermarketsCompetitors> _supermarketsCompetitorses;
        private CreateSupermarketPage _createSupermarketPage;
        private IToastNotificator _notificator;
        private SupermarketsCompetitorsPage _supermarketsCompetitorsPage;

        private CreateSupermarketPage SupermarketPage
        {
            get { return _createSupermarketPage ?? (_createSupermarketPage = new CreateSupermarketPage()); }
            set { _createSupermarketPage = value; }
        }

        public ICommand AddSupermarketCommand => new Command(AddSupermarke);

        private async void AddSupermarke()
        {
            try
            {
                await _supermarketsCompetitorsPage.Navigation.PushAsync(SupermarketPage);
            }
            catch (Exception e)
            {
                await _notificator.Notify(ToastNotificationType.Error, ":(", "Ocorreu um erro inesperado, tente mais tarde.", TimeSpan.FromSeconds(3));
                Debug.WriteLine(e);
            }
        }

        public ICollection<SupermarketsCompetitors> SupermarketsCompetitorses
        {
            get { return _supermarketsCompetitorses; }
            set
            {
                if (Equals(value, _supermarketsCompetitorses)) return;
                _supermarketsCompetitorses = value;
                OnPropertyChanged();
            }
        }

        public SupermarketsCompetitorsViewModel(SupermarketsCompetitorsPage supermarketsCompetitorsPage)
        {
            _supermarketsCompetitorsPage = supermarketsCompetitorsPage;
            _notificator = DependencyService.Get<IToastNotificator>();
            Task.Run(async () => await LoadAsync());
        }

        public async Task LoadAsync()
        {
            try
            {
                SupermarketsCompetitorses = DB.DBContext.SupermarketsCompetitorsDataBase.GetItems().ToList();
            }
            catch (Exception e)
            {
                await _notificator.Notify(ToastNotificationType.Error, "PriceCollector",
                    "Ocorreu um erro ao carregar os supermercados concorrentes, por favor tente mais tarde.",
                    TimeSpan.FromSeconds(3));
                throw;
            }
        }

        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}