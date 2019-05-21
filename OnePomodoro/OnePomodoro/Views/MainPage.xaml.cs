using System;

using OnePomodoro.ViewModels;

using Windows.UI.Xaml.Controls;

namespace OnePomodoro.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnSettingClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var dialog = new InformationDialog();
            dialog.ShowAsync();
        }
    }
}
