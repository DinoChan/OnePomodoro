using System;
using OnePomodoro.PomodoroViews;
using OnePomodoro.ViewModels;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
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
            SwitchCompactOverlayMode();
           // Frame.Navigate(typeof(OptionsPage));
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

        public static bool IsInCompactOverlayMode = false; //记录是否在画中画模式

        public static bool IsCompactOverlayModeSupported  //确定当前设备是否支持制定的视图模式，这里就是画中画模式
        {
            get { return ApplicationView.GetForCurrentView().IsViewModeSupported(ApplicationViewMode.CompactOverlay); }
        }

        public static async void SwitchCompactOverlayMode()
        {
            bool _modeswitchstatus = false; // 切换模式成功指示器
            if (IsInCompactOverlayMode)
            {
                _modeswitchstatus = await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.Default);
                //进入默认视图模式
            }
            else
            {
                if (IsCompactOverlayModeSupported)
                {
                    ViewModePreferences compactOptions = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
                    compactOptions.CustomSize = new Windows.Foundation.Size(350, 100); //调整画中画模式的窗口初始大小
                    _modeswitchstatus = await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay, compactOptions);
                    //进入画中画模式
                }
                else
                {
                    //如果当前设备不支持画中画模式，则...（动作）
                }
            }
            IsInCompactOverlayMode = _modeswitchstatus ? !IsInCompactOverlayMode : IsInCompactOverlayMode; //如果切换模式成功，则逆转这个值
        }
    }
}
