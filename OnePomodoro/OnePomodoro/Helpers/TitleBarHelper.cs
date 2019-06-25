using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OnePomodoro.Helpers
{
    public static class TitleBarHelper
    {
        public static void UpdatePageTitleColor(Page page)
        {
            UpdatePageTitleColor(page.ActualTheme);
        }

        public static void UpdatePageTitleColor(ElementTheme theme)
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            if (theme == ElementTheme.Dark)
            {
                titleBar.ButtonForegroundColor = Color.FromArgb(255, 242, 242, 242);
                titleBar.ButtonHoverForegroundColor = Colors.White;
                titleBar.ButtonPressedForegroundColor = Colors.White;
                titleBar.ButtonHoverBackgroundColor = Color.FromArgb(25, 255, 255, 255);
                titleBar.ButtonPressedBackgroundColor = Color.FromArgb(51, 255, 255, 255);
                titleBar.ButtonInactiveForegroundColor = Color.FromArgb(255, 102, 102, 102);
                titleBar.ForegroundColor = Colors.White;
            }
            else
            {
                titleBar.ButtonForegroundColor = Color.FromArgb(255, 23, 23, 23);
                titleBar.ButtonHoverForegroundColor = Colors.Black;
                titleBar.ButtonPressedForegroundColor = Colors.Black;
                titleBar.ButtonHoverBackgroundColor = Color.FromArgb(25, 00, 00, 00);
                titleBar.ButtonPressedBackgroundColor = Color.FromArgb(51, 00, 00, 00);
                titleBar.ButtonInactiveForegroundColor = Color.FromArgb(255, 153, 153, 153);
                titleBar.ForegroundColor = Colors.Black;
            }

            titleBar.BackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }

    }
}
