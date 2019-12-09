using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePomodoro.Helpers
{
    public class CompactOverlayAttribute : Attribute
    {
        public CompactOverlayAttribute()
        {
            CustomWidth = 200;
            CustomHeight = 200;
        }

        public double CustomWidth { get; set; }

        public double CustomHeight { get; set; }
    }
}
