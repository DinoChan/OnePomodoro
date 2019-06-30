using Microsoft.Toolkit.Uwp.Notifications;
using System;

using Windows.UI.Notifications;

namespace OnePomodoro.Services
{
    internal partial class ToastNotificationsService : IToastNotificationsService
    {
        public void ShowToastNotification(ToastNotification toastNotification)
        {
            try
            {
                ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
            }
            catch (Exception)
            {
                // TODO WTS: Adding ToastNotification can fail in rare conditions, please handle exceptions as appropriate to your scenario.
            }
        }

        public void ShowPomodoroFinishedToastNotification()
        {
            var content = new ToastContent()
            {
                Launch = "ToastContentActivationParams",
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "Pomodoro has finished"
                            },

                            new AdaptiveText()
                            {
                                 Text = @"Pomodoro has finished,let's take a break."
                            }
                        }
                    }
                },
            };

            var toast = new ToastNotification(content.GetXml())
            {
                Tag = "ToastTag"
            };

            ShowToastNotification(toast);
        }

        public void ShowBreakFinishedToastNotification()
        {
            var content = new ToastContent()
            {
                Launch = "ToastContentActivationParams",
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "Break has ended"
                            },

                            new AdaptiveText()
                            {
                                 Text = @"Break has ended,it's time to work."
                            }
                        }
                    }
                },
            };

            var toast = new ToastNotification(content.GetXml())
            {
                Tag = "ToastTag"
            };

            ShowToastNotification(toast);
        }
    }
}
