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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace OnePomodoro.Views
{
    public sealed partial class OptionsPage : Page
    {
        private OptionsViewModel ViewModel => DataContext as OptionsViewModel;

        public OptionsPage()
        {
            InitializeComponent();

            SystemNavigationManager.GetForCurrentView().BackRequested += BlankPage1_BackRequested;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
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

                  var licenseUri = new Uri("ms-appx:///Assets/License.md");
                  var licenseFile = await StorageFile.GetFileFromApplicationUriAsync(licenseUri);
                  var licenseText = await FileIO.ReadTextAsync(licenseFile);
                  _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                  {
                      LicenseMarkdownTextBlock.Text = licenseText;
                  });

                  try
                  {
                      var client = new HttpClient();
                      var whatsNewText = await client.GetStringAsync("https://raw.githubusercontent.com/DinoChan/OnePomodoro/master/Whats%20new.md"); ;
                      _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                      {
                          WhatsNewMarkdownTextBlock.Text = whatsNewText;
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

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                if (_visualImage != null)
                {
                    var animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("backAnimation", _visualImage);
                    animation.Configuration = new GravityConnectedAnimationConfiguration();
                    _visualImage = null;
                }
            }
        }

        private void BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            On_BackRequested();
            args.Handled = true;
        }

        private Image _visualImage;

        private void OnVisualChanged(object sender, Tuple<Type, Image> e)
        {
            _visualImage = e.Item2;
            On_BackRequested();
        }
    }
}
