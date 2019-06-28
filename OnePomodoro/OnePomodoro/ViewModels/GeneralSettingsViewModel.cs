using OnePomodoro.Models;
using OnePomodoro.Services;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePomodoro.ViewModels
{
    public class GeneralSettingsViewModel : ViewModelBase
    {
        public GeneralSettingsViewModel()
        {
            Settings = SettingsService.Current;
        }

        public ISettings Settings { get; }

        public async void OnSettingChanged()
        {
            await SettingsService.SaveAsync();
        }
    }
}
