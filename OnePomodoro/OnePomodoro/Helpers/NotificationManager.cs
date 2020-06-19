using OnePomodoro.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Notifications;

namespace OnePomodoro.Helpers
{
    public class NotificationManager
    {
        public static NotificationManager Current { get; } = new NotificationManager();

        public bool IsEnabled { get; set; }
        private ToastNotificationsService _toastNotificationsService;

        private List<ScheduledToastNotification> _notifications;

        private NotificationManager()
        {
            IsEnabled = true;
            _toastNotificationsService = new ToastNotificationsService();
            _notifications = new List<ScheduledToastNotification>();
        }

        public void AddPomodoroFinishedToastNotificationSchedule(DateTime time, bool isRemoveOthers = true)
        {
            if (IsEnabled == false)
                return;

            if (SettingsService.Current.IsNotifyWhenPomodoroFinished == false)
                return;

            var notification = _toastNotificationsService.AddPomodoroFinishedToastNotificationSchedule(time, SettingsService.Current.PomodoroAudioUri, isRemoveOthers);
            _notifications.Add(notification);
        }

        public void AddBreakFinishedToastNotificationSchedule(DateTime time, bool isRemoveOthers = true)
        {
            if (IsEnabled == false)
                return;

            if (SettingsService.Current.IsNotifyWhenBreakFinished == false)
                return;

            var notification = _toastNotificationsService.AddBreakFinishedToastNotificationSchedule(time, SettingsService.Current.BreakAudioUri, isRemoveOthers);
            _notifications.Add(notification);
        }

        public void RemovePomodoroFinishedToastNotificationSchedule(string id = null)
        {
            _toastNotificationsService.RemovePomodoroFinishedToastNotificationSchedule(id);
            RemoveFromNotifications(id);

        }

        public void RemoveBreakFinishedToastNotificationSchedule(string id = null)
        {
            _toastNotificationsService.RemoveBreakFinishedToastNotificationSchedule(id);
            RemoveFromNotifications(id);
        }

        public void RemoveAllNotificationsButFirst(bool isInPomodoro, DateTime startTime)
        {
            var notificationsToRemove = _notifications.Where(n => n.DeliveryTime < DateTime.Now).ToList();
            foreach (var notification in notificationsToRemove)
            {
                RemovePomodoroFinishedToastNotificationSchedule(notification.Id);
                RemoveBreakFinishedToastNotificationSchedule(notification.Id);
            }

            notificationsToRemove = _notifications.Skip(1).ToList();
            foreach (var notification in notificationsToRemove)
            {
                RemovePomodoroFinishedToastNotificationSchedule(notification.Id);
                RemoveBreakFinishedToastNotificationSchedule(notification.Id);
            }

            if (_notifications.Count == 0)
            {
                if (isInPomodoro)
                    AddPomodoroFinishedToastNotificationSchedule(startTime);
                else
                    AddBreakFinishedToastNotificationSchedule(startTime);
            }
        }

        public void AddAllNotifications(bool isInPomodoro, DateTime startTime, bool autoStartOfNextPomodoro, bool autoStartOfBreak,
           int completedPomodoros, int longBreakAfter, TimeSpan pomodoroLength, TimeSpan shortBreakLength, TimeSpan longBreakLength)
        {
            
            if (isInPomodoro)
                completedPomodoros++;

            int count = 0;
            while (count < 8)
            {
                isInPomodoro = isInPomodoro == false;
                if (isInPomodoro)
                {
                    if (autoStartOfNextPomodoro == false)
                        break;

                    startTime += pomodoroLength;
                    AddPomodoroFinishedToastNotificationSchedule(startTime, false);
                    completedPomodoros++;
                }
                else
                {
                    if (autoStartOfBreak == false)
                        break;

                    var breakLength = (completedPomodoros % longBreakAfter == 0) ? longBreakLength : shortBreakLength;
                    startTime += breakLength;
                    AddBreakFinishedToastNotificationSchedule(startTime, false);
                }

                count++;
            }
        }


        private void RemoveFromNotifications(string id = null)
        {
            if (string.IsNullOrWhiteSpace(id) == false)
            {
                var notification = _notifications.FirstOrDefault(n => n.Id == id);
                if (notification != null)
                    _notifications.Remove(notification);
            }
        }
    }
}
