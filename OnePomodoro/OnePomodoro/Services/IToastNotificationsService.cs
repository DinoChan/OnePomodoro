using Windows.UI.Notifications;

namespace OnePomodoro.Services
{
    internal interface IToastNotificationsService
    {
        void ShowToastNotification(ToastNotification toastNotification);

        void ShowToastNotificationSample();
    }
}
