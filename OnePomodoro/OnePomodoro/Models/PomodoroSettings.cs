using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;

namespace OnePomodoro.Models
{
    public class PomodoroSettings : ObservableObject, IPomodoroSettings
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
        private string _pomodoroAudioUri;
        private string _breakAudioUri;

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
                OnPropertyChanged();
            }
        }

        public bool AutoStartOfBreak
        {
            get { return _autoStartOfBreak; }
            set
            {
                _autoStartOfBreak = value;
                OnPropertyChanged();
            }
        }

        public bool IsNotifyWhenPomodoroFinished
        {
            get { return _isNotifyWhenPomodoroFinished; }
            set
            {
                _isNotifyWhenPomodoroFinished = value;
                OnPropertyChanged();
            }
        }

        public bool IsNotifyWhenBreakFinished
        {
            get { return _isNotifyWhenBreakFinished; }
            set
            {
                _isNotifyWhenBreakFinished = value;
                OnPropertyChanged();
            }
        }

        public int PomodoroLength
        {
            get { return _pomodoroLength; }
            set
            {
                _pomodoroLength = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }
        public int LongBreakLength
        {
            get { return _longBreakLength; }
            set
            {
                _longBreakLength = value;
                OnPropertyChanged();
            }
        }
        public int LongBreakAfter
        {
            get { return _longBreakAfter; }
            set
            {
                _longBreakAfter = value;
                OnPropertyChanged();
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

        public string PomodoroAudioUri
        {
            get
            {
                return _pomodoroAudioUri;
            }
            set
            {
                _pomodoroAudioUri = value;
                OnPropertyChanged();
            }
        }

        public string BreakAudioUri
        {
            get
            {
                return _breakAudioUri;
            }
            set
            {
                _breakAudioUri = value;
                OnPropertyChanged();
            }
        }
    }
}
