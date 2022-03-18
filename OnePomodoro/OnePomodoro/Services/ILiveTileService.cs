using System.Threading.Tasks;

using Windows.ApplicationModel.Activation;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

namespace OnePomodoro.Services
{
    internal interface ILiveTileService
    {
        bool CanHandleInternal(LaunchActivatedEventArgs args);

        Task EnableQueueAsync();

        Task HandleInternalAsync(LaunchActivatedEventArgs args);

        Task<bool> PinSecondaryTileAsync(SecondaryTile tile, bool allowDuplicity = false);

        Task SamplePinSecondaryAsync(string pageName);

        void SampleUpdate();

        void UpdateTile(TileNotification notification);
    }
}
