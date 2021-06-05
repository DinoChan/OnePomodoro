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
using Windows.UI.Xaml.Media.Animation;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using System.Collections.Generic;
using Microsoft.AppCenter.Analytics;

namespace OnePomodoro.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        private Type _pomodoroViewType;

        private bool _isInCompactOverlay;

        private CompactOverlayAttribute _currentCompactOverlayAttribute;

#warning need 1903
        //private List<TeachingTip> _teachingTip;

        private static bool _isShown = false;

        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            Window.Current.SizeChanged += OnWindowCurrentSizeChanged;
            //ChangePomodoroContent(typeof(TheFirst));
            Loaded += OnLoaded;
        }

        private bool DetectIfFirstRun()
        {
            return SystemInformation.Instance.IsFirstRun && !_isShown;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Windows.Foundation.Size(300, 300));
        }

        private void OnWindowCurrentSizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            UpdateButtonsVisibility();
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


            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("backAnimation");
            if (animation != null)
            {
                animation.TryStart(PomodoroContent);
            }
        }


        private void ChangePomodoroContent(Type type)
        {
            var properties = new Dictionary<string, string>
            {
                { "ViewType",SettingsService.Current.ViewType},
            };
            Analytics.TrackEvent("ChangePomodoroContent", properties);

            var properties2 = new Dictionary<string, string>
            {
                { "ViewType",SettingsService.Current.ViewType},
                { "Region", Windows.System.UserProfile.GlobalizationPreferences.HomeGeographicRegion},
            };
            Analytics.TrackEvent("ChangePomodoroContentCombination", properties2);
            var view = Activator.CreateInstance(type) as PomodoroView;
            PomodoroContent.Content = view;
            RequestedTheme = (view as FrameworkElement).RequestedTheme;

            var attributes = type.GetCustomAttributes(true);
            _currentCompactOverlayAttribute = attributes.OfType<CompactOverlayAttribute>().FirstOrDefault();
            UpdateButtonsVisibility();
        }


        private async void OnPinClick(object sender, RoutedEventArgs e)
        {
            var preferences = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
            preferences.CustomSize = new Windows.Foundation.Size(_currentCompactOverlayAttribute.CustomWidth, _currentCompactOverlayAttribute.CustomHeight);
            await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay, preferences);
            _isInCompactOverlay = true;
            UpdateButtonsVisibility();
        }

        private async void OnUnpinClick(object sender, RoutedEventArgs e)
        {
            var preferences = ViewModePreferences.CreateDefault(ApplicationViewMode.Default);
            await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.Default, preferences);
            _isInCompactOverlay = false;
            UpdateButtonsVisibility();
        }

        private void OnFullScreenClick(object sender, RoutedEventArgs e)
        {
            var view = ApplicationView.GetForCurrentView();
            view.TryEnterFullScreenMode();
        }

        private void UpdateButtonsVisibility()
        {
            var view = ApplicationView.GetForCurrentView();
            if (view.IsFullScreenMode)
            {
                FullScreenButton.Visibility = Visibility.Collapsed;
                PinButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (_isInCompactOverlay)
                {
                    FullScreenButton.Visibility = Visibility.Collapsed;
                    OptionsButton.Visibility = Visibility.Collapsed;
                    PinButton.Visibility = Visibility.Collapsed;
                    UnpinButton.Visibility = Visibility.Visible;
                }
                else
                {
                    FullScreenButton.Visibility = Visibility.Visible;
                    OptionsButton.Visibility = Visibility.Visible;
                    UnpinButton.Visibility = Visibility.Collapsed;
                    PinButton.Visibility = _currentCompactOverlayAttribute == null ? Visibility.Collapsed : Visibility.Visible;
                }

            }
        }
    }
}
