using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriceCollector.Api.WebAPI.Products;
using PriceCollector.Api.WebAPI.SupermarketCompetitors;
using PriceCollector.Api.WebAPI.User;
using PriceCollector.Utils;
using PriceCollector.View.SliderMenu;

using Xamarin.Forms;

namespace PriceCollector
{
    public partial class App : Application, ILoginManager
    {
        public static App CurrentApp;

        public App()
        {
            #region DependencyInjection Register

            DependencyService.Register<IUserApi, UserApi>();
            DependencyService.Register<IProductApi, ProductApi>();
            DependencyService.Register<ISupermarketCompetitorApi, SupermarketCompetitorApi>();

            #endregion

            InitializeComponent();
            InitializeConfigurations();
            CurrentApp = this;

            MainPage = new NavigationPage(new LoginModalPage(this) { Title = "Autenticação de Usuário" });
        }


        private void InitializeConfigurations()
        {
            App.Current.Properties["IsLoggedIn"] = false;
        }



        public void ShowMainPage()
        {
            // Se ja estiver logado, apenas apresentar pagina inicial.
            if ((bool)Properties["IsLoggedIn"])
                MainPage = new RootPage();
            else
                MainPage = new NavigationPage(new LoginModalPage(this));
        }

        public static async Task<bool> ShowQuestion(string title, string message, string accept, string cancel)
        {
            var result = await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
            return result;
        }

        public void Logout()
        {
            Properties["IsLoggedIn"] = false; // only gets set to 'true' on the LoginPage
            Properties["IsRegistered"] = false;
            // TODO: Confirmar isso;
            // Preciso recuperar o registro antes de deletar a base, para que eu possa deletar tambem na web api.
            /* var user = DatabaseUser.GetItems().FirstOrDefault();
             DeleteAll();
             if (user != null)
                 DependencyService.Get<IUserApi>().Delete(CurrentConfiguration.WebApiAddress, user.ID);*/
            MainPage = new NavigationPage(new LoginModalPage(this));
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        
    }
}
