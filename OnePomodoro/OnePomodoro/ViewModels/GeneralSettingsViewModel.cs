using OnePomodoro.Models;
using OnePomodoro.Services;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Microsoft.Practices.Unity;
using Prism.Unity.Windows;
using Windows.ApplicationModel;

namespace OnePomodoro.ViewModels
{
    public class GeneralSettingsViewModel : ViewModelBase
    {
        private IToastNotificationsService _toastNotificationsService;

        public GeneralSettingsViewModel()
        {
            Settings = SettingsService.Current;
            if (DesignMode.DesignMode2Enabled == false && App.Current is PrismUnityApplication)
                _toastNotificationsService = App.Current.Container.Resolve<IToastNotificationsService>();
        }

        public ISettings Settings { get; }

        public async void OnSettingChanged()
        {
            await SettingsService.SaveAsync();

            if (Settings.IsNotifyWhenPomodoroFinished)
            {
                if (PomodoroViewModel.Current.IsInPomodoro && PomodoroViewModel.Current.IsTimerInProgress)
                    _toastNotificationsService.AddPomodoroFinishedToastNotificationSchedule(DateTime.Now + PomodoroViewModel.Current.RemainingPomodoroTime);
            }
            else
            {
                _toastNotificationsService.RemovePomodoroFinishedToastNotificationSchedule();
            }

            if (Settings.IsNotifyWhenBreakFinished)
            {
                if (PomodoroViewModel.Current.IsInPomodoro == false && PomodoroViewModel.Current.IsTimerInProgress)
                    _toastNotificationsService.AddBreakFinishedToastNotificationSchedule(DateTime.Now + PomodoroViewModel.Current.RemainingBreakTime);
            }
            else
            {
                _toastNotificationsService.RemoveBreakFinishedToastNotificationSchedule();

            }
        }
    }
}
