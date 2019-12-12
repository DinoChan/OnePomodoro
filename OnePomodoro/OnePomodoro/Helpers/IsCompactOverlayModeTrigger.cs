using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace OnePomodoro.Helpers
{
    public class IsCompactOverlayModeTrigger : StateTriggerBase
    {
        public IsCompactOverlayModeTrigger()
        {
            var view = ApplicationView.GetForCurrentView();
            SetActive(view.IsFullScreenMode);
            (Window.Current.Content as FrameworkElement).SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var view = ApplicationView.GetForCurrentView();
            SetActive(view.ViewMode == ApplicationViewMode.CompactOverlay);
        }
    }
}
