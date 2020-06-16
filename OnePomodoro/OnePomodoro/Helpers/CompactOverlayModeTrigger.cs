using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace OnePomodoro.Helpers
{
    public class CompactOverlayModeTrigger : StateTriggerBase
    {
        public CompactOverlayModeTrigger()
        {
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                var weakEvent =
                    new WeakEventListener<CompactOverlayModeTrigger, ApplicationView, object>(this)
                    {
                        OnEventAction = (instance, source, eventArgs) => instance.CompactOverlayModeTrigger_VisibleBoundsChanged(source, eventArgs),
                        OnDetachAction = (weakEventListener) => ApplicationView.GetForCurrentView().VisibleBoundsChanged -= weakEventListener.OnEvent
                    };
                ApplicationView.GetForCurrentView().VisibleBoundsChanged += weakEvent.OnEvent;
            }
        }

        private void CompactOverlayModeTrigger_VisibleBoundsChanged(ApplicationView sender, object args) => UpdateTrigger(sender.ViewMode == ApplicationViewMode.CompactOverlay);

        private bool _isCompactOverlayMode;


        public bool IsCompactOverlayMode
        {
            get => _isCompactOverlayMode;
            set
            {
                _isCompactOverlayMode = value;
                if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                {
                    var isCompactOverlayMode = ApplicationView.GetForCurrentView().ViewMode== ApplicationViewMode.CompactOverlay;
                    UpdateTrigger(isCompactOverlayMode);
                }
            }
        }

        private void UpdateTrigger(bool isCompactOverlayMode) => SetActive(IsCompactOverlayMode == isCompactOverlayMode);
    }
}
