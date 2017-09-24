using Android.Widget;
using PriceCollector.Controls;
using PriceCollector.Droid.Renders;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomImageCell), typeof(CustomImageCellRenderer))]

namespace PriceCollector.Droid.Renders
{
    public class CustomImageCellRenderer : ImageCellRenderer
    {
        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, Android.Views.ViewGroup parent, Android.Content.Context context)
        {
            LinearLayout cell = (LinearLayout)base.GetCellCore(item, convertView, parent, context);
            ImageView image = (ImageView)cell.GetChildAt(0);
            image.SetScaleType(ImageView.ScaleType.Center);
            return cell;
        }
    }
}