using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using OnePomodoro.Helpers;
using OnePomodoro.Infrastructure;
using OnePomodoro.Models;
using OnePomodoro.Services;
using Windows.ApplicationModel;

namespace OnePomodoro.ViewModels
{
    public class GeneralSettingsViewModel : ObservableObject
    {
        private ToastNotificationsService _toastNotificationsService;

        public GeneralSettingsViewModel()
        {
            Settings = SettingsService.Current;
            (Settings as INotifyPropertyChanged).PropertyChanged += OnPropertyChanged;
            if (DesignMode.DesignMode2Enabled == false)
                _toastNotificationsService = App.Current.Services.GetService<ToastNotificationsService>();

            Audios = AudioDefinitions.Definitions;
        }

        private async void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                await SettingsService.SaveAsync();

                if (Settings.IsNotifyWhenPomodoroFinished)
                {
                    if (PomodoroViewModel.Current.IsInPomodoro && PomodoroViewModel.Current.IsTimerInProgress)
                        await NotificationManager.Current.AddPomodoroFinishedToastNotificationScheduleAsync(DateTime.Now + PomodoroViewModel.Current.RemainingPomodoroTime);
                }
                else
                {
                  await  NotificationManager.Current.RemovePomodoroFinishedToastNotificationScheduleAsync();
                }

                if (Settings.IsNotifyWhenBreakFinished)
                {
                    if (PomodoroViewModel.Current.IsInPomodoro == false && PomodoroViewModel.Current.IsTimerInProgress)
                        await NotificationManager.Current.AddBreakFinishedToastNotificationScheduleAsync(DateTime.Now + PomodoroViewModel.Current.RemainingBreakTime);
                }
                else
                {
                  await  NotificationManager.Current.RemoveBreakFinishedToastNotificationScheduleAsync();
                }

                if (e.PropertyName == nameof(IPomodoroSettings.PomodoroAudioUri))
                {
                    if (Settings.IsNotifyWhenPomodoroFinished && PomodoroViewModel.Current.IsInPomodoro && PomodoroViewModel.Current.IsTimerInProgress)
                        await NotificationManager.Current.AddPomodoroFinishedToastNotificationScheduleAsync(DateTime.Now + PomodoroViewModel.Current.RemainingPomodoroTime);
                }
                else if (e.PropertyName == nameof(IPomodoroSettings.BreakAudioUri))
                {
                    if (PomodoroViewModel.Current.IsInPomodoro == false && PomodoroViewModel.Current.IsTimerInProgress)
                        await NotificationManager.Current.AddBreakFinishedToastNotificationScheduleAsync(DateTime.Now + PomodoroViewModel.Current.RemainingBreakTime);
                }
            }
            catch (Exception ex)
            {
                Microsoft.AppCenter.Crashes.Crashes.TrackError(ex);
            }
        }

        public IEnumerable<AudioDefinition> Audios { get; }

        public IPomodoroSettings Settings { get; }
    }
}
