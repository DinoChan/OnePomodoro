using OnePomodoro.Helpers;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Streaming.Adaptive;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace OnePomodoro.Models
{
    public class PomodoroSettings : ViewModelBase, IPomodoroSettings
    {
        private string _viewType;
        private bool _autoStartOfNextPomodoro;
        private bool _autoStartOfBreak;
        private bool _isNotifyWhenPomodoroFinished;
        private bool _isNotifyWhenBreakFinished;
        private int _pomodoroLength;
        private int _shortBreakLength;
        private int _longBreakLength;
        private int _longBreakAfter;

        public PomodoroSettings()
        {
        }

        public event EventHandler ViewTypeChanged;

        public bool AutoStartOfNextPomodoro
        {
            get { return _autoStartOfNextPomodoro; }
            set
            {
                _autoStartOfNextPomodoro = value;
                RaisePropertyChanged();
            }
        }

        public bool AutoStartOfBreak
        {
            get { return _autoStartOfBreak; }
            set
            {
                _autoStartOfBreak = value;
                RaisePropertyChanged();
            }
        }

        public bool IsNotifyWhenPomodoroFinished
        {
            get { return _isNotifyWhenPomodoroFinished; }
            set
            {
                _isNotifyWhenPomodoroFinished = value;
                RaisePropertyChanged();
            }
        }

        public bool IsNotifyWhenBreakFinished
        {
            get { return _isNotifyWhenBreakFinished; }
            set
            {
                _isNotifyWhenBreakFinished = value;
                RaisePropertyChanged();
            }
        }

        public int PomodoroLength
        {
            get { return _pomodoroLength; }
            set
            {
                _pomodoroLength = value;
                RaisePropertyChanged();
            }
        }
        public int ShortBreakLength
        {
            get
            {
                return _shortBreakLength;
            }

            set
            {
                _shortBreakLength = value;
                RaisePropertyChanged();
            }
        }
        public int LongBreakLength
        {
            get { return _longBreakLength; }
            set
            {
                _longBreakLength = value;
                RaisePropertyChanged();
            }
        }
        public int LongBreakAfter
        {
            get { return _longBreakAfter; }
            set
            {
                _longBreakAfter = value;
                RaisePropertyChanged();
            }
        }

        public string ViewType
        {
            get { return _viewType; }
            set
            {
                if (_viewType == value)
                    return;

                _viewType = value;
                ViewTypeChanged?.Invoke(this, EventArgs.Empty);
            }
        }


    }
}
