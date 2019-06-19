using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using OnePomodoro.Helpers;
using OnePomodoro.Services;

using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

using Windows.ApplicationModel;
using Windows.UI.Xaml;

using Microsoft.Toolkit.Uwp.Helpers;

namespace OnePomodoro.ViewModels
{
    // TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings.md
    public class AboutViewModel : ViewModelBase
    {
        private string _version;

        public AboutViewModel()
        {
        }

        public string Version
        {
            get { return _version; }

            set { SetProperty(ref _version, value); }
        }

        private ICommand _reviewCommand;

        public ICommand ReviewCommand
        {
            get
            {
                if (_reviewCommand == null)
                {
                    _reviewCommand = new DelegateCommand(
                        async () =>
                        {
                            await SystemInformation.LaunchStoreForReviewAsync();
                        });
                }

                return _reviewCommand;
            }
        }
      
        public async Task InitializeAsync()
        {
            Version = GetVersion();
            await Task.CompletedTask;
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            await InitializeAsync();
        }

        private string GetVersion()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}
