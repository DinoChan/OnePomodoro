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
        private readonly TimeSpan PomodoroLength = TimeSpan.FromMinutes(0.1);
        private readonly TimeSpan BreakLength = TimeSpan.FromMinutes(0.1);
        private readonly TimeSpan LongBreakLength = TimeSpan.FromMinutes(0.1);
        private readonly int LongBreakAfter = 4;

        public static PomodoroViewModel Current { get; } = new PomodoroViewModel();

        private CountdownTimer _pomodoroTimer;
        private CountdownTimer _breakTimer;
        private CountdownTimer _longBreakTimer;
        private CountdownTimer _currentBreakTimer;
        private TimeSpan _remainingPomodoroTime;
        private TimeSpan _remainingBreakTime;
        private bool _isInPomodoro;
        private bool _isTimerInProgress;
        private int _completedPomodoros;
        private IToastNotificationsService _toastNotificationsService;


        public PomodoroViewModel()
        {
            StartTimerCommand = new DelegateCommand(StartTimer);
            StopTimerCommand = new DelegateCommand(StopTimer);

            IsInPomodoro = true;
            RemainingPomodoroTime = PomodoroLength;
            _pomodoroTimer = new CountdownTimer(PomodoroLength);
            _pomodoroTimer.Elapsed += OnPomodoroTimerElapsed;
            _pomodoroTimer.Finished += OnPomodoroTimerFinished;

            _breakTimer = new CountdownTimer(BreakLength);
            _breakTimer.Elapsed += OnBreakTimerElapsed;
            _breakTimer.Finished += OnBreakTimerFinished;

            _longBreakTimer = new CountdownTimer(LongBreakLength);
            _longBreakTimer.Elapsed += OnBreakTimerElapsed;
            _longBreakTimer.Finished += OnBreakTimerFinished;

            if (DesignMode.DesignMode2Enabled == false && App.Current is PrismUnityApplication)
                _toastNotificationsService = App.Current.Container.Resolve<IToastNotificationsService>();
        }

        public TimeSpan RemainingPomodoroTime
        {
            get => _remainingPomodoroTime;

            private set
            {
                _remainingPomodoroTime = value;
                RaisePropertyChanged();
            }
        }

        public TimeSpan RemainingBreakTime
        {
            get => _remainingBreakTime;


            private set
            {
                _remainingBreakTime = value;
                RaisePropertyChanged();
            }
        }

        public bool IsInPomodoro
        {
            get => _isInPomodoro;

            private set
            {
                _isInPomodoro = value;
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
            {
                _toastNotificationsService.RemovePomodoroFinishedToastNotificationSchedule();
                StopPomodoro();
            }
            else
            {
                _toastNotificationsService.RemoveBreakFinishedToastNotificationSchedule();
                StopBreak();
            }
        }

        private void StartPomodoro()
        {
            _pomodoroTimer.Start();
            IsTimerInProgress = true;
            if (SettingsService.Current.IsNotifyWhenPomodoroFinished)
                _toastNotificationsService.AddPomodoroFinishedToastNotificationSchedule(DateTime.Now + RemainingPomodoroTime);
        }
        private void StopPomodoro()
        {
            _pomodoroTimer.Stop();
        }

        private void StartBreak()
        {
            _currentBreakTimer.Start();
            IsTimerInProgress = true;
            if (SettingsService.Current.IsNotifyWhenBreakFinished)
                _toastNotificationsService.AddBreakFinishedToastNotificationSchedule(DateTime.Now + RemainingBreakTime);
        }

        private void StopBreak()
        {
            _currentBreakTimer.Stop();
        }

        private void OnPomodoroTimerElapsed(object sender, EventArgs e)
        {
            RemainingPomodoroTime = _pomodoroTimer.RemainingTime;
        }

        private void OnPomodoroTimerFinished(object sender, EventArgs e)
        {
            IsTimerInProgress = false;
            IsInPomodoro = false;
            _completedPomodoros++;
            _currentBreakTimer = (_completedPomodoros % LongBreakAfter == 0) ? _longBreakTimer : _breakTimer;
            RemainingBreakTime = _currentBreakTimer.TotalTime;

            if (SettingsService.Current.AutoStartOfBreak)
                StartBreak();
        }

        private void OnBreakTimerElapsed(object sender, EventArgs e)
        {
            RemainingBreakTime = _currentBreakTimer.RemainingTime;
        }

        private void OnBreakTimerFinished(object sender, EventArgs e)
        {
            RemainingPomodoroTime = PomodoroLength;
            IsTimerInProgress = false;
            IsInPomodoro = true;

            if (SettingsService.Current.AutoStartOfNextPomodoro)
                StartPomodoro();
        }
    }
}
