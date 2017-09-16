using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Toasts;
using PriceCollector.DB;
using PriceCollector.Model;
using PriceCollector.Properties;
using PriceCollector.View;
using PriceCollector.WebAPI.SupermarketCompetitors;
using Xamarin.Forms;

namespace PriceCollector.ViewModel
{
    public class ManageSupermarketViewModel : INotifyPropertyChanged
    {

        #region Fields

        private int _id;
        private string _name;
        private string _street;
        private string _number;
        private string _neighborhood;
        private string _city;
        private ISupermarketCompetitorApi _supermarketApi;
        private IToastNotificator _notificator;
        private ManageSupermarketPage _manageSupermarketPage;
        private int _idSupermarket;

        #endregion

        #region Commands

        public ICommand DeleteCommand => new Command(Delete);

        private async void Delete(object obj)
        {
            try
            {
                var result = await App.ShowQuestion("PriceCollector", "Deseja realmente deletar o supermercado?", "Sim", "Não");
                if (result)
                {
                    DBContext.SupermarketsCompetitorsDataBase.DeleteItem(ID);
                    await _manageSupermarketPage.Navigation.PopAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await _notificator.Notify(ToastNotificationType.Error, ":(", "Ocorreu um erro inesperado ao deletar o registro, porfavor tente mais tarde.", TimeSpan.FromSeconds(3));
            }
        }

        public ICommand SaveCommand => new Command(Save);

        private async void Save(object obj)
        {
            try
            {
                var market = DB.DBContext.SupermarketsCompetitorsDataBase.GetItem(ID);

                market.IDSupermarket = IDSupermarket;
                market.Name = Name;
                market.Street = Street;
                market.Number = Number;
                market.Neighborhood = Neighborhood;
                market.City = City;


                var result = await _supermarketApi.Put(DB.DBContext.CurrentAppConfiguration.WebApiAddress, market);
                if (!result.Success)
                {
                    await _notificator.Notify(ToastNotificationType.Error, ":(",
                        "Ocorreu um erro inesperado ao salvar suas alterações, porfavor tente mais tarde.", TimeSpan.FromSeconds(3));
                    return;
                }

                DB.DBContext.SupermarketsCompetitorsDataBase.Update(market);
                await _manageSupermarketPage.Navigation.PopAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await _notificator.Notify(ToastNotificationType.Error, ":(",
                    "Ocorreu um erro ao salvar o registro, porfavor tente mais tarde.", TimeSpan.FromSeconds(3));
            }
        }

        #endregion

        #region Properties

        public int ID
        {
            get { return _id; }
            set
            {
                if (value == _id) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        public int IDSupermarket
        {
            get { return _idSupermarket; }
            set
            {
                if (value == _idSupermarket) return;
                _idSupermarket = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Street
        {
            get { return _street; }
            set
            {
                if (value == _street) return;
                _street = value;
                OnPropertyChanged();
            }
        }

        public string Number
        {
            get { return _number; }
            set
            {
                if (value == _number) return;
                _number = value;
                OnPropertyChanged();
            }
        }

        public string Neighborhood
        {
            get { return _neighborhood; }
            set
            {
                if (value == _neighborhood) return;
                _neighborhood = value;
                OnPropertyChanged();
            }
        }

        public string City
        {
            get { return _city; }
            set
            {
                if (value == _city) return;
                _city = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Ctor

        public ManageSupermarketViewModel(Model.SupermarketsCompetitors supermarketsCompetitors, View.ManageSupermarketPage manageSupermarketPage)
        {
            Task.Run(async () => { await LoadAsync(supermarketsCompetitors); });
            _manageSupermarketPage = manageSupermarketPage;
            _supermarketApi = DependencyService.Get<ISupermarketCompetitorApi>();
            _notificator = DependencyService.Get<IToastNotificator>();
        }

        #endregion

        #region Methods

        private async Task LoadAsync(SupermarketsCompetitors market)
        {
            try
            {
                ID = market.ID;
                Name = market.Name;
                Neighborhood = market.Neighborhood;
                Number = market.Number;
                Street = market.Street;
                City = market.City;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await _notificator.Notify(ToastNotificationType.Error, ":(",
                    "Ocorreu um erro inesperado, por favor tente mais tarde.", TimeSpan.FromSeconds(3));
                await _manageSupermarketPage.Navigation.PopAsync();
            }
        }

        #endregion

        #region INotifyPropertyChangeImpl

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}