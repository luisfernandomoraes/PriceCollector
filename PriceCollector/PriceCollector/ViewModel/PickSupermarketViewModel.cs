using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Toasts;
using PriceCollector.Annotations;
using PriceCollector.Model;
using PriceCollector.Utils;
using PriceCollector.View;
using Xamarin.Forms;

namespace PriceCollector.ViewModel
{
    public class PickSupermarketViewModel:INotifyPropertyChanged
    {
        #region Fields

        private PickSupermarketPage _page;
        private IToastNotificator _notificator;
        private CreateSupermarketPage _createSupermarketPage;
        private ICollection<SupermarketsCompetitors> _supermarketsCompetitors;
        private ILoginManager _ilm;

        #endregion

        #region Properties

        private CreateSupermarketPage SupermarketPage => _createSupermarketPage ?? (_createSupermarketPage = new CreateSupermarketPage());

        public ICollection<Model.SupermarketsCompetitors> SupermarketsCompetitors
        {
            get { return _supermarketsCompetitors; }
            set
            {
                _supermarketsCompetitors = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand AddSupermarketCommand => new Command(AddSupermarke);

        private async void AddSupermarke()
        {
            try
            {
                await _page.Navigation.PushAsync(SupermarketPage);
            }
            catch (Exception e)
            {
                await _notificator.Notify(ToastNotificationType.Error, ":(", "Ocorreu um erro inesperado, tente mais tarde.", TimeSpan.FromSeconds(3));
                Debug.WriteLine(e);
            }
        }

        #endregion
        #region Ctor

        public PickSupermarketViewModel(PickSupermarketPage page, ILoginManager ilm)
        {
            _page = page;
            _notificator = DependencyService.Get<IToastNotificator>();
            _ilm = ilm;
            //Task.Run(async () => await LoadAsync());
        }

        #endregion

        #region Methods

        public async Task LoadAsync()
        {
            try
            {
                SupermarketsCompetitors = DB.DBContext.SupermarketsCompetitorsDataBase.GetItems().ToList();
            }
            catch (Exception e)
            {
                await _notificator.Notify(ToastNotificationType.Error, "PriceCollector",
                    "Ocorreu um erro ao carregar os supermercados concorrentes, por favor tente mais tarde.",
                    TimeSpan.FromSeconds(3));
                Debug.WriteLine(e);
            }
        }

        public void SetSupermarketToWillCollectedProdutcs(SupermarketsCompetitors market)
        {
            Application.Current.Properties[nameof(Model.SupermarketsCompetitors)] = market;
            _ilm.ShowMainPage();
        }
        #endregion

        #region INotifyPropertyChangedImpl

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

       
    }
}