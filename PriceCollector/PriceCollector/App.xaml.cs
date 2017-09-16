using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriceCollector.Utils;
using PriceCollector.View.Services;
using PriceCollector.View.SliderMenu;
using PriceCollector.ViewModel.Services;
using PriceCollector.WebAPI.Products;
using PriceCollector.WebAPI.SupermarketCompetitors;
using PriceCollector.WebAPI.User;
using Scandit.BarcodePicker.Unified;
using Scandit.BarcodePicker.Unified.Abstractions;
using Xamarin.Forms;

namespace PriceCollector
{
    public partial class App : Application, ILoginManager
    {
        public const string MessageOnStart = "OnStart";
        public const string MessageOnSleep = "OnSleep";
        public const string MessageOnResume = "OnResume";
        private static string _appKey = "xbENFiLaEeSCn/KdTNpva0lad8WlMjCpK89rxosdty4";


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
            InitScannerSettings();

            CurrentApp = this;

            MainPage = new NavigationPage(new LoginModalPage(this) { Title = "Autenticação de Usuário" });
        }

        async void InitScannerSettings()
        {
            ScanditService.ScanditLicense.AppKey = _appKey;

            IBarcodePicker picker = ScanditService.BarcodePicker;

            // The scanning behavior of the barcode picker is configured through scan
            // settings. We start with empty scan settings and enable a very generous
            // set of symbolizes. In your own apps, only enable the symbolizes you
            // actually need.
            var settings = picker.GetDefaultScanSettings();
            var symbologiesToEnable = new Symbology[] {
                Symbology.Ean13,
                Symbology.Ean8,
            };
            foreach (var sym in symbologiesToEnable)
                settings.EnableSymbology(sym, true);
            await picker.ApplySettingsAsync(settings);
            // This will open the scanner in full-screen mode. 
        }
        private void InitializeConfigurations()
        {

            App.Current.Properties["IsLoggedIn"] = false;

            // Load demonstration data.
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
