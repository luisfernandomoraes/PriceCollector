using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using ZXing.Mobile;

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

   
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}