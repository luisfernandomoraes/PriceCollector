using System.Collections.Generic;
using Xamarin.Forms;

namespace PriceCollector.View.SliderMenu
{
    public class MenuListView : ListView
    {
        public MenuListView()
        {
            List<MenuItem> data = new MenuListData();

            ItemsSource = data;
            VerticalOptions = LayoutOptions.FillAndExpand;
            BackgroundColor = Color.Transparent;

            var cell = new DataTemplate(typeof(MenuCell));
            cell.SetBinding(TextCell.TextProperty, "Titulo");
            

            ItemTemplate = cell;
        }
    }
}
