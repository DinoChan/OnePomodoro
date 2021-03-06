﻿using OnePomodoro.Models;
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
using OnePomodoro.Helpers;
using Windows.UI.Core;
using System.ComponentModel;
using OnePomodoro.Infrastructure;

namespace OnePomodoro.ViewModels
{
    public class GeneralSettingsViewModel : ViewModelBase
    {
        private ToastNotificationsService _toastNotificationsService;

        public GeneralSettingsViewModel()
        {
            Settings = SettingsService.Current;
            (Settings as INotifyPropertyChanged).PropertyChanged += OnPropertyChanged;
            if (DesignMode.DesignMode2Enabled == false && App.Current is PrismUnityApplication)
                _toastNotificationsService = App.Current.Container.Resolve<ToastNotificationsService>();

            Audios = AudioDefinitions.Definitions;

        }

        private async void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            await SettingsService.SaveAsync();

            if (Settings.IsNotifyWhenPomodoroFinished)
            {
                if (PomodoroViewModel.Current.IsInPomodoro && PomodoroViewModel.Current.IsTimerInProgress)
                    NotificationManager.Current.AddPomodoroFinishedToastNotificationSchedule(DateTime.Now + PomodoroViewModel.Current.RemainingPomodoroTime);
            }
            else
            {
                NotificationManager.Current.RemovePomodoroFinishedToastNotificationSchedule();
            }

            if (Settings.IsNotifyWhenBreakFinished)
            {
                if (PomodoroViewModel.Current.IsInPomodoro == false && PomodoroViewModel.Current.IsTimerInProgress)
                    NotificationManager.Current.AddBreakFinishedToastNotificationSchedule(DateTime.Now + PomodoroViewModel.Current.RemainingBreakTime);
            }
            else
            {
                NotificationManager.Current.RemoveBreakFinishedToastNotificationSchedule();

            }

            if (e.PropertyName == nameof(IPomodoroSettings.PomodoroAudioUri))
            {
                if (Settings.IsNotifyWhenPomodoroFinished && PomodoroViewModel.Current.IsInPomodoro && PomodoroViewModel.Current.IsTimerInProgress)
                    NotificationManager.Current.AddPomodoroFinishedToastNotificationSchedule(DateTime.Now + PomodoroViewModel.Current.RemainingPomodoroTime);
            }
            else if (e.PropertyName == nameof(IPomodoroSettings.BreakAudioUri))
            {
                if (PomodoroViewModel.Current.IsInPomodoro == false && PomodoroViewModel.Current.IsTimerInProgress)
                    NotificationManager.Current.AddBreakFinishedToastNotificationSchedule(DateTime.Now + PomodoroViewModel.Current.RemainingBreakTime);
            }
        }

        public IEnumerable<AudioDefinition> Audios { get; }

        public IPomodoroSettings Settings { get; }
    }
}
