using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rb.Forms.Barcode.Pcl;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PriceCollector.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScannerPage : ContentPage
    {
        public BarcodeScanner BarcodeScannerPage { get; }

        public ScannerPage()
        {
            InitializeComponent();
            // NavigationPage.SetHasBackButton(this, false);


            /*
             * So that we can release the camera when turning off phone or switching apps.
             */
            MessagingCenter.Subscribe<App>(this, App.MessageOnSleep, DisableScanner);
            MessagingCenter.Subscribe<App>(this, App.MessageOnResume, EnableScanner);
            BarcodeScannerPage = BarcodeScanner;
            BarcodeScanner.BarcodeChanged += AnimateFlash;
        }


        protected override void OnAppearing()
        {
            BarcodeScanner.IsEnabled = true;
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            BarcodeScanner.IsEnabled = false;
            base.OnDisappearing();
        }

        public void DisableScanner()
        {
            DisableScanner(null);
        }

        /*
         * Release camera so that other apps can access it.
         */
        private void DisableScanner(object sender)
        {
            try
            {
                BarcodeScanner.IsEnabled = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /*
         * All your camera belongs to us.
         */
        private void EnableScanner(object sender)
        {
            BarcodeScanner.IsEnabled = true;
        }

        private void AnimateFlash(object sender, BarcodeEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () => {
                await flash.FadeTo(1, 150, Easing.CubicInOut);
                flash.Opacity = 0;
            });
        }

        /*
         * You need to take care of realeasing the camera when you are done with it else bad things can happen!
         */
        ~ScannerPage()
        {
            DisableScanner(this);

            /*
             * Camera is released we dont need the events anymore.
             */
            MessagingCenter.Unsubscribe<App>(this, App.MessageOnSleep);
            MessagingCenter.Unsubscribe<App>(this, App.MessageOnResume);

            BarcodeScanner.BarcodeChanged -= AnimateFlash;
        }
    }
}
