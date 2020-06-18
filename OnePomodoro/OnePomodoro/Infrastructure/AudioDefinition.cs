using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePomodoro.Infrastructure
{
    public class AudioDefinition
    {
        public AudioDefinition(string name, string uri)
        {
            Name = name;
            Uri = uri;
        }

        public string Name { get; private set; }

        public string Uri { get; private set; }
    }

    public class AudioDefinitions
    {
        public static AudioDefinition[] Definitions { get; } = {
         new AudioDefinition("None",null),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Alarm01.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Alarm02.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Alarm03.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Alarm04.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Alarm05.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Alarm06.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Alarm07.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Alarm08.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Alarm09.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Alarm10.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/chimes.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/chord.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/ding.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/notify.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/recycle.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Ring01.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Ring02.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Ring03.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Ring04.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Ring05.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Ring06.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Ring07.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Ring08.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Ring09.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Ring10.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/ringout.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Speech Disambiguation.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Speech Misrecognition.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Speech Off.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Speech Sleep.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/tada.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Windows Background.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Windows Balloon.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Windows Default.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Windows Ding.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Windows Exclamation.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Windows Feed Discovered.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Windows Foreground.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Windows Logoff Sound.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Windows Logon.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Windows Message Nudge.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Windows Notify Calendar.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Windows Notify Email.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Windows Notify Messaging.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Windows Notify System Generic.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Windows Notify.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Windows Pop-up Blocked.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Windows Proximity Connection.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Windows Proximity Notification.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Windows Ringin.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/Windows Ringout.wav"),
        };
    }
}
