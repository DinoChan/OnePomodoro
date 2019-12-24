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
        private FrameworkElement _targetElement;

        public FrameworkElement TargetElement
        {
            get
            {
                return _targetElement;
            }
            set
            {
                if (_targetElement != null)
                {
                    _targetElement.SizeChanged -= OnSizeChanged;
                }
                _targetElement = value;
                _targetElement.SizeChanged += OnSizeChanged;
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var view = ApplicationView.GetForCurrentView();
            SetActive(view.ViewMode == ApplicationViewMode.CompactOverlay);
        }




    }
}
