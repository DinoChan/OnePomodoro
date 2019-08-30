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
using OnePomodoro.Helpers;

namespace OnePomodoro.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        private Type _pomodoroViewType;

        private bool _isInCompactMode;

        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            Window.Current.SizeChanged += OnWindowCurrentSizeChanged;
            //ChangePomodoroContent(typeof(TheFirst));
        }

        private void OnWindowCurrentSizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            var view = ApplicationView.GetForCurrentView();
            if (view.IsFullScreenMode)
            {
                FullScreenButton.Visibility = Visibility.Collapsed;
                PinButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                FullScreenButton.Visibility = Visibility.Visible;
                PinButton.Visibility = Visibility.Visible;
            }
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

            if (_pomodoroViewType != viewType || _isInCompactMode)
                ChangePomodoroContent(viewType);

            _pomodoroViewType = viewType;
            _isInCompactMode = false;
        }


        private void ChangePomodoroContent(Type type)
        {
            var view = Activator.CreateInstance(type) as PomodoroView;
            PomodoroContent.Content = view;
            RequestedTheme = (view as FrameworkElement).RequestedTheme;
        }


        private async void OnPinClick(object sender, RoutedEventArgs e)
        {
            var preferences = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
            preferences.CustomSize = new Windows.Foundation.Size(200, 200);
            await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay, preferences);
            Frame.Navigate(typeof(CompactPage));
            _isInCompactMode = true;
        }

        private void OnFullScreenClick(object sender, RoutedEventArgs e)
        {
            ApplicationView view = ApplicationView.GetForCurrentView();
            view.TryEnterFullScreenMode();
        }
    }
}
