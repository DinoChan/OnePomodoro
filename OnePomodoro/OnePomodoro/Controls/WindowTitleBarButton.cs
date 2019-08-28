using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OnePomodoro.Controls
{
    [TemplateVisualState(GroupName = ActivationStateName, Name = ActivatedStateName)]
    [TemplateVisualState(GroupName = ActivationStateName, Name = DeactivatedStateName)]
    public class WindowTitleBarButton : Button
    {
        private const string ActivationStateName = "ActivationStates";
        private const string ActivatedStateName = "Activated";
        private const string DeactivatedStateName = "Deactivated";


        public WindowTitleBarButton()
        {
            DefaultStyleKey = typeof(WindowTitleBarButton);

            CoreApplication.GetCurrentView().CoreWindow.Activated += (s, e) =>
            {
                if (e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated)
                    VisualStateManager.GoToState(this, DeactivatedStateName, true);
                else
                    VisualStateManager.GoToState(this, ActivatedStateName, true);
            };
        }


    }
}
