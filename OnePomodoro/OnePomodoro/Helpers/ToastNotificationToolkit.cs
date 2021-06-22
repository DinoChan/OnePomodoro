using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace OnePomodoro.Helpers
{
    public static class ToastNotificationToolkit
    {
        public static async Task<ToastNotifier> CreateToastNotifierAsync()
        {

            int tryCount = 0;
            while (true)
            {
                try
                {
                    return ToastNotificationManager.CreateToastNotifier();
                }
                catch (Exception)
                {
                    if (tryCount++ > 5)
                        throw;
                }
                await Task.Delay(TimeSpan.FromSeconds(0.1));
            }
        }
    }
}
