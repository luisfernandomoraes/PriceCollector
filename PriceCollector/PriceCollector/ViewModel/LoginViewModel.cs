using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Plugin.Toasts;
using PriceCollector.Api.WebAPI.User;
using PriceCollector.Properties;
using PriceCollector.Utils;
using Xamarin.Forms;

namespace PriceCollector.ViewModel
{

    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly ILoginManager _ilm;
        private readonly IToastNotificator _notificator;
        private readonly IUserApi _userApi;
        private string _userName;
        private string _password;

        public ICommand LoginCommand => new Command(Login);

        private string UserName
        {
            get { return _userName; }
            set
            {
                if (value == _userName) return;
                _userName = value;
                OnPropertyChanged();
            }
        }

        private string Password
        {
            get { return _password; }
            set
            {
                if (value == _password) return;
                _password = value;
                OnPropertyChanged();
            }
        }

        public LoginViewModel(ILoginManager ilm)
        {
            _ilm = ilm;
            _notificator = DependencyService.Get<IToastNotificator>();
            _userApi = DependencyService.Get<IUserApi>();
        }

        private async void Login(object obj)
        {
            var userResponse = await _userApi.Login(UserName, Password);

            if (userResponse.Success && userResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                Application.Current.Properties["IsLoggedIn"] = true;
                
                await Application.Current.MainPage.Navigation.PushAsync(new View.PickSupermarketPage(_ilm));
                //_ilm.ShowMainPage();
                // await _notificator.Notify(ToastNotificationType.Success, Constants.AppName, "Login efetuado com sucesso", TimeSpan.FromSeconds(3));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}