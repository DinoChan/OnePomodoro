using OnePomodoro.Helpers;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Streaming.Adaptive;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace OnePomodoro
{
    public class PomodoroSettings : ViewModelBase
    {
        private const string SettingsKey = "PomodoroSettings";
        private
            static PomodoroSettings _current = new PomodoroSettings();

        public PomodoroSettings()
        {
        }

        public static PomodoroSettings Current { get; private set; }

        public bool AutoStartOfNextPomodoro { get; set; }

        public bool AutoStartOfBreak { get; set; }

        public bool IsNotifyWhenPomodoroFinished { get; set; }

        public bool IsNotifyWhenBreakFinished { get; set; }


        public static  async Task InitializeAsync()
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
