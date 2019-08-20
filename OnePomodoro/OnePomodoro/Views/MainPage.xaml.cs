using System;
using OnePomodoro.PomodoroViews;
using OnePomodoro.Services;
using OnePomodoro.ViewModels;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Linq;
using Windows.UI.ViewManagement;

namespace OnePomodoro.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        private Type _pomodoroViewType;

        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            //ChangePomodoroContent(typeof(TheFirst));
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;

        }

        private void OnOptionsClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OptionsPage));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //Window.Current.SetTitleBar(null);

            var viewType = PomodoroView.Views.FirstOrDefault(t => t.Name == (SettingsService.Current.ViewType ?? string.Empty));

            if (viewType == null)
                viewType = PomodoroView.Views.FirstOrDefault();

            if (_pomodoroViewType != viewType)
                ChangePomodoroContent(viewType);

            _pomodoroViewType = viewType;

            Window.Current.SetTitleBar(AppTitleBar);
        }


        private void ChangePomodoroContent(Type type)
        {
            var view = Activator.CreateInstance(type) as PomodoroView;
            PomodoroContent.Content = view;
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            UpdateTitleBarLayout(sender);
        }

        private void UpdateTitleBarLayout(CoreApplicationViewTitleBar coreTitleBar)
        {
            AppTitleBar.Height = coreTitleBar.Height;
            AppButtonBar.Height = coreTitleBar.Height;
            //OptionsButton.Margin = new Thickness(0, 0, coreTitleBar.SystemOverlayRightInset, 0);
        }

        private async void OnPinClick(object sender, RoutedEventArgs e)
        {
            var preferences = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
            preferences.CustomSize = new Windows.Foundation.Size(200, 200);
            await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay, preferences);
            Frame.Navigate(typeof(CompactPage));
        }

        private void OnFullScreenClick(object sender, RoutedEventArgs e)
        {
            ApplicationView view = ApplicationView.GetForCurrentView();

            //bool isInFullScreenMode = view.IsFullScreenMode;

            //if (isInFullScreenMode)
            //{
            //    view.ExitFullScreenMode();
            //}
            //else
            //{
            view.TryEnterFullScreenMode();
            FullScreenButton.Visibility = Visibility.Collapsed;
            PinButton.Visibility = Visibility.Collapsed;
            BackToWindowButton.Visibility = Visibility.Visible;
            //}
        }

        private void OnBackToWindowClick(object sender, RoutedEventArgs e)
        {
            ApplicationView view = ApplicationView.GetForCurrentView();
            bool isInFullScreenMode = view.IsFullScreenMode;

            //if (isInFullScreenMode)
            //{
            view.ExitFullScreenMode();
            BackToWindowButton.Visibility = Visibility.Collapsed;
            FullScreenButton.Visibility = Visibility.Visible;
            PinButton.Visibility = Visibility.Visible;
        }
    }
}
