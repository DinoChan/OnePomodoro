using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePomodoro.Models
{
    public interface IPomodoroSettings
    {
        bool AutoStartOfNextPomodoro { get; set; }

        bool AutoStartOfBreak { get; set; }

        bool IsNotifyWhenPomodoroFinished { get; set; }

        bool IsNotifyWhenBreakFinished { get; set; }

        int PomodoroLength { get; set; }

        int ShortBreakLength { get; set; }

        int LongBreakLength { get; set; }

        int LongBreakAfter { get; set; }

        string ViewType { get; set; }

        event EventHandler ViewTypeChanged;

        string PomodoroAudioUri { get; set; }

        string BreakAudioUri { get; set; }
    }
}
