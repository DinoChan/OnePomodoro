using OnePomodoro.Models;
using OnePomodoro.Services;
using Prism.Unity.Windows;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace OnePomodoro.ViewModels
{
    class ViewTypeSettingViewModel : ViewModelBase
    {

        public ViewTypeSettingViewModel()
        {
            Settings = SettingsService.Current;
        }

        public IPomodoroSettings Settings { get; }

        public async void OnSettingChanged()
        {
            await SettingsService.SaveAsync();

            //if (Settings.IsNotifyWhenPomodoroFinished)
            //{
            //    if (PomodoroViewModel.Current.IsInPomodoro && PomodoroViewModel.Current.IsTimerInProgress)
            //        _toastNotificationsService.AddPomodoroFinishedToastNotificationSchedule(DateTime.Now + PomodoroViewModel.Current.RemainingPomodoroTime);
            //}
            //else
            //{
            //    _toastNotificationsService.RemovePomodoroFinishedToastNotificationSchedule();
            //}

            //if (Settings.IsNotifyWhenBreakFinished)
            //{
            //    if (PomodoroViewModel.Current.IsInPomodoro == false && PomodoroViewModel.Current.IsTimerInProgress)
            //        _toastNotificationsService.AddBreakFinishedToastNotificationSchedule(DateTime.Now + PomodoroViewModel.Current.RemainingBreakTime);
            //}
            //else
            //{
            //    _toastNotificationsService.RemoveBreakFinishedToastNotificationSchedule();

            //}
        }
    }
}
