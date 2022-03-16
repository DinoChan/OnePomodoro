using System;

namespace OnePomodoro.Helpers
{
    public class ScreenshotAttribute : Attribute
    {
        public ScreenshotAttribute(string uri)
        {
            Uri = uri;
        }

        public string Uri { get; }
    }
}
