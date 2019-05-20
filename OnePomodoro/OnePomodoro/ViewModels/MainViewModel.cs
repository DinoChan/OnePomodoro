using Prism.Commands;
using Prism.Windows.Mvvm;
using System;
using Windows.UI.Xaml;

namespace OnePomodoro.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly TimeSpan PomodoroInterval = TimeSpan.FromMinutes(25);
        private readonly TimeSpan BreakInterval = TimeSpan.FromMinutes(5);
        private readonly TimeSpan LongBreakInterval = TimeSpan.FromMinutes(15);
        private readonly int LongBreakAfter = 4;


        private DispatcherTimer _pomodoroTimer;
        private DispatcherTimer _breakTimer;
        private TimeSpan _remainingPomodoroInterval;
        private TimeSpan _remainingBreakInterval;
        private bool _isInPomodoro;
        private bool _isTimerInProgress;
        private int _completedPomodoros;
        private DateTime _pomodoroStartTime;
        private DateTime _breakStartTime;


        public MainViewModel()
        {
            StartPomodoroCommand = new DelegateCommand(StartPomodoro);
            StopPomodoroCommand = new DelegateCommand(StopPomodoro);
            StartBreakCommand = new DelegateCommand(StartBreak);
            StopBreakCommand = new DelegateCommand(StopBreak);

            IsInPomodoro = true;
            RemainingPomodoroInterval = PomodoroInterval;
            _pomodoroTimer = new DispatcherTimer();
            _pomodoroTimer.Interval = PomodoroInterval;
            _pomodoroTimer.Tick += OnPomodoroTimerTick;

            _breakTimer = new DispatcherTimer();
            _breakTimer.Tick += OnBreakTimerTick;
        }

        public TimeSpan RemainingPomodoroInterval
        {
            get => _remainingPomodoroInterval;

            private set
            {
                _remainingPomodoroInterval = value;
                RaisePropertyChanged();
            }
        }

        public TimeSpan RemainingBreakInterval
        {
            get => _remainingBreakInterval;


            private set
            {
                _remainingBreakInterval = value;
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

        public DelegateCommand StartPomodoroCommand { get; }

        public DelegateCommand StopPomodoroCommand { get; }

        public DelegateCommand StartBreakCommand { get; }

        public DelegateCommand StopBreakCommand { get; }


        private void StartPomodoro()
        {
            RemainingBreakInterval = _completedPomodoros % LongBreakAfter == 0 ? BreakInterval : LongBreakInterval; ;
            _pomodoroStartTime = DateTime.Now;
            _pomodoroTimer.Start();
            IsTimerInProgress = true;
        }
        private void StopPomodoro()
        {
            _pomodoroTimer.Stop();
            IsTimerInProgress = false;
            IsInPomodoro = false;
            _completedPomodoros++;
        }


        private void StartBreak()
        {
            RemainingPomodoroInterval = PomodoroInterval;
            _breakStartTime = DateTime.Now;
            _breakTimer.Start();
            IsTimerInProgress = true;
        }


        private void StopBreak()
        {
            _breakTimer.Stop();
            IsTimerInProgress = false;
            IsInPomodoro = true;
        }

        private void OnPomodoroTimerTick(object sender, object e)
        {
            var remainingBreakInterval = PomodoroInterval - (DateTime.Now - _pomodoroStartTime);
            if (remainingBreakInterval < TimeSpan.Zero)
                remainingBreakInterval = TimeSpan.Zero;

            RemainingPomodoroInterval = remainingBreakInterval;
            if (RemainingBreakInterval == TimeSpan.Zero)
                StopPomodoro();
        }

        private void OnBreakTimerTick(object sender, object e)
        {
            var breakInterval = _completedPomodoros % LongBreakAfter == 0 ? BreakInterval : LongBreakInterval;
            TimeSpan remainingBreakInterval = breakInterval - (DateTime.Now - _breakStartTime);
            if (remainingBreakInterval < TimeSpan.Zero)
                remainingBreakInterval = TimeSpan.Zero;

            RemainingBreakInterval = remainingBreakInterval;
            if (RemainingBreakInterval <= TimeSpan.Zero){
                StopBreak();}
        }
    }
}
