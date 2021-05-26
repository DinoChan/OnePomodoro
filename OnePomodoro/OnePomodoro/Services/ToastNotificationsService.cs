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

        private readonly XmlDocument PomodoroToastContent = GetPomodoroToastContent().GetXml();

        private readonly XmlDocument BreakToastContent = GetBreakToastContent().GetXml();

        private int _id;

        public ToastNotificationsService()
        {
            _id = 0;
        }

        private static ToastContent GetPomodoroToastContent()
        {
            //        new ToastContentBuilder()
            //   .AddArgument("action", "viewItemsDueToday")
            //   .AddText("ASTR 170B1")
            //   .AddText("You have 3 items due today!")
            //.Schedule(DateTime.Now.AddSeconds(5), toast =>
            // {
            //     toast.Tag = "18365";
            //     toast.Group = "ASTR 170B1";
            // });
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
                                 Text = @"Pomodoro has finished, let's take a break."
                            }
                        }
                    }
                },
            };

            return content;
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


            var toastContent = GetPomodoroToastContent();
            if (string.IsNullOrWhiteSpace(audioUri) == false)
            {
                toastContent.Audio = new ToastAudio()
                {
                    Src = new Uri(audioUri)
                };
                //("ms-appx:///Assets/Audio/CustomToastAudio.m4a")
            }


            var notifier = ToastNotificationManager.CreateToastNotifier();

            var toast = new ScheduledToastNotification(toastContent.GetXml(), time)
            {
                Tag = PomodoroTag,
                Group = ToastGroup,
                ExpirationTime = time.AddHours(1),
                Id = _id++.ToString()
            };

            notifier.AddToSchedule(toast);
            Debug.WriteLine("add pomodoro:" + toast.Id);
            return toast;
        }

        public ScheduledToastNotification AddBreakFinishedToastNotificationSchedule(DateTime time, string audioUri, bool isRemoveOthers = true)
        {
            if (isRemoveOthers)
                RemoveBreakFinishedToastNotificationSchedule();

            var toastContent = GetBreakToastContent();
            if (string.IsNullOrWhiteSpace(audioUri) == false)
            {
                toastContent.Audio = new ToastAudio()
                {
                    Src = new Uri(audioUri)
                };
                //("ms-appx:///Assets/Audio/CustomToastAudio.m4a")
            }

            var notifier = ToastNotificationManager.CreateToastNotifier();

            var toast = new ScheduledToastNotification(toastContent.GetXml(), time)
            {
                Tag = BreakTag,
                Group = ToastGroup,
                ExpirationTime = time.AddHours(1),
                Id = _id++.ToString()
            };

            notifier.AddToSchedule(toast);
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
