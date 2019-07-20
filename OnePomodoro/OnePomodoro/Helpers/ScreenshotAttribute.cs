using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
