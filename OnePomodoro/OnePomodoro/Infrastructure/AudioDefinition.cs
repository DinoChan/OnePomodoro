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
            //new AudioDefinition("Default", @"ms-winsoundevent:Notification.Default"),
            //new AudioDefinition("IM", @"ms-winsoundevent:Notification.IM"),
            //new AudioDefinition("Mail", @"ms-winsoundevent:Notification.Mail"),
            //new AudioDefinition("Reminder", @"ms-winsoundevent:Notification.Reminder"),
            //new AudioDefinition("SMS", @"ms-winsoundevent:Notification.SMS"),
            //new AudioDefinition("Alarm", @"ms-winsoundevent:Notification.Looping.Alarm"),
            //new AudioDefinition("Alarm2", @"ms-winsoundevent:Notification.Looping.Alarm2"),
            //new AudioDefinition("Alarm3", @"ms-winsoundevent:Notification.Looping.Alarm3"),
            //new AudioDefinition("Alarm4", @"ms-winsoundevent:Notification.Looping.Alarm4"),
            //new AudioDefinition("Alarm5", @"ms-winsoundevent:Notification.Looping.Alarm5"),
            //new AudioDefinition("Alarm6", @"ms-winsoundevent:Notification.Looping.Alarm6"),
            //new AudioDefinition("Alarm7", @"ms-winsoundevent:Notification.Looping.Alarm7"),
            //new AudioDefinition("Alarm8", @"ms-winsoundevent:Notification.Looping.Alarm8"),
            //new AudioDefinition("Alarm9", @"ms-winsoundevent:Notification.Looping.Alarm9"),
            //new AudioDefinition("Alarm10", @"ms-winsoundevent:Notification.Looping.Alarm10"),
            //new AudioDefinition("Call", @"ms-winsoundevent:Notification.Looping.Call"),
            //new AudioDefinition("Call2", @"ms-winsoundevent:Notification.Looping.Call2"),
            //new AudioDefinition("Call3", @"ms-winsoundevent:Notification.Looping.Call3"),
            //new AudioDefinition("Call4", @"ms-winsoundevent:Notification.Looping.Call4"),
            //new AudioDefinition("Call5", @"ms-winsoundevent:Notification.Looping.Call5"),
            //new AudioDefinition("Call6", @"ms-winsoundevent:Notification.Looping.Call6"),
            //new AudioDefinition("Call7", @"ms-winsoundevent:Notification.Looping.Call7"),
            //new AudioDefinition("Call8", @"ms-winsoundevent:Notification.Looping.Call8"),
            //new AudioDefinition("Call9", @"ms-winsoundevent:Notification.Looping.Call9"),
            //new AudioDefinition("Call10", @"ms-winsoundevent:Notification.Looping.Call10"),

         new AudioDefinition("Default",string.Empty),
          new AudioDefinition("Alarm01", "ms-appx:///Assets/Media/Alarm01.wav"),
          new AudioDefinition("Alarm02", "ms-appx:///Assets/Media/Alarm02.wav"),
          new AudioDefinition("Alarm03", "ms-appx:///Assets/Media/Alarm03.wav"),
          new AudioDefinition("Alarm04", "ms-appx:///Assets/Media/Alarm04.wav"),
          new AudioDefinition("Alarm05", "ms-appx:///Assets/Media/Alarm05.wav"),
          new AudioDefinition("Alarm06", "ms-appx:///Assets/Media/Alarm06.wav"),
          new AudioDefinition("Alarm07", "ms-appx:///Assets/Media/Alarm07.wav"),
          new AudioDefinition("Alarm08", "ms-appx:///Assets/Media/Alarm08.wav"),
          new AudioDefinition("Alarm09", "ms-appx:///Assets/Media/Alarm09.wav"),
          new AudioDefinition("Alarm10", "ms-appx:///Assets/Media/Alarm10.wav"),
          new AudioDefinition("chimes", "ms-appx:///Assets/Media/chimes.wav"),
          new AudioDefinition("chord", "ms-appx:///Assets/Media/chord.wav"),
          new AudioDefinition("ding", "ms-appx:///Assets/Media/ding.wav"),
          new AudioDefinition("notify", "ms-appx:///Assets/Media/notify.wav"),
          new AudioDefinition("recycle", "ms-appx:///Assets/Media/recycle.wav"),
          new AudioDefinition("Ring01", "ms-appx:///Assets/Media/Ring01.wav"),
          new AudioDefinition("Ring02", "ms-appx:///Assets/Media/Ring02.wav"),
          new AudioDefinition("Ring03", "ms-appx:///Assets/Media/Ring03.wav"),
          new AudioDefinition("Ring04", "ms-appx:///Assets/Media/Ring04.wav"),
          new AudioDefinition("Ring05", "ms-appx:///Assets/Media/Ring05.wav"),
          new AudioDefinition("Ring06", "ms-appx:///Assets/Media/Ring06.wav"),
          new AudioDefinition("Ring07", "ms-appx:///Assets/Media/Ring07.wav"),
          new AudioDefinition("Ring08", "ms-appx:///Assets/Media/Ring08.wav"),
          new AudioDefinition("Ring09", "ms-appx:///Assets/Media/Ring09.wav"),
          new AudioDefinition("Ring10", "ms-appx:///Assets/Media/Ring10.wav"),
          new AudioDefinition("ringout", "ms-appx:///Assets/Media/ringout.wav"),
          new AudioDefinition("Speech Disambiguation", "ms-appx:///Assets/Media/Speech Disambiguation.wav"),
          new AudioDefinition("Speech Misrecognition", "ms-appx:///Assets/Media/Speech Misrecognition.wav"),
          new AudioDefinition("Speech Off", "ms-appx:///Assets/Media/Speech Off.wav"),
          new AudioDefinition("Speech Sleep", "ms-appx:///Assets/Media/Speech Sleep.wav"),
          new AudioDefinition("tada", "ms-appx:///Assets/Media/tada.wav"),
          new AudioDefinition("Windows Background", "ms-appx:///Assets/Media/Windows Background.wav"),
          new AudioDefinition("Windows Balloon", "ms-appx:///Assets/Media/Windows Balloon.wav"),
          new AudioDefinition("Windows Default", "ms-appx:///Assets/Media/Windows Default.wav"),
          new AudioDefinition("Windows Ding", "ms-appx:///Assets/Media/Windows Ding.wav"),
          new AudioDefinition("Windows Exclamation", "ms-appx:///Assets/Media/Windows Exclamation.wav"),
          new AudioDefinition("Windows Feed Discovered", "ms-appx:///Assets/Media/Windows Feed Discovered.wav"),
          new AudioDefinition("Windows Foreground", "ms-appx:///Assets/Media/Windows Foreground.wav"),
          new AudioDefinition("Windows Logoff Sound", "ms-appx:///Assets/Media/Windows Logoff Sound.wav"),
          new AudioDefinition("Windows Logon", "ms-appx:///Assets/Media/Windows Logon.wav"),
          new AudioDefinition("Windows Message Nudge", "ms-appx:///Assets/Media/Windows Message Nudge.wav"),
          new AudioDefinition("Windows Notify Calendar", "ms-appx:///Assets/Media/Windows Notify Calendar.wav"),
          new AudioDefinition("Windows Notify Email", "ms-appx:///Assets/Media/Windows Notify Email.wav"),
          new AudioDefinition("Windows Notify Messaging", "ms-appx:///Assets/Media/Windows Notify Messaging.wav"),
          new AudioDefinition("Windows Notify System Generic", "ms-appx:///Assets/Media/Windows Notify System Generic.wav"),
          new AudioDefinition("Windows Notify", "ms-appx:///Assets/Media/Windows Notify.wav"),
          new AudioDefinition("Windows Pop-up Blocked", "ms-appx:///Assets/Media/Windows Pop-up Blocked.wav"),
          new AudioDefinition("Windows Proximity Connection", "ms-appx:///Assets/Media/Windows Proximity Connection.wav"),
          new AudioDefinition("Windows Proximity Notification", "ms-appx:///Assets/Media/Windows Proximity Notification.wav"),
          new AudioDefinition("Windows Ringin", "ms-appx:///Assets/Media/Windows Ringin.wav"),
          new AudioDefinition("Windows Ringout", "ms-appx:///Assets/Media/Windows Ringout.wav"),
        };
    }
}
