using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Windows.System.Threading;
using Windows.UI.Xaml;

namespace OnePomodoro.Helpers
{
    public class CountdownTimer : BindableBase
    {

        private DispatcherTimer _innerTimer;
        private DateTime _timerStartTime;
        private TimeSpan _totalTime;
        private TimeSpan _remainingTime;

        public CountdownTimer(TimeSpan totalTime)
        {
            _totalTime = totalTime;
            _innerTimer = new DispatcherTimer();
            _innerTimer.Tick += OnInnerTimerTick;
            _innerTimer.Interval = TimeSpan.FromSeconds(0.1);

        }

        public event EventHandler Finished;
        public event EventHandler Elapsed;

        /// <summary>
        /// 获取或设置 RemainingInterval 的值
        /// </summary>
        public TimeSpan RemainingTime
        {
            get
            {
                return _remainingTime;
            }

            set
            {
                if (_remainingTime == value)
                    return;

                _remainingTime = value;
                RaisePropertyChanged();
                Elapsed?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Start()
        {
            _timerStartTime = DateTime.Now;
            _innerTimer.Start();
        }

        public void Stop()
        {
            if (_innerTimer.IsEnabled == false)
                return;

            _innerTimer.Stop();
            Finished?.Invoke(this, EventArgs.Empty);
        }

        private void OnInnerTimerTick(object sender, object e)
        {
            var remainingTime = _totalTime - (DateTime.Now - _timerStartTime);
            if (Math.Abs((remainingTime - RemainingTime).TotalSeconds) < 1)
                return;

            if (remainingTime < TimeSpan.Zero)
                remainingTime = TimeSpan.Zero;

            RemainingTime = remainingTime;
            if (RemainingTime == TimeSpan.Zero)
                Stop();
        }
    }
}
