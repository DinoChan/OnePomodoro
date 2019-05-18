using System;
using System.Threading;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            get
            {
                return _remainingPomodoroInterval;
            }

            private set
            {
                _remainingPomodoroInterval = value;
                RaisePropertyChanged();
            }
        }

        public TimeSpan RemainingBreakingInterval
        {
            get
            {
                return _remainingBreakInterval;
            }


            private set
            {
                _remainingBreakInterval = value;
                RaisePropertyChanged();
            }
        }

        public bool IsInPomodoro
        {
            get
            {
                return _isInPomodoro;
            }

            private set
            {
                _isInPomodoro = value;
                RaisePropertyChanged();
            }
        }

        public bool IsTimerInProgress
        {
            get
            {
                return _isTimerInProgress;
            }

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
            _breakStartTime = DateTime.Now;
            _pomodoroTimer.Start();
            IsTimerInProgress = true;
        }
        private void StopPomodoro()
        {
            _pomodoroTimer.Stop();
            IsTimerInProgress = false;
            IsInPomodoro = false;
            RemainingPomodoroInterval = PomodoroInterval;

            _completedPomodoros++;
            
        }


        private void StartBreak()
        {
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
            throw new NotImplementedException();
        }

        private void OnBreakTimerTick(object sender, object e)
        {
            var remainingBreakingInterval= DateTime.Now - _breakStartTime;
            if (remainingBreakingInterval < TimeSpan.Zero)
                remainingBreakingInterval = TimeSpan.Zero;

            RemainingBreakingInterval = _completedPomodoros % LongBreakAfter == 0 ? BreakInterval : LongBreakInterval;

            RemainingBreakingInterval = remainingBreakingInterval;
            if (RemainingBreakingInterval == TimeSpan.Zero)
                StopBreak();


            
            _breakTimer.Interval = RemainingBreakingInterval;
        }
    }
}
