using System;

using OnePomodoro.ViewModels;

using Windows.UI.Xaml.Controls;
using System.Linq;
using Windows.UI.Xaml.Input;
using Windows.UI.Core;
using OnePomodoro.Helpers;
using Windows.UI.Xaml;
using Windows.System;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;

namespace OnePomodoro.Views
{
    // TODO WTS: Change the URL for your privacy policy in the Resource File, currently set to https://YourPrivacyUrlGoesHere
    public sealed partial class OptionsPage : Page
    {
        private OptionsViewModel ViewModel => DataContext as OptionsViewModel;

        public OptionsPage()
        {
            InitializeComponent();
            KeyboardAccelerator GoBack = new KeyboardAccelerator();
            GoBack.Key = VirtualKey.GoBack;
            GoBack.Invoked += BackInvoked;
            KeyboardAccelerator AltLeft = new KeyboardAccelerator();
            AltLeft.Key = VirtualKey.Left;
            AltLeft.Invoked += BackInvoked;
            //this.KeyboardAccelerators.Add(GoBack);
            //this.KeyboardAccelerators.Add(AltLeft);//不能禁止Tooltip
            // ALT routes here
            AltLeft.Modifiers = VirtualKeyModifiers.Menu;
            SystemNavigationManager.GetForCurrentView().BackRequested += BlankPage1_BackRequested;

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;

            // Set XAML element as a draggable region.
            Window.Current.SetTitleBar(AppTitleBar);
            //var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            UpdateTitleBarLayout(coreTitleBar);
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            TitleBarHelper.UpdatePageTitleColor(this);

            if (PrivacyStatementMarkdownTextBlock.Text != null)
            {

                //var target = obj as MarkdownTextBlock;
                var uri = new Uri("ms-appx:///Assets/Privacy Statement.md");
                var storageFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
                var text = await FileIO.ReadTextAsync(storageFile);
                PrivacyStatementMarkdownTextBlock.Text = text;
            }

            if (LicenseMarkdownTextBlock.Text != null)
            {
                var uri = new Uri("ms-appx:///Assets/License.md");
                var storageFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
                var text = await FileIO.ReadTextAsync(storageFile);
                LicenseMarkdownTextBlock.Text = text;
            }

            if (WhatsNewMarkdownTextBlock.Text != null)
            {
                var uri = new Uri("ms-appx:///Assets/Whats new.md");
                var storageFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
                var text = await FileIO.ReadTextAsync(storageFile);
                WhatsNewMarkdownTextBlock.Text = text;
            }
        }

        private void BlankPage1_BackRequested(object sender, BackRequestedEventArgs e)
        {
            On_BackRequested();
        }

        private void UpdateTitleBarLayout(CoreApplicationViewTitleBar coreTitleBar)
        {
            BackButton.Height = coreTitleBar.Height;
            AppTitleBar.Height = coreTitleBar.Height;
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
