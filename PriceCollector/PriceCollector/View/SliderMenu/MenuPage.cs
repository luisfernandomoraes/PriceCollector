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
			Menu = new MenuListView ();
            
            
            var menuLabel = new Image {
                Source = "back_ground_menu.jpg"

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
