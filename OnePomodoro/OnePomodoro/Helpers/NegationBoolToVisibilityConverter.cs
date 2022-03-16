using Microsoft.Toolkit.Uwp.UI.Converters;
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
