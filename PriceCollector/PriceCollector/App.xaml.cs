using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceCollector.Api.WebAPI.Products;
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
            DependencyService.Register<IUserApi, UserApi>();
            DependencyService.Register<IProductApi, ProductApi>();


            InitializeComponent();
            InitializeConfigurations();
            CurrentApp = this;

            MainPage = new NavigationPage(new LoginModalPage(this));
        }

        private void InitializeConfigurations()
        {
            Application.Current.Properties["IsLoggedIn"] = false;
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

        public void ShowMainPage()
        {
            // Se ja estiver logado, apenas apresentar pagina inicial.
            if ((bool)Properties["IsLoggedIn"])
                MainPage = new RootPage();
            else
                MainPage = new NavigationPage(new LoginModalPage(this));
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

        /// <summary>
        /// Deletar usuario.
        /// </summary>
        private void DeleteAll()
        {

        }
    }
}
