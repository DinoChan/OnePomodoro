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
    public class PomodoroSettings : ViewModelBase, ISettings
    {
      

        public PomodoroSettings()
        {
        }

     

        public bool AutoStartOfNextPomodoro { get; set; }

        public bool AutoStartOfBreak { get; set; }

        public bool IsNotifyWhenPomodoroFinished { get; set; }

        public bool IsNotifyWhenBreakFinished { get; set; }


      
    }
}
