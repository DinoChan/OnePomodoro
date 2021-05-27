using Microsoft.Toolkit.Uwp.Notifications;
using System;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace OnePomodoro.Services
{
    internal partial class ToastNotificationsService
    {
        private const string ToastTag = "ToastTag";
        private const string ToastGroup = "ToastGroup";
        private const string PomodoroTag = "Pomodoro";
        private const string BreakTag = "Break";



        private int _id;

        public ToastNotificationsService()
        {
            _id = 0;
        }

        private static ToastContentBuilder CreatePomodoroToastBuilder()
        {
            return new ToastContentBuilder()
                  .SetToastScenario(ToastScenario.Alarm)
                  .AddText("Pomodoro has finished")
                  .AddText(@"Pomodoro has finished, let's take a break.");
        }

        private static ToastContentBuilder CreateBreakToastBuilder()
        {
            return new ToastContentBuilder()
                  .SetToastScenario(ToastScenario.Alarm)
                  .AddText("Break has ended")
                  .AddText(@"Break has ended, it's time to work.");
        }




        private static ToastContent GetBreakToastContent()
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
                                 Text = @"Break has ended, it's time to work."
                            }
                        }
                    }
                },
            };

            return content;
        }

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


        public ScheduledToastNotification AddPomodoroFinishedToastNotificationSchedule(DateTime time, string audioUri, bool isRemoveOthers = true)
        {
            if (isRemoveOthers)
                RemovePomodoroFinishedToastNotificationSchedule();

            var toastBuilder = CreatePomodoroToastBuilder();
            if (string.IsNullOrWhiteSpace(audioUri) == false)
                toastBuilder.AddAudio(new Uri(audioUri));

            var toast = new ScheduledToastNotification(toastBuilder.GetXml(), time)
            {
                Tag = PomodoroTag,
                Group = ToastGroup,
                ExpirationTime = time.AddHours(1),
                Id = _id++.ToString()
            };

            ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
            Debug.WriteLine("add pomodoro:" + toast.Id);
            return toast;
        }

        public ScheduledToastNotification AddBreakFinishedToastNotificationSchedule(DateTime time, string audioUri, bool isRemoveOthers = true)
        {
            if (isRemoveOthers)
                RemoveBreakFinishedToastNotificationSchedule();

            var toastBuilder = CreatePomodoroToastBuilder();
            if (string.IsNullOrWhiteSpace(audioUri) == false)
                toastBuilder.AddAudio(new Uri(audioUri));

            var toast = new ScheduledToastNotification(toastBuilder.GetXml(), time)
            {
                Tag = BreakTag,
                Group = ToastGroup,
                ExpirationTime = time.AddHours(1),
                Id = _id++.ToString()
            };

            ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
            Debug.WriteLine("add break:" + toast.Id);
            return toast;
        }

        public IEnumerable<string> RemovePomodoroFinishedToastNotificationSchedule(string id = null)
        {
            var notifier = ToastNotificationManager.CreateToastNotifier();
            foreach (var scheduledToast in notifier.GetScheduledToastNotifications())
            {
                if (scheduledToast.Group == ToastGroup && scheduledToast.Tag == PomodoroTag && (string.IsNullOrWhiteSpace(id) || scheduledToast.Id == id))
                {
                    notifier.RemoveFromSchedule(scheduledToast);
                    yield return scheduledToast.Id;
                    Debug.WriteLine("remove pomodoro:" + scheduledToast.Id);
                }
            }
        }

        public IEnumerable<string> RemoveBreakFinishedToastNotificationSchedule(string id = null)
        {
            var notifier = ToastNotificationManager.CreateToastNotifier();
            foreach (var scheduledToast in notifier.GetScheduledToastNotifications())
            {
                if (scheduledToast.Group == ToastGroup && scheduledToast.Tag == BreakTag && (string.IsNullOrWhiteSpace(id) || scheduledToast.Id == id))
                {
                    notifier.RemoveFromSchedule(scheduledToast);
                    yield return scheduledToast.Id;
                    Debug.WriteLine("remove break:" + scheduledToast.Id);
                }
            }
        }
    }
}
