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
            CustomWidth = 150;
            CustomHeight = 150;
        }

        public double CustomWidth { get; set; }

        public double CustomHeight { get; set; }
    }
}
