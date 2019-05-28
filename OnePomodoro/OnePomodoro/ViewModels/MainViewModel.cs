using OnePomodoro.Helpers;
using Prism.Commands;
using Prism.Windows.Mvvm;
using System;
using Windows.UI.Xaml;

namespace OnePomodoro.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly TimeSpan PomodoroLength = TimeSpan.FromMinutes(25);
        private readonly TimeSpan BreakLength = TimeSpan.FromMinutes(5);
        private readonly TimeSpan LongBreakLength = TimeSpan.FromMinutes(15);
        private readonly int LongBreakAfter = 4;


        private CountdownTimer _pomodoroTimer;
        private CountdownTimer _breakTimer;
        private CountdownTimer _longBreakTimer;
        private CountdownTimer _currentBreakTimer;
        private TimeSpan _remainingPomodoroTime;
        private TimeSpan _remainingBreakTime;
        private bool _isInPomodoro;
        private bool _isTimerInProgress;
        private int _completedPomodoros;


        public MainViewModel()
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


        private void StartPomodoro()
        {
            _pomodoroTimer.Start();
            IsTimerInProgress = true;
        }
        private void StopPomodoro()
        {
            _pomodoroTimer.Stop();
            IsTimerInProgress = false;
            IsInPomodoro = false;
            _completedPomodoros++;
            RemainingBreakTime = (_completedPomodoros % LongBreakAfter == 0) ? LongBreakLength : BreakLength;
        }


        private void StartBreak()
        {
            _currentBreakTimer = (_completedPomodoros % LongBreakAfter == 0) ? _longBreakTimer :_breakTimer;
            _currentBreakTimer.Start();
            IsTimerInProgress = true;
        }


        private void StopBreak()
        {
            RemainingPomodoroTime = PomodoroLength;
            _currentBreakTimer.Stop();
            IsTimerInProgress = false;
            IsInPomodoro = true;
        }
       

        private void OnPomodoroTimerElapsed(object sender, EventArgs e)
        {
            RemainingPomodoroTime = _pomodoroTimer.RemainingTime;
        }

        private void OnPomodoroTimerFinished(object sender, EventArgs e)
        {
            StopTimer();
        }

        private void OnBreakTimerElapsed(object sender, EventArgs e)
        {
            RemainingBreakTime = _currentBreakTimer.RemainingTime;
        }

        private void OnBreakTimerFinished(object sender, EventArgs e)
        {
            StopTimer();
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
                StopPomodoro();
            else
                StopBreak();
        }

    }
}
