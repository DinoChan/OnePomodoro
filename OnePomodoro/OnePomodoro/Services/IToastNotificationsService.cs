using System;
using Windows.UI.Notifications;

namespace OnePomodoro.Services
{
    internal interface IToastNotificationsService
    {
        void AddBreakFinishedToastNotificationSchedule(DateTime time);

        void AddPomodoroFinishedToastNotificationSchedule(DateTime time);

        void RemoveBreakFinishedToastNotificationSchedule();

        void RemovePomodoroFinishedToastNotificationSchedule();

        void ShowBreakFinishedToastNotification();

        void ShowPomodoroFinishedToastNotification();

        void ShowToastNotification(ToastNotification toastNotification);
    }
}
