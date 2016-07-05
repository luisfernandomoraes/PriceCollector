using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace PriceCollector.View.SliderMenu
{
    public class RootPage : MasterDetailPage
    {
        private readonly MenuPage _menuPage;

        public RootPage()
        {
            _menuPage = new MenuPage();

            _menuPage.Menu.ItemSelected +=
                (sender, e) => NavigateTo(e.SelectedItem as MenuItem);
            
            Master = _menuPage;

            Application.Current.Properties["menuPage"] = _menuPage;

            Detail = new NavigationPage(new MainPage()); //{ BarBackgroundColor = Color.FromHex(Utils.Constants.BarBackgroundColor) };

        }


        async void NavigateTo(MenuItem menu)
        {
            
            try
            {
                if (menu == null)
                    return;

                //Validações page por page.
                //TODO: estudar algum modo de simplificar isso.
                bool canNavigate = ValidateNavigation(menu.TargetType);
                if (!canNavigate)
                {
                    _menuPage.Menu.SelectedItem = null;
                    IsPresented = false;
                    return;
                }

                Page displayPage = (Page)Activator.CreateInstance(menu.TargetType);

                Detail = new NavigationPage(displayPage) { BarBackgroundColor = Color.FromHex(Utils.Constants.BarBackgroundColor) };

                _menuPage.Menu.SelectedItem = null;
                IsPresented = false;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("ERRO", "Erro " + ex.Message, "OK");
            }

        }


        /// <summary>
        ///  Rever.
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        private bool ValidateNavigation(Type targetType)
        {
            Debug.WriteLine(targetType);
            // Se a pagina selecionada for a pesquisa de preço e
            //if (targetType == typeof(SearchProduct))
            //{
            //    // ou a opção de ignorar validação de qr estiver desativada, e não estiver lido um qr valido
            //    if (!App.CurrentConfiguration.IgnoreQrcodeValidation && !App.CurrentApp.Properties.ContainsKey("Qrcode"))
            //    {
            //        DisplayAlert("ScannPrice", "Você deve ler o Qrcode para utilizar a consulta de produto!", "Ok");
            //        return false;
            //    }
            //    // ou a opção de ignorar ssid estiver desativada e o ssid corrente não for o ssid alvo.return false;
            //    if (!App.CurrentConfiguration.IgnoreSSIDValidation && App.CurrentSSID != App.CurrentConfiguration.TargetSSID)
            //    {
            //        DisplayAlert("ScannPrice", "Você deve estar logado na wifi do mercado para utilizar a consulta de produto!", "Ok");
            //        return false;
            //    }
            //}
            
            return true;
        }
    }
}
