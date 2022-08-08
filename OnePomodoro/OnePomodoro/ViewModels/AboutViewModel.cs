using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.ApplicationModel;

namespace OnePomodoro.ViewModels
{
    // TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings.md
    public class AboutViewModel : ObservableObject
    {
        private ICommand _reviewCommand;
        private string _version;

        public AboutViewModel()
        {
            Version = GetVersion();
        }

        public ICommand ReviewCommand
        {
            get
            {
                if (_reviewCommand == null)
                {
                    _reviewCommand = new RelayCommand(
                        async () =>
                        {
                            await SystemInformation.LaunchStoreForReviewAsync();
                        });
                }

                return _reviewCommand;
            }
        }

        public string Version
        {
            get { return _version; }

            set { SetProperty(ref _version, value); }
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
