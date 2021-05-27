using System;
using System.Threading.Tasks;

using Microsoft.Toolkit.Uwp.Helpers;

using OnePomodoro.Views;

namespace OnePomodoro.Services
{
    public class FirstRunDisplayService : IFirstRunDisplayService
    {
        private static bool shown = false;

        public async Task ShowIfAppropriateAsync()
        {
            if (SystemInformation.Instance.IsFirstRun && !shown)
            {
                shown = true;
                var dialog = new FirstRunDialog();
                await dialog.ShowAsync();
            }
        }
    }
}
