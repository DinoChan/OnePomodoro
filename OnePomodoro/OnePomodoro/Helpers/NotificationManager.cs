using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnePomodoro.Services;
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

        public async Task AddPomodoroFinishedToastNotificationScheduleAsync(DateTime time, bool isRemoveOthers = true)
        {
            if (IsEnabled == false)
                return;

            if (SettingsService.Current.IsNotifyWhenPomodoroFinished == false)
                return;

            var notification = await _toastNotificationsService.AddPomodoroFinishedToastNotificationScheduleAsync(time, SettingsService.Current.PomodoroAudioUri, isRemoveOthers);
            if (notification != null)
                _notifications.Add(notification);
        }

        public async Task AddBreakFinishedToastNotificationScheduleAsync(DateTime time, bool isRemoveOthers = true)
        {
            if (IsEnabled == false)
                return;

            if (SettingsService.Current.IsNotifyWhenBreakFinished == false)
                return;

            var notification = await _toastNotificationsService.AddBreakFinishedToastNotificationScheduleAsync(time, SettingsService.Current.BreakAudioUri, isRemoveOthers);
            if (notification != null)
                _notifications.Add(notification);
        }

        public async Task RemovePomodoroFinishedToastNotificationScheduleAsync(string id = null)
        {
            var removedIds = await _toastNotificationsService.RemovePomodoroFinishedToastNotificationScheduleAsync(id);
            foreach (var removedId in removedIds)
            {
                RemoveFromNotifications(removedId);
            }
            RemoveFromNotifications(id);
        }

        public async Task RemoveBreakFinishedToastNotificationScheduleAsync(string id = null)
        {
            var removedIds = await _toastNotificationsService.RemoveBreakFinishedToastNotificationScheduleAsync(id);
            foreach (var removedId in removedIds)
            {
                RemoveFromNotifications(removedId);
            }
            RemoveFromNotifications(id);
        }

        public async Task RemoveAllNotificationsButFirstAsync(bool isInPomodoro, DateTime startTime)
        {
            var notificationsToRemove = _notifications.Where(n => n.DeliveryTime < DateTime.Now).ToList();
            foreach (var notification in notificationsToRemove)
            {
                await RemovePomodoroFinishedToastNotificationScheduleAsync(notification.Id);
                await RemoveBreakFinishedToastNotificationScheduleAsync(notification.Id);
            }

            notificationsToRemove = _notifications.Skip(1).ToList();
            foreach (var notification in notificationsToRemove)
            {
                await RemovePomodoroFinishedToastNotificationScheduleAsync(notification.Id);
                await RemoveBreakFinishedToastNotificationScheduleAsync(notification.Id);
            }

            if (_notifications.Count == 0)
            {
                if (isInPomodoro)
                    await AddPomodoroFinishedToastNotificationScheduleAsync(startTime);
                else
                    await AddBreakFinishedToastNotificationScheduleAsync(startTime);
            }
        }

        public async Task AddAllNotificationsAsync(bool isInPomodoro, DateTime startTime, bool autoStartOfNextPomodoro, bool autoStartOfBreak,
           int completedPomodoros, int longBreakAfter, TimeSpan pomodoroLength, TimeSpan shortBreakLength, TimeSpan longBreakLength)
        {
            if (isInPomodoro)
                completedPomodoros++;

            int count = 0;
            while (count < 16)
            {
                isInPomodoro = isInPomodoro == false;
                if (isInPomodoro)
                {
                    if (autoStartOfNextPomodoro == false)
                        break;

                    startTime += pomodoroLength;
                    await AddPomodoroFinishedToastNotificationScheduleAsync(startTime, false);
                    completedPomodoros++;
                }
                else
                {
                    if (autoStartOfBreak == false)
                        break;

                    var breakLength = (completedPomodoros % longBreakAfter == 0) ? longBreakLength : shortBreakLength;
                    startTime += breakLength;
                    await AddBreakFinishedToastNotificationScheduleAsync(startTime, false);
                }

                count++;
            }
        }

        private void RemoveFromNotifications(string id = null)
        {
            lock (_notifications)
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
}
