using System;
using OnePomodoro.PomodoroViews;
using OnePomodoro.ViewModels;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OnePomodoro.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            ChangePomodoroContent(typeof(TheFirst));
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
            Window.Current.SetTitleBar(AppTitleBar);
        }


        private void ChangePomodoroContent(Type type)
        {
           var view= Activator.CreateInstance(type) as PomodoroView;
            PomodoroContent.Content = view;
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            UpdateTitleBarLayout(sender);
        }

        private void UpdateTitleBarLayout(CoreApplicationViewTitleBar coreTitleBar)
        {
            AppTitleBar.Height = coreTitleBar.Height;
            OptionsButton.Height= coreTitleBar.Height;
            //OptionsButton.Margin = new Thickness(0, 0, coreTitleBar.SystemOverlayRightInset, 0);
        }
    }
}
