using System;
using Plugin.Toasts;

namespace PriceCollector.Droid
{
    // This class is never actually executed, but when Xamarin linking is enabled it does how to ensure types and properties
    // are preserved in the deployed app.

    public class LinkerPleaseInclude
    {
        public void Include(IToastNotificator notificator)
        {
            notificator.Notify(ToastNotificationType.Success, String.Empty, String.Empty, TimeSpan.MinValue);
        }
    }
}