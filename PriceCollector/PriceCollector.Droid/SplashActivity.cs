using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;

namespace PriceCollector.Droid
{
    [Activity(Theme = "@style/Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        private static readonly string Tag = "X:" + typeof(SplashActivity).Name;

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            
            base.OnCreate(savedInstanceState, persistentState);
            Log.Debug(Tag, "SplashActivity.OnCreate");
        }

        //protected override void OnCreate(Bundle savedInstanceState)
        //{
        //    //Task.Run(() =>
        //    //{
        //    //    Wifi w = new Wifi(Xamarin.Forms.Forms.Context);
        //    //    w.GetWifiNetworks();
        //    //});

        //    //            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
        //    //            {
        //    //                Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
        //    //                Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
        //    //                Window.SetStatusBarColor(new Android.Graphics.Color(0, 102, 51));
        //    //            }

        //    base.OnCreate(savedInstanceState);
        //}

        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            
            //Task startupWork = new Task(() =>
            //{
            //    Log.Debug(Tag, "Performing some startup work that takes a bit of time.");
            //    //Task.Delay(3000);  // Simulate a bit of startup work
            //    Log.Debug(Tag, "Working in the background - important stuff.");
            //});

            //startupWork.ContinueWith(t =>
            //{
            //    Log.Debug(Tag, "Work is finished - start Activity1.");
            //    StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            //}, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}