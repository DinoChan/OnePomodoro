using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePomodoro.Infrastructure
{
    public class AudioDefinition
    {
        public AudioDefinition(string name, Uri uri)
        {
            Name = name;
            Uri = uri;
        }

        public string Name { get; private set; }

        public Uri Uri { get; private set; }
    }

    public class AudioDefinitions
    {
        public static AudioDefinition[] Definitions { get; } = {
          new AudioDefinition("Gooey Button", new Uri("")),
          new AudioDefinition("Gooey Button", new Uri("")),
          new AudioDefinition("Gooey Button", new Uri("")),
          new AudioDefinition("Gooey Button", new Uri("")),
          new AudioDefinition("Gooey Button", new Uri("")),
          new AudioDefinition("Gooey Button", new Uri("")),
          new AudioDefinition("Gooey Button", new Uri("")),
          new AudioDefinition("Gooey Button", new Uri("")),

        };
    }
}
