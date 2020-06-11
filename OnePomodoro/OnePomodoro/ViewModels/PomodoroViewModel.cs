using OnePomodoro.Helpers;
using OnePomodoro.Services;
using Prism.Commands;
using Prism.Windows.Mvvm;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;

namespace OnePomodoro.ViewModels
{
    /// <summary>
    /// 2019年12月5日：添加了自动开始功能。
    /// 但最小化后程序进入后台模式Timer不能运行。但是……
    ///
    /// 最小化后，用户看不到程序，他怎么知道下一个番茄钟有自动开始运行？
    /// 最小化后，用户看不到程序，他怎么知道下一个番茄钟没有自动开始运行？
    /// 应用怎么知道用户知道或者不知道下一个番茄钟有开始或者没有开始运行？
    ///
    /// 所以在退出后台模式时更新一下就好了，不需要后台任务，简单就好。
    /// </summary>
    public class PomodoroViewModel : ViewModelBase
    {
        private TimeSpan PomodoroLength =>  TimeSpan.FromMinutes(SettingsService.Current == null ? 25 : SettingsService.Current.PomodoroLength);
        private TimeSpan ShortBreakLength =>  TimeSpan.FromMinutes(SettingsService.Current == null ? 5 : SettingsService.Current.ShortBreakLength);
        private TimeSpan LongBreakLength => TimeSpan.FromMinutes(SettingsService.Current == null ? 15 : SettingsService.Current.LongBreakLength);
        private int LongBreakAfter => SettingsService.Current == null ? 4 : SettingsService.Current.LongBreakAfter;

        public static PomodoroViewModel Current { get; } = new PomodoroViewModel();

        private TimeSpan _remainingPomodoroTime;
        private TimeSpan _remainingBreakTime;
        private bool _isInPomodoro;
        private bool _isTimerInProgress;
        private int _completedPomodoros;

        private CountdownTimer _currentTimer;


        public PomodoroViewModel()
        {
            StartTimerCommand = new DelegateCommand(StartTimer);
            StopTimerCommand = new DelegateCommand(StopTimer);

            IsInPomodoro = true;
            RemainingPomodoroTime = PomodoroLength;
            TotalPomodoroTime = PomodoroLength;

            CoreApplication.LeavingBackground += OnLeavingBackground;
            CoreApplication.EnteredBackground += OnEnteredBackground;
        }

        private void OnEnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            if (IsTimerInProgress)
                NotificationManager.Current.AddAllNotifications(IsInPomodoro, CurrentTimer.StartTime + CurrentTimer.TotalTime,
                 SettingsService.Current.AutoStartOfNextPomodoro, SettingsService.Current.AutoStartOfBreak,
                 _completedPomodoros, LongBreakAfter, PomodoroLength,
                 ShortBreakLength, LongBreakLength);
        }

        private void OnLeavingBackground(object sender, LeavingBackgroundEventArgs e)
        {
            if (CurrentTimer != null && IsTimerInProgress)
                CurrentTimer.CheckTime();

            if (IsTimerInProgress)
                NotificationManager.Current.RemoveAllNotificationsButFirst(IsInPomodoro, CurrentTimer.StartTime + CurrentTimer.TotalTime);
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
            NotificationManager.Current.RemovePomodoroFinishedToastNotificationSchedule();
            NotificationManager.Current.RemoveBreakFinishedToastNotificationSchedule();

            CurrentTimer.Stop();
        }

        private void StartPomodoro()
        {
            IsTimerInProgress = true;


            if (SettingsService.Current.AutoStartOfNextPomodoro && CurrentTimer != null && CurrentTimer.RemainingTime == TimeSpan.Zero)
                CurrentTimer = new CountdownTimer(CurrentTimer.StartTime + CurrentTimer.TotalTime, PomodoroLength);
            else
                CurrentTimer = new CountdownTimer(DateTime.Now, PomodoroLength);

            if (SettingsService.Current.IsNotifyWhenPomodoroFinished && (CurrentTimer.StartTime + CurrentTimer.TotalTime) > DateTime.Now)
                NotificationManager.Current.AddPomodoroFinishedToastNotificationSchedule(CurrentTimer.StartTime + CurrentTimer.TotalTime);

            CurrentTimer.Start();
        }

        private void StartBreak()
        {
            IsTimerInProgress = true;


            var breakLength = (CompletedPomodoros % LongBreakAfter == 0) ? LongBreakLength : ShortBreakLength;
            if (SettingsService.Current.AutoStartOfBreak && CurrentTimer != null && CurrentTimer.RemainingTime == TimeSpan.Zero)
                CurrentTimer = new CountdownTimer(CurrentTimer.StartTime + CurrentTimer.TotalTime, breakLength);
            else
                CurrentTimer = new CountdownTimer(DateTime.Now, breakLength);

            if (SettingsService.Current.IsNotifyWhenBreakFinished && (CurrentTimer.StartTime + CurrentTimer.TotalTime) > DateTime.Now)
                NotificationManager.Current.AddBreakFinishedToastNotificationSchedule(CurrentTimer.StartTime + CurrentTimer.TotalTime);

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
