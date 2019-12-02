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

        public PomodoroSettings()
        {
        }

        public event EventHandler ViewTypeChanged;

        public bool AutoStartOfNextPomodoro { get; set; }

        public bool AutoStartOfBreak { get; set; }

        public bool IsNotifyWhenPomodoroFinished { get; set; }

        public bool IsNotifyWhenBreakFinished { get; set; }

        public int PomodoroLength { get; set; }
        public int ShortBreakLength { get; set; }
        public int LongBreakLength { get; set; }
        public int LongBreakAfter { get; set; }

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
