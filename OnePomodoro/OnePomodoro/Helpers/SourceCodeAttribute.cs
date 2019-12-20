using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
