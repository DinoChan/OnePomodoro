using System;

namespace OnePomodoro.Helpers
{
    public class CompactOverlayAttribute : Attribute
    {
        public CompactOverlayAttribute()
        {
            CustomWidth = 150;
            CustomHeight = 150;
        }

        public double CustomHeight { get; set; }
        public double CustomWidth { get; set; }
    }
}
