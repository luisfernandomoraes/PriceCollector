using PriceCollector.Utils;
using Xamarin.Forms;

namespace PriceCollector.View.SliderMenu
{
    public class LoginModalPage : CarouselPage
    {
        readonly ContentPage login;

        public LoginModalPage(ILoginManager ilm)
        {
            if (!(bool)Application.Current.Properties["IsLoggedIn"])
            {
                // Se não estiver registrado, proceder com a view de crição do usuario.
                login = new LoginPage(ilm);
            }
            
            Children.Add(login);

            MessagingCenter.Subscribe<ContentPage>(this, "Login", (sender) =>
                {
                    SelectedItem = login;
                });
            
        }
    }
}
