using OnePomodoro.Helpers;
using OnePomodoro.PomodoroViews;
using OnePomodoro.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace OnePomodoro.Views
{
    public sealed partial class VisualSettingView : UserControl
    {
        public VisualSettingView()
        {
            this.InitializeComponent();
            Items = PomodoroView.Views.Select(v => new VisualSettingItem(v));
        }

        public IEnumerable<VisualSettingItem> Items { get; }

        public event EventHandler VisualChanged;

        private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = VisualsElement.SelectedItem as VisualSettingItem;
            if (item == null)
                return;

            SettingsService.Current.ViewType = item.Type.Name;
            VisualChanged?.Invoke(this, EventArgs.Empty);

            await SettingsService.SaveAsync();
        }
    }

    public class VisualSettingItem
    {
        public string ScreenshotUri { get; }

        public string Title { get; }

        public Type Type { get; }

        private readonly string DefaultScreenshotUri = "/Assets/SplashScreen.png";

        public VisualSettingItem(Type pomodoroViewType)
        {
            Type = pomodoroViewType;
           

            var attributes = Type.GetCustomAttributes(true);
            var titleAttribute = attributes.OfType<TitleAttribute>().FirstOrDefault();
            if (titleAttribute != null)
                Title = titleAttribute.Title;

            var screenshotAttribute = attributes.OfType<ScreenshotAttribute>().FirstOrDefault();
            if (screenshotAttribute != null && string.IsNullOrWhiteSpace(screenshotAttribute.Uri) == false)
                ScreenshotUri =screenshotAttribute.Uri;
            else
                ScreenshotUri = DefaultScreenshotUri;
        }
    }
}
