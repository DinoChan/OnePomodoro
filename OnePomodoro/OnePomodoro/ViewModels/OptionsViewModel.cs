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

namespace OnePomodoro.ViewModels
{
    public class OptionsViewModel : ViewModelBase
    {
        public OptionsViewModel()
        {
        }

        public async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            await InitializeAsync();
        }
    }
}
