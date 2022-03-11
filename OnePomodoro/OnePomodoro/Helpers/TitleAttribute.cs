using System;

namespace OnePomodoro.Helpers
{
    public class TitleAttribute : Attribute
    {
        public TitleAttribute(string title)
        {
            Title = title;
        }

        public string Title { get; }
    }
}
