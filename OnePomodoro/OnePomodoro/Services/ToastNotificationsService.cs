using Microsoft.Toolkit.Uwp.Notifications;
using System;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using System.Linq;
using System.Diagnostics;

namespace OnePomodoro.Services
{
    internal partial class ToastNotificationsService
    {
        private const string ToastTag = "ToastTag";
        private const string PomodoroGroup = "Pomodoro";
        private const string BreakGroup = "Break";
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

        public void ShowPomodoroFinishedToastNotification()
        {
            var toast = new ToastNotification(PomodoroToastContent)
            {
                Tag = ToastTag
            };

            ShowToastNotification(toast);
        }

        public void ShowBreakFinishedToastNotification()
        {
            var toast = new ToastNotification(BreakToastContent)
            {
                Tag = ToastTag
            };

            ShowToastNotification(toast);
        }

        public ScheduledToastNotification AddPomodoroFinishedToastNotificationSchedule(DateTime time,string audioUri, bool isRemoveOthers = true)
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
                Tag = ToastTag,
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
                Tag = ToastTag,
                ExpirationTime = time.AddHours(1),
                Id = _id++.ToString()
            };

            notifier.AddToSchedule(toast);
            Debug.WriteLine("add break:" + toast.Id);
            return toast;
        }

        public void RemovePomodoroFinishedToastNotificationSchedule(string id = null)
        {
            var notifier = ToastNotificationManager.CreateToastNotifier();
            foreach (var scheduledToast in notifier.GetScheduledToastNotifications())
            {
                if (scheduledToast.Content.InnerText == PomodoroToastContent.InnerText && (string.IsNullOrWhiteSpace(id) || scheduledToast.Id == id))
                {
                    notifier.RemoveFromSchedule(scheduledToast);
                    Debug.WriteLine("remove pomodoro:" + scheduledToast.Id);
                }
            }
        }

        public void RemoveBreakFinishedToastNotificationSchedule(string id = null)
        {
            var notifier = ToastNotificationManager.CreateToastNotifier();
            foreach (var scheduledToast in notifier.GetScheduledToastNotifications())
            {
                if (scheduledToast.Content.InnerText == BreakToastContent.InnerText && (string.IsNullOrWhiteSpace(id) || scheduledToast.Id == id))
                {
                    notifier.RemoveFromSchedule(scheduledToast);
                    Debug.WriteLine("remove break:" + scheduledToast.Id);
                }
            }
        }
    }
}
