using OnePomodoro.Helpers;
using OnePomodoro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace OnePomodoro.Services
{
    public class SettingsService
    {
        private const string SettingsKey = "PomodoroSettings";
        private static PomodoroSettings _current = new PomodoroSettings();

        public static ISettings Current { get; private set; }

        public static async Task InitializeAsync()
        {
            Current = await ApplicationData.Current.LocalSettings.ReadAsync<PomodoroSettings>(SettingsKey);
            if (Current == null)
            {
                Current = new PomodoroSettings();
                await SaveAsync();
            }
        }

        public static async Task SaveAsync()
        {
            await ApplicationData.Current.LocalSettings.SaveAsync(SettingsKey, Current);
        }
    }
}
