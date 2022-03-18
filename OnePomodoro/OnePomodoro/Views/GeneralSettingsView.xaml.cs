using OnePomodoro.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace OnePomodoro.Views
{
    public sealed partial class GeneralSettingsView : UserControl
    {
        public GeneralSettingsView()
        {
            this.InitializeComponent();
            Loaded += GeneralSettingsView_Loaded;
        }

        private GeneralSettingsViewModel ViewModel => DataContext as GeneralSettingsViewModel;

        private void GeneralSettingsView_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void OnDefaultLongBreakAfterClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Settings.LongBreakAfter = 4;
        }

        private void OnDefaultLongBreakLengthClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Settings.LongBreakLength = 15;
        }

        private void OnDefaultPomodoroLengthClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Settings.PomodoroLength = 25;
        }

        private void OnDefaultShortBreakLengthClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Settings.ShortBreakLength = 5;
        }
    }
}
