using System;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using OnePomodoro.Helpers;
using OnePomodoro.Services;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;

namespace OnePomodoro.ViewModels
{
    public class PomodoroViewModel : ObservableObject
    {
        private int _completedPomodoros;
        private CountdownTimer _currentTimer;
        private bool _isInPomodoro;
        private bool _isTimerInProgress;
        private bool _isUserStop;
        private TimeSpan _remainingBreakTime;
        private TimeSpan _remainingPomodoroTime;

        public PomodoroViewModel()
        {
            StartTimerCommand = new RelayCommand(StartTimer);
            StopTimerCommand = new RelayCommand(StopTimer);

            IsInPomodoro = true;
            RemainingPomodoroTime = PomodoroLength;
            TotalPomodoroTime = PomodoroLength;

            CoreApplication.LeavingBackground += OnLeavingBackground;
            CoreApplication.EnteredBackground += OnEnteredBackground;
        }

        public event EventHandler IsInPomodoroChanged;

        public event EventHandler RemainingBreakTimeChanged;

        public event EventHandler RemainingPomodoroTimeChanged;

        public static PomodoroViewModel Current { get; } = new PomodoroViewModel();

        public int CompletedPomodoros
        {
            get { return _completedPomodoros; }
            set
            {
                _completedPomodoros = value;
                OnPropertyChanged();
            }
        }

        public CountdownTimer CurrentTimer
        {
            get
            {
                return _currentTimer;
            }
            set
            {
                _currentTimer = value;
                _currentTimer.Elapsed += OnCurrentTimerElapsed;
                _currentTimer.Finished += OnCurrentFinished; ;
            }
        }

        public bool IsInPomodoro
        {
            get => _isInPomodoro;

            private set
            {
                _isInPomodoro = value;
                IsInPomodoroChanged?.Invoke(this, EventArgs.Empty);
                OnPropertyChanged();
            }
        }

