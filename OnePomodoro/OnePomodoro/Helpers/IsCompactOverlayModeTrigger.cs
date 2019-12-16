using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Gaming.XboxLive.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace OnePomodoro.Helpers
{
    public class IsCompactOverlayModeTrigger : StateTriggerBase
    {
        public IsCompactOverlayModeTrigger()
        {
            var view = ApplicationView.GetForCurrentView();
            SetActive(view.IsFullScreenMode);

            var content = Window.Current.Content as FrameworkElement;
            if (content == null)
                Window.Current.SizeChanged += OnWindowSIzeChanged;
            else
                content.SizeChanged += OnSizeChanged;
        }

        private void OnWindowSIzeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            var content = Window.Current.Content as FrameworkElement;
            content.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var view = ApplicationView.GetForCurrentView();
            SetActive(view.ViewMode == ApplicationViewMode.CompactOverlay);
        }




    }
}
