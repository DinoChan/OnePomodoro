using Microsoft.Toolkit.Uwp.UI.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace OnePomodoro.Helpers
{
    public class NegationBoolToVisibilityConverter : BoolToObjectConverter
    {
        public NegationBoolToVisibilityConverter()
        {
            base.TrueValue = Visibility.Collapsed;
            base.FalseValue = Visibility.Visible;
        }
    }
}
