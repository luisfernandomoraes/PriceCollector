using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriceCollector.Api.WebAPI.Products;
using PriceCollector.Api.WebAPI.SupermarketCompetitors;
using PriceCollector.Api.WebAPI.User;
using PriceCollector.Utils;
using PriceCollector.View.Services;
using PriceCollector.View.SliderMenu;
using PriceCollector.ViewModel.Services;
using Xamarin.Forms;

namespace PriceCollector
{
    public partial class App : Application, ILoginManager
    {
        public const string MessageOnStart = "OnStart";
        public const string MessageOnSleep = "OnSleep";
        public const string MessageOnResume = "OnResume";


        public static App CurrentApp;

        public App()
        {
            #region DependencyInjection Register

            DependencyService.Register<IUserApi, UserApi>();
            DependencyService.Register<IProductApi, ProductApi>();
            DependencyService.Register<ISupermarketCompetitorApi, SupermarketCompetitorApi>();
            DependencyService.Register<INavigationService, NavigationService>();

            #endregion

            InitializeComponent();
            InitializeConfigurations();
            CurrentApp = this;

            MainPage = new NavigationPage(new LoginModalPage(this) { Title = "Autenticação de Usuário" });
        }


        private void InitializeConfigurations()
        {

            App.Current.Properties["IsLoggedIn"] = false;
#if DEMO
            // Carregar dados de demonstração
            if (!DB.DBContext.SupermarketsCompetitorsDataBase.GetItems().Any())
            {


                var imperatriz = new Model.SupermarketsCompetitors()
                {
                    Name = "Supermercados Imperatriz",
                    Street = "Rod. Jornalista Maurício Sirotsky",
                    Neighborhood = "Jurerê Internacional",
                    Number = "7475",
                    City = "Florianópolis"
                };

                var hippo = new Model.SupermarketsCompetitors()
                {
                    Name = "Hippo Supermercados",
                    Street = "R Almirante Alvim",
                    Neighborhood = "Centro",
                    Number = "555",
                    City = "Florianópolis"
                };


                DB.DBContext.SupermarketsCompetitorsDataBase.SaveItem(imperatriz);
                DB.DBContext.SupermarketsCompetitorsDataBase.SaveItem(hippo);

            }
#endif
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
            MessagingCenter.Send<App>(this, MessageOnStart);
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            MessagingCenter.Send<App>(this, MessageOnSleep);

        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            MessagingCenter.Send<App>(this, MessageOnResume);

        }

    }
}
