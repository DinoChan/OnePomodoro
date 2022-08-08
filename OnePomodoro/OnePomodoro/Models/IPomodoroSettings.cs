using System;

namespace OnePomodoro.Models
{
    public interface IPomodoroSettings
    {
        event EventHandler ViewTypeChanged;

        bool AutoStartOfBreak { get; set; }
        bool AutoStartOfNextPomodoro { get; set; }
        string BreakAudioUri { get; set; }
        bool IsNotifyWhenBreakFinished { get; set; }
        bool IsNotifyWhenPomodoroFinished { get; set; }
        int LongBreakAfter { get; set; }
        int LongBreakLength { get; set; }
        string PomodoroAudioUri { get; set; }
        int PomodoroLength { get; set; }

        int ShortBreakLength { get; set; }
        string ViewType { get; set; }
    }
}
