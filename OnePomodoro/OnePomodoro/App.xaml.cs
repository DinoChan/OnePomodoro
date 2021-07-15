using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.DependencyInjection;
using OnePomodoro.Helpers;
using OnePomodoro.Services;
using OnePomodoro.Views;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Devices.Geolocation;
using Windows.Globalization;
using Windows.Services.Maps;
using Windows.UI.Core.Preview;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OnePomodoro
{
    [Windows.UI.Xaml.Data.Bindable]
    public sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            UnhandledException += (sender, e) =>
            {
                e.Handled = true;
                var errorMessage = e.Message + Environment.NewLine + e.Exception.StackTrace;
                ContentDialog dialog = new ContentDialog
                {
                    CloseButtonText = "Ok",
                    Content = errorMessage
                };

                _ = dialog.ShowAsync();
            };

            CoreApplication.Exiting += (s, e) =>
            {

            };


            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            AppCenter.Start("ba644924-74c7-432e-a7fa-e86442a1c601",
                typeof(Analytics), typeof(Crashes));

            Services = ConfigureServices();
        }

        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => (App)Application.Current;

        public bool HasExited { get; private set; }
        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider Services { get; }
        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
                    coreTitleBar.ExtendViewIntoTitleBar = true;
                    await DataService.CreateTheDatabaseAsync();
                    await DataService.RemoveFuturePeriodsAsync();
                    await ThemeSelectorService.InitializeAsync().ConfigureAwait(false);
                    await ThemeSelectorService.SetRequestedThemeAsync();

                    await SettingsService.InitializeAsync();
                    // 当导航堆栈尚未还原时，导航到第一页，
                    // 并通过将所需信息作为导航参数传入来配置
                    // 参数
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                    HandleClosed();
                    try
                    {
                        await NotificationManager.Current.RemoveBreakFinishedToastNotificationScheduleAsync();
                        await NotificationManager.Current.RemovePomodoroFinishedToastNotificationScheduleAsync();
                    }
                    catch (Exception ex)
                    {
                        Microsoft.AppCenter.Crashes.Crashes.TrackError(ex);
                    }

                    var properties = new Dictionary<string, string>
                    {
                        { "Region", Windows.System.UserProfile.GlobalizationPreferences.HomeGeographicRegion},
                    };
                    Analytics.TrackEvent("Launched", properties);
                }
                // 确保当前窗口处于活动状态
                Window.Current.Activate();


                await Services.GetService<IFirstRunDisplayService>().ShowIfAppropriateAsync();
                await SetCountryCode();
            }
        }

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IWhatsNewDisplayService, WhatsNewDisplayService>();
            services.AddSingleton<IFirstRunDisplayService, FirstRunDisplayService>();
            services.AddSingleton<ILiveTileService, LiveTileService>();

            return services.BuildServiceProvider();
        }

        private void AppDomainUnhandledException(object sender, System.UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
                Crashes.TrackError(ex);
        }

        private void HandleClosed()
        {
            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += async (s, args) =>
            {
                HasExited = true;
                var deferral = args.GetDeferral();
                await DataService.RemoveFuturePeriodsAsync();
                try
                {
                    NotificationManager.Current.IsEnabled = false;
                    await NotificationManager.Current.RemoveBreakFinishedToastNotificationScheduleAsync();
                    await NotificationManager.Current.RemovePomodoroFinishedToastNotificationScheduleAsync();
                }
                catch (Exception ex)
                {
                    Microsoft.AppCenter.Crashes.Crashes.TrackError(ex);
                }

                deferral.Complete();
            };
        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e) => Crashes.TrackError(e.Exception);

        private static async Task SetCountryCode()
        {
            // The following country code is used only as a fallback for the main implementation.
            // This fallback country code doesn't reflect the physical device location, but rather the
            // country that corresponds to the culture it uses.
            var countryCode = new GeographicRegion().CodeTwoLetter;
            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    var geoLocator = new Geolocator
                    {
                        DesiredAccuracyInMeters = 100
                    };
                    var position = await geoLocator.GetGeopositionAsync();
                    var myLocation = new BasicGeoposition
                    {
                        Longitude = position.Coordinate.Point.Position.Longitude,
                        Latitude = position.Coordinate.Point.Position.Latitude
                    };
                    var pointToReverseGeocode = new Geopoint(myLocation);
                    MapService.ServiceToken = "IMWjHP2McfAwNKq8DAIP~Vor0jubIiy8-IJwoSQbzBQ~AkMH8oQZioNtAHeNvv_rHBJoy_jLcTXtf5_RrpQtrauTdsCKQvMGkFho5Jf9mF-8";
                    var result = await MapLocationFinder.FindLocationsAtAsync(pointToReverseGeocode);
                    if (result.Status != MapLocationFinderStatus.Success || result.Locations == null || result.Locations.Count == 0)
                    {
                        break;
                    }

                    // The returned country code is in 3-letter format (ISO 3166-1 alpha-3).
                    // Below we convert it to ISO 3166-1 alpha-2 (two letter).
                    var country = result.Locations[0].Address.CountryCode;
                    countryCode = new GeographicRegion(country).CodeTwoLetter;
                    break;
                case GeolocationAccessStatus.Denied:
                    AppCenterLog.Info("Map", "Geolocation access denied. To set country code in App Center, enable location service in Windows 10.");
                    break;
                case GeolocationAccessStatus.Unspecified:
                    break;
            }
            AppCenter.SetCountryCode(countryCode);
        }
    }
}
