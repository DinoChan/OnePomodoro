using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePomodoro.Models
{
    public interface ISettings
    {
        bool AutoStartOfNextPomodoro { get; set; }

        bool AutoStartOfBreak { get; set; }

        bool IsNotifyWhenPomodoroFinished { get; set; }

        bool IsNotifyWhenBreakFinished { get; set; }
    }
}
