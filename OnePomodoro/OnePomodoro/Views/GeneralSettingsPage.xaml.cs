using System;

using OnePomodoro.ViewModels;

using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace OnePomodoro.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class GeneralSettingsPage : Page
    {
        public GeneralSettingsPage()
        {
            this.InitializeComponent();
        }

        private GeneralSettingsViewModel ViewModel => DataContext as GeneralSettingsViewModel;
    }
}
