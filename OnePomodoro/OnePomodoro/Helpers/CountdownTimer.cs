using System;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Windows.UI.Xaml;

//https://docs.microsoft.com/zh-cn/windows/uwp/launch-resume/app-lifecycle
namespace OnePomodoro.Helpers
{
    public class CountdownTimer : ObservableObject
    {
        private DispatcherTimer _innerTimer;
        private TimeSpan _totalTime;
        private TimeSpan _remainingTime;

        public CountdownTimer(DateTime startTime, TimeSpan totalTime)
        {
            StartTime = startTime;
            _totalTime = totalTime;
            _innerTimer = new DispatcherTimer();
            _innerTimer.Tick += OnInnerTimerTick;
            _innerTimer.Interval = TimeSpan.FromSeconds(0.1);
        }

        public event EventHandler Finished;

        public event EventHandler Elapsed;

        public TimeSpan TotalTime => _totalTime;

        public DateTime StartTime { get; private set; }

        /// <summary>
        /// 获取或设置 RemainingInterval 的值
        /// </summary>
        public TimeSpan RemainingTime
        {
            get
            {
                return _remainingTime;
            }

            private set
            {
                if (_remainingTime == value)
                    return;

                _remainingTime = value;
                OnPropertyChanged();
                Elapsed?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Start()
        {
            _innerTimer.Start();
            CheckTime();
        }

        public void Stop()
        {
            _innerTimer.Stop();
            Finished?.Invoke(this, EventArgs.Empty);
        }

        private void OnInnerTimerTick(object sender, object e)
        {
            CheckTime();
        }

        public void CheckTime(bool updateRemainingTime = true)
        {
            var remainingTime = _totalTime - (DateTime.Now - StartTime);
            if (Math.Abs((remainingTime - RemainingTime).TotalSeconds) < 1)
                return;

            if (remainingTime < TimeSpan.Zero)
                remainingTime = TimeSpan.Zero;

            if (updateRemainingTime)
                RemainingTime = remainingTime;

            if (remainingTime == TimeSpan.Zero)
                Stop();
        }
    }
}
