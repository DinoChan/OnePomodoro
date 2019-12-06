using OnePomodoro.Helpers;
using OnePomodoro.ViewModels;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace OnePomodoro.Views
{
    // TODO WTS: Change the URL for your privacy policy in the Resource File, currently set to https://YourPrivacyUrlGoesHere
    public sealed partial class OptionsPage : Page
    {
        private OptionsViewModel ViewModel => DataContext as OptionsViewModel;

        public OptionsPage()
        {
            InitializeComponent();

            SystemNavigationManager.GetForCurrentView().BackRequested += BlankPage1_BackRequested;
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _ = Task.Run(async () =>
              {
                  var uri = new Uri("ms-appx:///Assets/Privacy Statement.md");
                  var storageFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
                  var text = await FileIO.ReadTextAsync(storageFile);
                  _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                  {
                      PrivacyStatementMarkdownTextBlock.Text = text;
                  });

                  uri = new Uri("ms-appx:///Assets/License.md");
                  storageFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
                  text = await FileIO.ReadTextAsync(storageFile);
                  _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                  {
                      LicenseMarkdownTextBlock.Text = text;
                  });

                  try
                  {
                      var client = new HttpClient();
                      text = await client.GetStringAsync("https://raw.githubusercontent.com/DinoChan/OnePomodoro/master/Whats%20new.md"); ;
                      _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                      {
                          WhatsNewMarkdownTextBlock.Text = text;
                      });
                  }
                  catch (Exception)
                  {
                  }
              });
        }

        private void BlankPage1_BackRequested(object sender, BackRequestedEventArgs e)
        {
            On_BackRequested();
        }




        private void OnBackClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            On_BackRequested();
        }

        private bool On_BackRequested()
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
                return true;
            }
            return false;
        }

        private void BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            On_BackRequested();
            args.Handled = true;
        }

        private void OnVisualChanged(object sender, EventArgs e)
        {
            On_BackRequested();
        }
    }
}
