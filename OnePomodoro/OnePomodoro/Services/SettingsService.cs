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

        public static IPomodoroSettings Current { get; private set; }

        public static async Task InitializeAsync()
        {
            Current = await ApplicationData.Current.LocalSettings.ReadAsync<PomodoroSettings>(SettingsKey);
            if (Current == null)
            {
                Current = new PomodoroSettings { IsNotifyWhenBreakFinished = true, IsNotifyWhenPomodoroFinished = true, BreakAudioUri = string.Empty, PomodoroAudioUri = string.Empty };
                await SaveAsync();
            }

            if (Current.PomodoroLength == 0)
                Current.PomodoroLength = 25;

            if (Current.ShortBreakLength == 0)
                Current.ShortBreakLength = 5;

            if (Current.LongBreakLength == 0)
                Current.LongBreakLength = 15;

            if (Current.LongBreakAfter == 0)
                Current.LongBreakAfter = 4;
        }

        public static async Task SaveAsync()
        {
            await ApplicationData.Current.LocalSettings.SaveAsync(SettingsKey, Current);
        }
    }
}
