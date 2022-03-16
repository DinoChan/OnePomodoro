using System;

namespace OnePomodoro.Helpers
{
    public class SourceCodeAttribute : Attribute
    {
        public SourceCodeAttribute(string sourceCodeUri)
        {
            SourceCodeUri = sourceCodeUri;
        }

        public string SourceCodeUri { get; }
    }
}
