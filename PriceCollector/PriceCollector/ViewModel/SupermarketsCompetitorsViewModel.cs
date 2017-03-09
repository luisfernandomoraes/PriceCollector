using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Toasts;
using PriceCollector.Annotations;
using PriceCollector.Model;
using PriceCollector.View;
using Xamarin.Forms;

namespace PriceCollector.ViewModel
{
    public class SupermarketsCompetitorsViewModel:INotifyPropertyChanged
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
                await _notificator.Notify(ToastNotificationType.Error, ":(", "Ocorreu um erro inesperado, tente mais tarde.",TimeSpan.FromSeconds(3));
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

        public SupermarketsCompetitorsViewModel()
        {
            throw new NotImplementedException("Chama o outro ctor da classe");
        }

        public SupermarketsCompetitorsViewModel(SupermarketsCompetitorsPage supermarketsCompetitorsPage)
        {
            _supermarketsCompetitorsPage = supermarketsCompetitorsPage;
            _notificator = DependencyService.Get<IToastNotificator>();
            LoadAsync();
        }

        private void LoadAsync()
        {
            SupermarketsCompetitorses = new List<SupermarketsCompetitors>()
            {
                new SupermarketsCompetitors()
                {
                    Name = ""
                }
            };
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