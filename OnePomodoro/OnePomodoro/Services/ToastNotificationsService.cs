using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Notifications;
using OnePomodoro.Helpers;
using Windows.Data.Xml.Dom;
using Windows.Foundation.Metadata;
using Windows.UI.Notifications;

namespace OnePomodoro.Services
{
    internal partial class ToastNotificationsService
    {
        private const string BreakTag = "Break";
        private const string PomodoroTag = "Pomodoro";
        private const string ToastGroup = "ToastGroup";
        private const string ToastTag = "ToastTag";
        private int _id;

        public ToastNotificationsService()
        {
            _id = 0;
        }

        public async Task<ScheduledToastNotification> AddBreakFinishedToastNotificationScheduleAsync(DateTime time, string audioUri, bool isRemoveOthers = true)
        {
            if (ApiInformation.IsTypePresent("Windows.ApplicationModel.Background.ToastNotificationActionTrigger") == false)
            {
                return null;
            }

            if (isRemoveOthers)
                await RemoveBreakFinishedToastNotificationScheduleAsync();

            var toastBuilder = CreateBreakToastBuilder();
            if (string.IsNullOrWhiteSpace(audioUri) == false)
                toastBuilder.AddAudio(new Uri(audioUri));

            XmlDocument xml = null;
            ScheduledToastNotification toast;
            if (time < DateTime.Now)
                return null;

            try
            {
                xml = toastBuilder.GetXml();
                var toastNotifier = await ToastNotificationToolkit.CreateToastNotifierAsync();
                if (time < DateTime.Now)
                    time = DateTime.Now.AddSeconds(3);

                toast = new ScheduledToastNotification(xml, time)
                {
                    Tag = BreakTag,
                    Group = ToastGroup,
                    ExpirationTime = time.AddHours(1),
                    Id = _id++.ToString()
                };

                toastNotifier.AddToSchedule(toast);
                Debug.WriteLine("add break:" + toast.Id);
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                    {
                        {"xml",  xml.GetXml()},
                        {"time",time.ToString() },
                        {"now",DateTime.Now.ToString() },
                        {"utc_now",DateTimeOffset.UtcNow.ToString() },
                    };
                Microsoft.AppCenter.Crashes.Crashes.TrackError(ex, properties);
                throw;
            }
            return toast;
        }

        public async Task<ScheduledToastNotification> AddPomodoroFinishedToastNotificationScheduleAsync(DateTime time, string audioUri, bool isRemoveOthers = true)
        {
            if (ApiInformation.IsTypePresent("Windows.ApplicationModel.Background.ToastNotificationActionTrigger") == false)
            {
                return null;
            }

            if (isRemoveOthers)
                await RemovePomodoroFinishedToastNotificationScheduleAsync();

            var toastBuilder = CreatePomodoroToastBuilder();
            if (string.IsNullOrWhiteSpace(audioUri) == false)
                toastBuilder.AddAudio(new Uri(audioUri));

            ScheduledToastNotification toast = null;
            XmlDocument xml = null;
            if (time < DateTime.Now)
                return null;

            try
            {
                xml = toastBuilder.GetXml();
                var toastNotifier = await ToastNotificationToolkit.CreateToastNotifierAsync();
                if (time < DateTime.Now)
                    time = DateTime.Now.AddSeconds(3);
                toast = new ScheduledToastNotification(xml, time)
                {
                    Tag = PomodoroTag,
                    Group = ToastGroup,
                    ExpirationTime = time.AddHours(1),
                    Id = _id++.ToString()
                };

                toastNotifier.AddToSchedule(toast);
                Debug.WriteLine("add pomodoro:" + toast.Id);
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                    {
                        {"xml",  xml.GetXml()},
                        {"time",time.ToString() },
                        {"now",DateTime.Now.ToString() },
                        {"utc_now",DateTimeOffset.UtcNow.ToString() },
                    };
                Microsoft.AppCenter.Crashes.Crashes.TrackError(ex, properties);
                throw;
            }

            return toast;
        }

        public async Task<IEnumerable<string>> RemoveBreakFinishedToastNotificationScheduleAsync(string id = null)
        {
            if (ApiInformation.IsTypePresent("Windows.ApplicationModel.Background.ToastNotificationActionTrigger") == false)
            {
                return new List<string>();
            }

            var notifier = await ToastNotificationToolkit.CreateToastNotifierAsync();
            List<string> result = new List<string>();
            foreach (var scheduledToast in notifier.GetScheduledToastNotifications())
            {
                if (scheduledToast.Group == ToastGroup && scheduledToast.Tag == BreakTag && (string.IsNullOrWhiteSpace(id) || scheduledToast.Id == id))
                {
                    notifier.RemoveFromSchedule(scheduledToast);
                    result.Add(scheduledToast.Id);
                    Debug.WriteLine("remove break:" + scheduledToast.Id);
                }
            }

            return result;
        }

        public async Task<IEnumerable<string>> RemovePomodoroFinishedToastNotificationScheduleAsync(string id = null)
        {
            if (ApiInformation.IsTypePresent("Windows.ApplicationModel.Background.ToastNotificationActionTrigger") == false)
            {
                return new List<string>();
            }

            var notifier = await ToastNotificationToolkit.CreateToastNotifierAsync();
            List<string> result = new List<string>();
            foreach (var scheduledToast in notifier.GetScheduledToastNotifications())
            {
                if (scheduledToast.Group == ToastGroup && scheduledToast.Tag == PomodoroTag && (string.IsNullOrWhiteSpace(id) || scheduledToast.Id == id))
                {
                    notifier.RemoveFromSchedule(scheduledToast);
                    result.Add(scheduledToast.Id);
                    Debug.WriteLine("remove pomodoro:" + scheduledToast.Id);
                }
            }

            return result;
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

        private static ToastContentBuilder CreateBreakToastBuilder()
        {
            return new ToastContentBuilder()
                  .SetToastScenario(ToastScenario.Alarm)
                  .AddAppLogoOverride(new Uri("ms-appx:///Assets/ToastAppLogo.png"))
                  .AddText("Break has ended")
                  .AddText(@"Break has ended, it's time to work.");
        }

        private static ToastContentBuilder CreatePomodoroToastBuilder()
        {
            return new ToastContentBuilder()
                  .SetToastScenario(ToastScenario.Alarm)
                  .AddAppLogoOverride(new Uri("ms-appx:///Assets/ToastAppLogo.png"))
                  .AddText("Pomodoro has finished")
                  .AddText(@"Pomodoro has finished, let's take a break.");
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
    }
}
