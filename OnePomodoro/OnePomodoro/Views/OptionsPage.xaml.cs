using System;

using OnePomodoro.ViewModels;

using Windows.UI.Xaml.Controls;
using System.Linq;

namespace OnePomodoro.Views
{
    // TODO WTS: Change the URL for your privacy policy in the Resource File, currently set to https://YourPrivacyUrlGoesHere
    public sealed partial class OptionsPage : Page
    {
        private OptionsViewModel ViewModel => DataContext as OptionsViewModel;

        public OptionsPage()
        {
            InitializeComponent();
            NavigationView.SelectedItem= NavigationView.MenuItems.OfType<NavigationViewItem>().First();
        }

        private void OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            Frame.GoBack();
        }

        private void OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (NavigationView.SelectedItem == AboutItem)
                ContentFrame.Navigate(typeof(AboutPage));
        }
    }
}
