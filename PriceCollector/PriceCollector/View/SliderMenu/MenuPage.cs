using Xamarin.Forms;

namespace PriceCollector.View.SliderMenu
{
	public class MenuPage : ContentPage
	{
		public ListView Menu { get; set; }

		public MenuPage ()
		{
			Title = "menu";
            BackgroundColor = Color.Default;
			Icon = "ic_menu_black_24dp.png";
			Menu = new MenuListView ();
            
            
            var menuLabel = new Image {
                Source = "CabACATSMenu.png"
                /*Padding = new Thickness(10, 36, 0, 5),
                Content = new Label {
                    TextColor = Color.Black,
					Text = "Menu",
                    FontSize = 20
				}*/

            };

			var layout = new StackLayout {
				Spacing = 0,
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			layout.Children.Add (menuLabel);
			layout.Children.Add (Menu);

			Content = layout;
		}
    }
}
