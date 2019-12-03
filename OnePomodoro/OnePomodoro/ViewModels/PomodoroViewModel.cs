using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnePomodoro.Helpers;
using OnePomodoro.Services;
using Prism.Commands;
using Microsoft.Practices.Unity;
using Prism.Unity.Windows;
using Windows.ApplicationModel;

namespace OnePomodoro.ViewModels
{
    public class PomodoroViewModel : ViewModelBase
    {
        private TimeSpan PomodoroLength => TimeSpan.FromSeconds(SettingsService.Current.PomodoroLength);
        private TimeSpan ShortBreakLength => TimeSpan.FromSeconds(SettingsService.Current.ShortBreakLength);
        private TimeSpan LongBreakLength => TimeSpan.FromSeconds(SettingsService.Current.LongBreakLength);
        private int LongBreakAfter => SettingsService.Current.LongBreakAfter;

        public static PomodoroViewModel Current { get; } = new PomodoroViewModel();

        private TimeSpan _remainingPomodoroTime;
        private TimeSpan _remainingBreakTime;
        private bool _isInPomodoro;
        private bool _isTimerInProgress;
        private int _completedPomodoros;
        private IToastNotificationsService _toastNotificationsService;
        private bool _isStopByUser;

        private CountdownTimer _currentTimer;


        public PomodoroViewModel()
        {
            StartTimerCommand = new DelegateCommand(StartTimer);
            StopTimerCommand = new DelegateCommand(StopTimer);

            IsInPomodoro = true;
            RemainingPomodoroTime = PomodoroLength;
            TotalPomodoroTime = PomodoroLength;

            if (DesignMode.DesignMode2Enabled == false && App.Current is PrismUnityApplication)
                _toastNotificationsService = App.Current.Container.Resolve<IToastNotificationsService>();
        }

        public event EventHandler RemainingPomodoroTimeChanged;
        public event EventHandler RemainingBreakTimeChanged;
        public event EventHandler IsInPomodoroChanged;

        public TimeSpan RemainingPomodoroTime
        {
            get => _remainingPomodoroTime;

            private set
            {
                _remainingPomodoroTime = value;
                RemainingPomodoroTimeChanged?.Invoke(this, EventArgs.Empty);
                RaisePropertyChanged();
            }
        }

        public TimeSpan TotalPomodoroTime { get; private set; }

        public TimeSpan RemainingBreakTime
        {
            get => _remainingBreakTime;


            private set
            {
                _remainingBreakTime = value;
                RemainingBreakTimeChanged?.Invoke(this, EventArgs.Empty);
                RaisePropertyChanged();
            }
        }

        public TimeSpan TotalBreakTime { get; private set; }

        public bool IsInPomodoro
        {
            get => _isInPomodoro;

            private set
            {
                _isInPomodoro = value;
                IsInPomodoroChanged?.Invoke(this, EventArgs.Empty);
                RaisePropertyChanged();
            }
        }

        public bool IsTimerInProgress
        {
            get => _isTimerInProgress;

            private set
            {
                _isTimerInProgress = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand StartTimerCommand { get; }

        public DelegateCommand StopTimerCommand { get; }

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

        public int CompletedPomodoros
        {
            get { return _completedPomodoros; }
            set
            {
                _completedPomodoros = value;
                RaisePropertyChanged();
            }
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

        private void StartTimer()
        {
            if (_isInPomodoro)
                StartPomodoro();
            else
                StartBreak();
        }

        private void StopTimer()
        {
            if (_isInPomodoro)
                _toastNotificationsService.RemovePomodoroFinishedToastNotificationSchedule();
            else
                _toastNotificationsService.RemoveBreakFinishedToastNotificationSchedule();

            CurrentTimer.Stop();
        }

        private void StartPomodoro()
        {
            IsTimerInProgress = true;
            if (SettingsService.Current.IsNotifyWhenPomodoroFinished)
                _toastNotificationsService.AddPomodoroFinishedToastNotificationSchedule(DateTime.Now + RemainingPomodoroTime);

            if (SettingsService.Current.AutoStartOfNextPomodoro && CurrentTimer != null && CurrentTimer.RemainingTime == TimeSpan.Zero)
                CurrentTimer = new CountdownTimer(CurrentTimer.StartTime + CurrentTimer.TotalTime, PomodoroLength);
            else
                CurrentTimer = new CountdownTimer(DateTime.Now, PomodoroLength);

            CurrentTimer.Start();
        }

        private void StartBreak()
        {
            IsTimerInProgress = true;
            if (SettingsService.Current.IsNotifyWhenBreakFinished)
                _toastNotificationsService.AddBreakFinishedToastNotificationSchedule(DateTime.Now + RemainingBreakTime);

            var breakLength = (CompletedPomodoros % LongBreakAfter == 0) ? LongBreakLength : ShortBreakLength;
            if (SettingsService.Current.AutoStartOfNextPomodoro && CurrentTimer != null && CurrentTimer.RemainingTime == TimeSpan.Zero)
                CurrentTimer = new CountdownTimer(CurrentTimer.StartTime + CurrentTimer.TotalTime, breakLength);
            else
                CurrentTimer = new CountdownTimer(DateTime.Now, breakLength);

            CurrentTimer.Start();
        }

        private void OnPomodoroTimerFinished()
        {
            IsTimerInProgress = false;
            IsInPomodoro = false;
            CompletedPomodoros++;

            var breakLength = (CompletedPomodoros % LongBreakAfter == 0) ? LongBreakLength : ShortBreakLength;

            TotalBreakTime = breakLength;
            RemainingBreakTime = breakLength;

            if (SettingsService.Current.AutoStartOfBreak)
                StartBreak();
        }

        private void OnBreakTimerFinished()
        {
            RemainingPomodoroTime = PomodoroLength;
            IsTimerInProgress = false;
            IsInPomodoro = true;

            if (SettingsService.Current.AutoStartOfNextPomodoro)
                StartPomodoro();
        }
    }
}
