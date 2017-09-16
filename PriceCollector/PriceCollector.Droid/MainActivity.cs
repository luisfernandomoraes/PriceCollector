using System;
using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Toasts;
using Rb.Forms.Barcode.Droid;
using Xamarin.Forms;
using FloatingActionButton = Android.Support.Design.Widget.FloatingActionButton;
using Resource = PriceCollector.Droid.Resource;

namespace PriceCollector.Droid
{
    [Activity(Label = "PriceCollector", Icon = "@drawable/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            DependencyService.Register<ToastNotificatorImplementation>(); // Register your dependency
            ToastNotificatorImplementation.Init(this); //you can pass additional parameters here
                                                       //MobileBarcodeScanner.Initialize(Application);
                                                       //MobileBarcodeScanner.Initialize(this.Application);
            var configBarcodeScanner = new Configuration
            {
                // Some devices, mostly samsung, stop auto focusing as soon as one of the advanced features is enabled.
                CompatibilityMode = Build.Manufacturer.Contains("samsung"),
                Zoom = 5
            };

            BarcodeScannerRenderer.Init(configBarcodeScanner);

            FAB.Droid.FloatingActionButtonRenderer.InitControl();

            UserDialogs.Init(this);

            LoadApplication(new App());
        }
    }
}

