using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace OnePomodoro.Controls
{
    public class InnerContentControl : ContentControl
    {
        public event EventHandler Measuring;

        protected override Size MeasureOverride(Size constraint)
        {
            //if (IsMeasureValid == false)
            Measuring?.Invoke(this, EventArgs.Empty);

            return base.MeasureOverride(constraint);
        }
    }
}
