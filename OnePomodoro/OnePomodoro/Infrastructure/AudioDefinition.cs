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
         new AudioDefinition("Default",string.Empty),
          new AudioDefinition("Alarm01", "ms-appx:///Assets/Media/Alarm01.mp3"),
          new AudioDefinition("Alarm02", "ms-appx:///Assets/Media/Alarm02.mp3"),
          new AudioDefinition("Alarm03", "ms-appx:///Assets/Media/Alarm03.mp3"),
          new AudioDefinition("Alarm04", "ms-appx:///Assets/Media/Alarm04.mp3"),
          new AudioDefinition("Alarm05", "ms-appx:///Assets/Media/Alarm05.mp3"),
          new AudioDefinition("Alarm06", "ms-appx:///Assets/Media/Alarm06.mp3"),
          new AudioDefinition("Alarm07", "ms-appx:///Assets/Media/Alarm07.mp3"),
          new AudioDefinition("Alarm08", "ms-appx:///Assets/Media/Alarm08.mp3"),
          new AudioDefinition("Alarm09", "ms-appx:///Assets/Media/Alarm09.mp3"),
          new AudioDefinition("Alarm10", "ms-appx:///Assets/Media/Alarm10.mp3"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/chimes.mp3"),
          new AudioDefinition("chord", "ms-appx:///Assets/Media/chord.mp3"),
          new AudioDefinition("ding", "ms-appx:///Assets/Media/ding.mp3"),
          new AudioDefinition("notify", "ms-appx:///Assets/Media/notify.mp3"),
          new AudioDefinition("recycle", "ms-appx:///Assets/Media/recycle.mp3"),
          new AudioDefinition("Ring01", "ms-appx:///Assets/Media/Ring01.mp3"),
          new AudioDefinition("Ring02", "ms-appx:///Assets/Media/Ring02.mp3"),
          new AudioDefinition("Ring03", "ms-appx:///Assets/Media/Ring03.mp3"),
          new AudioDefinition("Ring04", "ms-appx:///Assets/Media/Ring04.mp3"),
          new AudioDefinition("Ring05", "ms-appx:///Assets/Media/Ring05.mp3"),
          new AudioDefinition("Ring06", "ms-appx:///Assets/Media/Ring06.mp3"),
          new AudioDefinition("Ring07", "ms-appx:///Assets/Media/Ring07.mp3"),
          new AudioDefinition("Ring08", "ms-appx:///Assets/Media/Ring08.mp3"),
          new AudioDefinition("Ring09", "ms-appx:///Assets/Media/Ring09.mp3"),
          new AudioDefinition("Ring10", "ms-appx:///Assets/Media/Ring10.mp3"),
          new AudioDefinition("ringout", "ms-appx:///Assets/Media/ringout.mp3"),
          new AudioDefinition("Speech Disambiguation", "ms-appx:///Assets/Media/Speech Disambiguation.mp3"),
          new AudioDefinition("Speech Misrecognition", "ms-appx:///Assets/Media/Speech Misrecognition.mp3"),
          new AudioDefinition("Speech Off", "ms-appx:///Assets/Media/Speech Off.mp3"),
          new AudioDefinition("Speech Sleep", "ms-appx:///Assets/Media/Speech Sleep.mp3"),
          new AudioDefinition("tada", "ms-appx:///Assets/Media/tada.mp3"),
          new AudioDefinition("Windows Background", "ms-appx:///Assets/Media/Windows Background.mp3"),
          new AudioDefinition("Windows Balloon", "ms-appx:///Assets/Media/Windows Balloon.mp3"),
          new AudioDefinition("Windows Default", "ms-appx:///Assets/Media/Windows Default.mp3"),
          new AudioDefinition("Windows Ding", "ms-appx:///Assets/Media/Windows Ding.mp3"),
          new AudioDefinition("Windows Exclamation", "ms-appx:///Assets/Media/Windows Exclamation.mp3"),
          new AudioDefinition("Windows Feed Discovered", "ms-appx:///Assets/Media/Windows Feed Discovered.mp3"),
          new AudioDefinition("Windows Foreground", "ms-appx:///Assets/Media/Windows Foreground.mp3"),
          new AudioDefinition("Windows Logoff Sound", "ms-appx:///Assets/Media/Windows Logoff Sound.mp3"),
          new AudioDefinition("Windows Logon", "ms-appx:///Assets/Media/Windows Logon.mp3"),
          new AudioDefinition("Windows Message Nudge", "ms-appx:///Assets/Media/Windows Message Nudge.mp3"),
          new AudioDefinition("Windows Notify Calendar", "ms-appx:///Assets/Media/Windows Notify Calendar.mp3"),
          new AudioDefinition("Windows Notify Email", "ms-appx:///Assets/Media/Windows Notify Email.mp3"),
          new AudioDefinition("Windows Notify Messaging", "ms-appx:///Assets/Media/Windows Notify Messaging.mp3"),
          new AudioDefinition("Windows Notify System Generic", "ms-appx:///Assets/Media/Windows Notify System Generic.mp3"),
          new AudioDefinition("Windows Notify", "ms-appx:///Assets/Media/Windows Notify.mp3"),
          new AudioDefinition("Windows Pop-up Blocked", "ms-appx:///Assets/Media/Windows Pop-up Blocked.mp3"),
          new AudioDefinition("Windows Proximity Connection", "ms-appx:///Assets/Media/Windows Proximity Connection.mp3"),
          new AudioDefinition("Windows Proximity Notification", "ms-appx:///Assets/Media/Windows Proximity Notification.mp3"),
          new AudioDefinition("Windows Ringin", "ms-appx:///Assets/Media/Windows Ringin.mp3"),
          new AudioDefinition("Windows Ringout", "ms-appx:///Assets/Media/Windows Ringout.mp3"),
        };
    }
}