        public bool IsTimerInProgress
        {
            get => _isTimerInProgress;

            private set
            {
                _isTimerInProgress = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan RemainingBreakTime
        {
            get => _remainingBreakTime;

            private set
            {
                _remainingBreakTime = value;
                RemainingBreakTimeChanged?.Invoke(this, EventArgs.Empty);
                OnPropertyChanged();
            }
        }

        public TimeSpan RemainingPomodoroTime
        {
            get => _remainingPomodoroTime;

            private set
            {
                _remainingPomodoroTime = value;
                RemainingPomodoroTimeChanged?.Invoke(this, EventArgs.Empty);
                OnPropertyChanged();
            }
        }

        public RelayCommand StartTimerCommand { get; }
        public RelayCommand StopTimerCommand { get; }
        public TimeSpan TotalBreakTime { get; private set; }
        public TimeSpan TotalPomodoroTime { get; private set; }
        private int LongBreakAfter => SettingsService.Current == null ? 4 : SettingsService.Current.LongBreakAfter;
        private TimeSpan LongBreakLength => TimeSpan.FromMinutes(SettingsService.Current == null ? 15 : SettingsService.Current.LongBreakLength);
        private TimeSpan PomodoroLength => TimeSpan.FromMinutes(SettingsService.Current == null ? 25 : SettingsService.Current.PomodoroLength);
        private TimeSpan ShortBreakLength => TimeSpan.FromMinutes(SettingsService.Current == null ? 5 : SettingsService.Current.ShortBreakLength);

        private async void OnBreakTimerFinished()
        {
            var startTime = CurrentTimer.StartTime;
            RemainingPomodoroTime = PomodoroLength;
            IsTimerInProgress = false;
            IsInPomodoro = true;

            if (SettingsService.Current.AutoStartOfNextPomodoro)
                await StartPomodoroAsync();

            await DataService.AddPeriodAsync(false, _isUserStop == false, startTime, DateTime.Now);
            _isUserStop = false;
        }

        private void OnCurrentFinished(object sender, EventArgs e)
        {
            if (_isInPomodoro)
                OnPomodoroTimerFinished();
            else
                OnBreakTimerFinished();
        }

        private void OnCurrentTimerElapsed(object sender, EventArgs e)
        {
            if (_isInPomodoro)
                RemainingPomodoroTime = CurrentTimer.RemainingTime;
            else
                RemainingBreakTime = CurrentTimer.RemainingTime;
        }

        private async void OnEnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            if (IsTimerInProgress && App.Current.HasExited == false)
            {
                var deferral = e.GetDeferral();
                try
                {
                    await NotificationManager.Current.AddAllNotificationsAsync(IsInPomodoro, CurrentTimer.StartTime + CurrentTimer.TotalTime,
                        SettingsService.Current.AutoStartOfNextPomodoro, SettingsService.Current.AutoStartOfBreak,
                        _completedPomodoros, LongBreakAfter, PomodoroLength,
                        ShortBreakLength, LongBreakLength);

                    await DataService.AddFuturePeriodsAsync(IsInPomodoro, CurrentTimer.StartTime + CurrentTimer.TotalTime,
                     SettingsService.Current.AutoStartOfNextPomodoro, SettingsService.Current.AutoStartOfBreak,
                     _completedPomodoros, LongBreakAfter, PomodoroLength,
                     ShortBreakLength, LongBreakLength);
                }
                catch (Exception ex)
                {
                    Microsoft.AppCenter.Crashes.Crashes.TrackError(ex);
                }
                deferral.Complete();
            }
        }

        private async void OnLeavingBackground(object sender, LeavingBackgroundEventArgs e)
        {
            try
            {
                if (CurrentTimer != null && IsTimerInProgress)
                    CurrentTimer.CheckTime();

                if (IsTimerInProgress)
                {
                    await NotificationManager.Current.RemoveAllNotificationsButFirstAsync(IsInPomodoro, CurrentTimer.StartTime + CurrentTimer.TotalTime);
                    await DataService.RemoveFuturePeriodsAsync();
                }
            }
            catch (Exception ex)
            {
                Microsoft.AppCenter.Crashes.Crashes.TrackError(ex);
            }
        }

        private async void OnPomodoroTimerFinished()
        {
            var startTime = CurrentTimer.StartTime;
            IsTimerInProgress = false;
            IsInPomodoro = false;
            CompletedPomodoros++;

            var breakLength = (CompletedPomodoros % LongBreakAfter == 0) ? LongBreakLength : ShortBreakLength;

            TotalBreakTime = breakLength;
            RemainingBreakTime = breakLength;

            if (SettingsService.Current.AutoStartOfBreak)
                await StartBreakAsync();

            await DataService.AddPeriodAsync(true, _isUserStop == false, startTime, DateTime.Now);
            _isUserStop = false;
        }

        private async Task StartBreakAsync()
        {
            IsTimerInProgress = true;

            var breakLength = (CompletedPomodoros % LongBreakAfter == 0) ? LongBreakLength : ShortBreakLength;
            if (SettingsService.Current.AutoStartOfBreak && CurrentTimer != null && CurrentTimer.RemainingTime == TimeSpan.Zero)
                CurrentTimer = new CountdownTimer(CurrentTimer.StartTime + CurrentTimer.TotalTime, breakLength);
            else
                CurrentTimer = new CountdownTimer(DateTime.Now, breakLength);

            if (SettingsService.Current.IsNotifyWhenBreakFinished && (CurrentTimer.StartTime + CurrentTimer.TotalTime) > DateTime.Now)
                await NotificationManager.Current.AddBreakFinishedToastNotificationScheduleAsync(CurrentTimer.StartTime + CurrentTimer.TotalTime);

            CurrentTimer.Start();
        }

        private async Task StartPomodoroAsync()
        {
            IsTimerInProgress = true;

            if (SettingsService.Current.AutoStartOfNextPomodoro && CurrentTimer != null && CurrentTimer.RemainingTime == TimeSpan.Zero)
                CurrentTimer = new CountdownTimer(CurrentTimer.StartTime + CurrentTimer.TotalTime, PomodoroLength);
            else
                CurrentTimer = new CountdownTimer(DateTime.Now, PomodoroLength);

            if (SettingsService.Current.IsNotifyWhenPomodoroFinished && (CurrentTimer.StartTime + CurrentTimer.TotalTime) > DateTime.Now)
                await NotificationManager.Current.AddPomodoroFinishedToastNotificationScheduleAsync(CurrentTimer.StartTime + CurrentTimer.TotalTime);

            CurrentTimer.Start();
        }

        private async void StartTimer()
        {
            if (_isInPomodoro)
                await StartPomodoroAsync();
            else
                await StartBreakAsync();
        }

        private async void StopTimer()
        {
            await NotificationManager.Current.RemovePomodoroFinishedToastNotificationScheduleAsync();
            await NotificationManager.Current.RemoveBreakFinishedToastNotificationScheduleAsync();
            _isUserStop = true;
            CurrentTimer.Stop();
        }
    }
}
