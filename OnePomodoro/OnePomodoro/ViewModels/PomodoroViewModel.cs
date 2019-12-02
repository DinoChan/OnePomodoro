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
        private TimeSpan PomodoroLength => TimeSpan.FromMinutes(SettingsService.Current.PomodoroLength);
        private TimeSpan ShortBreakLength => TimeSpan.FromMinutes(SettingsService.Current.ShortBreakLength);
        private TimeSpan LongBreakLength => TimeSpan.FromMinutes(SettingsService.Current.LongBreakLength);
        private int LongBreakAfter => SettingsService.Current.LongBreakAfter;

        public static PomodoroViewModel Current { get; } = new PomodoroViewModel();

        private TimeSpan _remainingPomodoroTime;
        private TimeSpan _remainingBreakTime;
        private bool _isInPomodoro;
        private bool _isTimerInProgress;
        private int _completedPomodoros;
        private IToastNotificationsService _toastNotificationsService;

        private CountdownTimer _currentTimer;


        public PomodoroViewModel()
        {
            StartTimerCommand = new DelegateCommand(StartTimer);
            StopTimerCommand = new DelegateCommand(StopTimer);

            IsInPomodoro = true;
            RemainingPomodoroTime = PomodoroLength;

            CurrentTimer = new CountdownTimer(DateTime.Now, PomodoroLength);

            if (DesignMode.DesignMode2Enabled == false && App.Current is PrismUnityApplication)
                _toastNotificationsService = App.Current.Container.Resolve<IToastNotificationsService>();

            TotalPomodoroTime = PomodoroLength;
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
            CurrentTimer.Start();
        }

        private void StopTimer()
        {
            if (_isInPomodoro)
            {
                _toastNotificationsService.RemovePomodoroFinishedToastNotificationSchedule();
            }
            else
            {
                _toastNotificationsService.RemoveBreakFinishedToastNotificationSchedule();
            }

            CurrentTimer.Stop();
        }

        private void StartPomodoro()
        {

            CurrentTimer.Start();
            IsTimerInProgress = true;
            if (SettingsService.Current.IsNotifyWhenPomodoroFinished)
                _toastNotificationsService.AddPomodoroFinishedToastNotificationSchedule(DateTime.Now + RemainingPomodoroTime);
        }
        private void StartBreak()
        {
            CurrentTimer.Start();
            IsTimerInProgress = true;
            if (SettingsService.Current.IsNotifyWhenBreakFinished)
                _toastNotificationsService.AddBreakFinishedToastNotificationSchedule(DateTime.Now + RemainingBreakTime);
        }

        private void OnPomodoroTimerFinished()
        {
            IsTimerInProgress = false;
            IsInPomodoro = false;
            _completedPomodoros++;

            var breakLength = (_completedPomodoros % LongBreakAfter == 0) ? ShortBreakLength : LongBreakLength;
            if (SettingsService.Current.AutoStartOfNextPomodoro)
                CurrentTimer = new CountdownTimer(DateTime.Now, breakLength);
            else
                CurrentTimer = new CountdownTimer(CurrentTimer.StartTime + CurrentTimer.TotalTime, breakLength);

            TotalBreakTime = breakLength;
            RemainingBreakTime = breakLength;

            if (SettingsService.Current.AutoStartOfBreak)
                StartBreak();
        }

        private void OnBreakTimerFinished()
        {
            if (SettingsService.Current.AutoStartOfNextPomodoro)
                CurrentTimer = new CountdownTimer(DateTime.Now, PomodoroLength);
            else
                CurrentTimer = new CountdownTimer(CurrentTimer.StartTime + CurrentTimer.TotalTime, PomodoroLength);

            RemainingPomodoroTime = PomodoroLength;
            IsTimerInProgress = false;
            IsInPomodoro = true;

            if (SettingsService.Current.AutoStartOfNextPomodoro)
                StartPomodoro();
        }
    }
}
