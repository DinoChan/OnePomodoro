using System;
using System.Globalization;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;
using OnePomodoro.Helpers;
using OnePomodoro.Services;

using Prism.Mvvm;
using Prism.Unity.Windows;
using Prism.Windows.AppModel;

using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Core.Preview;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OnePomodoro
{
    [Windows.UI.Xaml.Data.Bindable]
    public sealed partial class App : PrismUnityApplication
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

           
        }

        protected override void ConfigureContainer()
        {
            // register a singleton using Container.RegisterType<IInterface, Type>(new ContainerControlledLifetimeManager());
            base.ConfigureContainer();
            Container.RegisterType<IWhatsNewDisplayService, WhatsNewDisplayService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IFirstRunDisplayService, FirstRunDisplayService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ILiveTileService, LiveTileService>(new ContainerControlledLifetimeManager());
            Container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));
        }

        protected override async Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            await LaunchApplicationAsync(PageTokens.MainPage, null);
        }

        private async Task LaunchApplicationAsync(string page, object launchParam)
        {
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            await ThemeSelectorService.SetRequestedThemeAsync();
            NavigationService.Navigate(page, launchParam);
            Window.Current.Activate();
            
            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += (s, e) =>
            {
                NotificationManager.Current.IsEnabled = false;
                NotificationManager.Current.RemoveBreakFinishedToastNotificationSchedule();
                NotificationManager.Current.RemovePomodoroFinishedToastNotificationSchedule();
            };
            NotificationManager.Current.RemoveBreakFinishedToastNotificationSchedule();
            NotificationManager.Current.RemovePomodoroFinishedToastNotificationSchedule();
            //var dialog = new Views.FirstRunDialog();
            //await dialog.ShowAsync();
            //await Container.Resolve<IWhatsNewDisplayService>().ShowIfAppropriateAsync();
            await Container.Resolve<IFirstRunDisplayService>().ShowIfAppropriateAsync();
            //Container.Resolve<ILiveTileService>().SampleUpdate();
            //Container.Resolve<IToastNotificationsService>().ShowToastNotificationSample();
        }

        protected override async Task OnActivateApplicationAsync(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.ToastNotification && args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                // Handle a toast notification here
                // Since dev center, toast, and Azure notification hub will all active with an ActivationKind.ToastNotification
                // you may have to parse the toast data to determine where it came from and what action you want to take
                // If the app isn't running then launch the app here
                await OnLaunchApplicationAsync(args as LaunchActivatedEventArgs);
            }

            await Task.CompletedTask;
        }

        protected override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            await base.OnInitializeAsync(args);
            await ThemeSelectorService.InitializeAsync().ConfigureAwait(false);

            // We are remapping the default ViewNamePage and ViewNamePageViewModel naming to ViewNamePage and ViewNameViewModel to
            // gain better code reuse with other frameworks and pages within Windows Template Studio
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "OnePomodoro.ViewModels.{0}ViewModel, OnePomodoro", viewType.Name.Substring(0, viewType.Name.Length - 4));
                return Type.GetType(viewModelTypeName);
            });
            await Container.Resolve<ILiveTileService>().EnableQueueAsync().ConfigureAwait(false);
            await SettingsService.InitializeAsync();
        }

        protected override IDeviceGestureService OnCreateDeviceGestureService()
        {
            var service = base.OnCreateDeviceGestureService();
            service.UseTitleBarBackButton = false;
            return service;
        }

    }
}
