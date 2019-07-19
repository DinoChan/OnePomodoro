using System;
using Windows.UI.Notifications;

namespace OnePomodoro.Services
{
    internal interface IToastNotificationsService
    {
        void ShowToastNotification(ToastNotification toastNotification);

        void ShowPomodoroFinishedToastNotification();

        void ShowBreakFinishedToastNotification();

        void AddPomodoroFinishedToastNotificationSchedule(DateTime time);

        void AddBreakFinishedToastNotificationSchedule(DateTime time);

        void RemovePomodoroFinishedToastNotificationSchedule();

        void RemoveBreakFinishedToastNotificationSchedule();
    }
}
