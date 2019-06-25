using System;
using System.ComponentModel;
using OnePomodoro.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace OnePomodoro.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AboutPage : Page
    {
        public AboutPage()
        {
            this.InitializeComponent();
            Loaded += OnLoaded;
        }

       
        private AboutViewModel ViewModel => DataContext as AboutViewModel;

        private void OnLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.OnNavigatedTo(null, null);
        }

    }
}
