using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Plugin.Toasts;
using PriceCollector.Annotations;
using PriceCollector.Api.WebAPI.SupermarketCompetitors;
using PriceCollector.View;
using Xamarin.Forms;

namespace PriceCollector.ViewModel
{
    public class CreateSupermarketViewModel : INotifyPropertyChanged
    {
        private readonly CreateSupermarketPage _createSupermarketPage;

        #region Fields

        private int _id;
        private string _name;
        private string _street;
        private string _number;
        private string _neighborhood;
        private string _city;
        private ISupermarketCompetitorApi _supermarketApi;
        private IToastNotificator _notificator;

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

        #region Commands

        public ICommand SaveCommand => new Command(Save);

        private async void Save(object obj)
        {
            try
            {
                // Criação do objeto 
                var supermarket = new Model.SupermarketsCompetitors
                {
                    Name = Name.TrimEnd(),
                    City = City.TrimEnd(),
                    Neighborhood = Neighborhood.TrimEnd(),
                    Street = Street.TrimEnd(),
                    Number = Number.TrimEnd()
                };

                // Persistindo no banco.
                var resultApi = await _supermarketApi.Post(DB.DBContext.CurrentAppConfiguration.WebApiAddress, supermarket);
                if (resultApi.Success)
                {
                    var resultDb = DB.DBContext.SupermarketsCompetitorsDataBase.SaveItem(supermarket);
                    if (resultDb > 0)
                    {
                        await _createSupermarketPage.Navigation.PopAsync();

                        await  _notificator.Notify(ToastNotificationType.Success, "PriceCollector",
                            "Supermercado concorrente adicionado com sucesso!",TimeSpan.FromSeconds(3));
                        
                    }
                        
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        #endregion

        #region Ctor

        public CreateSupermarketViewModel(CreateSupermarketPage createSupermarketPage)
        {
            _id = 0;
            _name = string.Empty;
            _street = string.Empty;
            _number = string.Empty;
            _neighborhood = string.Empty;
            _city = string.Empty;

            _supermarketApi = DependencyService.Get<ISupermarketCompetitorApi>();
            _notificator = DependencyService.Get<IToastNotificator>();
            _createSupermarketPage = createSupermarketPage;

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