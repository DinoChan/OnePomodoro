using Microsoft.Toolkit.Uwp.Notifications;
using System;

using Windows.UI.Notifications;

namespace OnePomodoro.Services
{
    internal partial class ToastNotificationsService : IToastNotificationsService
    {
        private const string ToastTag = "ToastTag";
        private const string PomodoroGroup = "Pomodoro";
        private const string BreakGroup = "Break";
        private const string ToastGroup = "ToastGroup";
        private const string PomodoroTag = "Pomodoro";
        private const string BreakTag = "Break";

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
                Tag = ToastTag
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
                Tag = ToastTag
            };

            ShowToastNotification(toast);
        }

        public void AddPomodoroFinishedToastNotificationSchedule(DateTime time)
        {
            RemovePomodoroFinishedToastNotificationSchedule();
            var notifier = ToastNotificationManager.CreateToastNotifier();

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


            var toast = new ScheduledToastNotification(content.GetXml(), time)
            {
                Tag = ToastTag,
              
            };

            notifier.AddToSchedule(toast);
        }

        public void AddBreakFinishedToastNotificationSchedule(DateTime time)
        {
            RemoveBreakFinishedToastNotificationSchedule();
            var notifier = ToastNotificationManager.CreateToastNotifier();

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


            var toast = new ScheduledToastNotification(content.GetXml(), time)
            {
                Tag = ToastTag,
               
            };

            notifier.AddToSchedule(toast);
        }

        public void RemovePomodoroFinishedToastNotificationSchedule()
        {
            var notifier = ToastNotificationManager.CreateToastNotifier();
            foreach (var scheduledToast in notifier.GetScheduledToastNotifications())
            {
                if (scheduledToast.Id == PomodoroGroup)
                    notifier.RemoveFromSchedule(scheduledToast);
            }
        }

        public void RemoveBreakFinishedToastNotificationSchedule()
        {
            var notifier = ToastNotificationManager.CreateToastNotifier();
            foreach (var scheduledToast in notifier.GetScheduledToastNotifications())
            {
                if (scheduledToast.Id == BreakGroup)
                    notifier.RemoveFromSchedule(scheduledToast);
            }
        }
    }
}
