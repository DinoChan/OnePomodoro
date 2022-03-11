using System;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace OnePomodoro.Controls
{
    public sealed partial class TitleBar : UserControl
    {
        private UISettings _uiSettings;
        private AccessibilitySettings _accessibilitySettings;
        private CoreApplicationViewTitleBar _coreTitleBar;

        public TitleBar()
        {
            this.InitializeComponent();
            _uiSettings = new UISettings();
            _accessibilitySettings = new AccessibilitySettings();
            _coreTitleBar = CoreApplication.GetCurrentView().TitleBar;

            _coreTitleBar.ExtendViewIntoTitleBar = true;

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            Buttons.CollectionChanged += OnButtonsCollectionChanged;
        }

        public bool IsTitleVisibile { get; set; }

        public ObservableCollection<Button> Buttons { get; } = new ObservableCollection<Button>();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SetTitleBar(BackgroundElement);
            // Register events
            _coreTitleBar.IsVisibleChanged += OnIsVisibleChanged;
            _coreTitleBar.LayoutMetricsChanged += OnLayoutMetricsChanged;

            _uiSettings.ColorValuesChanged += OnColorValuesChanged;
            _accessibilitySettings.HighContrastChanged += OnHighContrastChanged;
            Window.Current.Activated += OnWindowActivated;
            // Set properties
            LayoutRoot.Height = _coreTitleBar.Height;
            SetTitleBarControlColors();

            SetTitleBarVisibility();
            SetTitleBarPadding();

            if (IsTitleVisibile == false)
                AppName.Visibility = Visibility.Collapsed;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            // Unregister events
            _coreTitleBar.LayoutMetricsChanged -= OnLayoutMetricsChanged;
            _coreTitleBar.IsVisibleChanged -= OnIsVisibleChanged;
            _uiSettings.ColorValuesChanged -= OnColorValuesChanged;
            _accessibilitySettings.HighContrastChanged -= OnHighContrastChanged;
            Window.Current.Activated -= OnWindowActivated;
        }

        private void SetTitleBarVisibility()
        {
            LayoutRoot.Visibility = _coreTitleBar.IsVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SetTitleBarPadding()
        {
            double leftAddition = 0;
            double rightAddition = 0;

            if (FlowDirection == FlowDirection.LeftToRight)
            {
                leftAddition = _coreTitleBar.SystemOverlayLeftInset;
                rightAddition = _coreTitleBar.SystemOverlayRightInset;
            }
            else
            {
                leftAddition = _coreTitleBar.SystemOverlayRightInset;
                rightAddition = _coreTitleBar.SystemOverlayLeftInset;
            }

            LayoutRoot.Padding = new Thickness(leftAddition, 0, rightAddition, 0);
        }

        private void OnIsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            SetTitleBarVisibility();
        }

        private void OnLayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            LayoutRoot.Height = _coreTitleBar.Height;
            SetTitleBarPadding();
        }

        private async void OnColorValuesChanged(UISettings sender, Object e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { SetTitleBarControlColors(); });
        }

        private void SetTitleBarControlColors()
        {
            var applicationView = ApplicationView.GetForCurrentView();
            if (applicationView == null)
                return;

            var applicationTitleBar = applicationView.TitleBar;
            if (applicationTitleBar == null)
                return;

            if (_accessibilitySettings.HighContrast)
            {
                // Reset to use default colors.
                applicationTitleBar.ButtonBackgroundColor = null;
                applicationTitleBar.ButtonForegroundColor = null;
                applicationTitleBar.ButtonInactiveBackgroundColor = null;
                applicationTitleBar.ButtonInactiveForegroundColor = null;
                applicationTitleBar.ButtonHoverBackgroundColor = null;
                applicationTitleBar.ButtonHoverForegroundColor = null;
                applicationTitleBar.ButtonPressedBackgroundColor = null;
                applicationTitleBar.ButtonPressedForegroundColor = null;
            }
            else
            {
                Color bgColor = Colors.Transparent;
                //Color fgColor = ((SolidColorBrush)Application.Current.Resources["SystemControlPageTextBaseHighBrush"]).Color;
                //Color inactivefgColor = ((SolidColorBrush)Application.Current.Resources["SystemControlForegroundChromeDisabledLowBrush"]).Color;
                //Color hoverbgColor = ((SolidColorBrush)Application.Current.Resources["SystemControlBackgroundListLowBrush"]).Color;
                //Color hoverfgColor = ((SolidColorBrush)Application.Current.Resources["SystemControlForegroundBaseHighBrush"]).Color;
                //Color pressedbgColor = ((SolidColorBrush)Application.Current.Resources["SystemControlBackgroundListMediumBrush"]).Color;
                //Color pressedfgColor = ((SolidColorBrush)Application.Current.Resources["SystemControlForegroundBaseHighBrush"]).Color;

                Color fgColor = ((SolidColorBrush)Resources["ButtonForegroundColor"]).Color;
                Color inactivefgColor = ((SolidColorBrush)Resources["ButtonInactiveForegroundBrush"]).Color;
                Color hoverbgColor = ((SolidColorBrush)Resources["ButtonHoverBackgroundBrush"]).Color;
                Color hoverfgColor = ((SolidColorBrush)Resources["ButtonHoverForegroundBrush"]).Color;
                Color pressedbgColor = ((SolidColorBrush)Resources["ButtonPressedBackgroundBrush"]).Color;
                Color pressedfgColor = ((SolidColorBrush)Resources["ButtonPressedForegroundBrush"]).Color;
                applicationTitleBar.ButtonBackgroundColor = bgColor;
                applicationTitleBar.ButtonForegroundColor = fgColor;
                applicationTitleBar.ButtonInactiveBackgroundColor = bgColor;
                applicationTitleBar.ButtonInactiveForegroundColor = inactivefgColor;
                applicationTitleBar.ButtonHoverBackgroundColor = hoverbgColor;
                applicationTitleBar.ButtonHoverForegroundColor = hoverfgColor;
                applicationTitleBar.ButtonPressedBackgroundColor = pressedbgColor;
                applicationTitleBar.ButtonPressedForegroundColor = pressedfgColor;
            }
        }

        private async void OnHighContrastChanged(AccessibilitySettings sender, Object args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                SetTitleBarControlColors();
                SetTitleBarVisibility();
            });
        }

        private void OnWindowActivated(Object sender, WindowActivatedEventArgs e)
        {
            VisualStateManager.GoToState(
                this, e.WindowActivationState == CoreWindowActivationState.Deactivated ? WindowNotFocused.Name : WindowFocused.Name, false);
        }

        private void OnButtonsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (ItemsPanel == null)
                return;

            ItemsPanel.Children.Clear();
            foreach (var button in Buttons)
            {
                ItemsPanel.Children.Add(button);
            }
        }
    }
}
