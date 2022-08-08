using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Toolkit.Uwp.UI;
using OnePomodoro.Helpers;
using OnePomodoro.PomodoroViews;
using OnePomodoro.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace OnePomodoro.Views
{
    public class VisualSettingItem
    {
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
                ScreenshotUri = screenshotAttribute.Uri;
            else
                ScreenshotUri = DefaultScreenshotUri;

            var compactOverlayAttribute = attributes.OfType<CompactOverlayAttribute>().FirstOrDefault();
            Pinable = compactOverlayAttribute != null;

            var tagsAttribute = attributes.OfType<FunctionTagsAttribute>().FirstOrDefault();
            Tags = tagsAttribute?.Tags;

            var sourceCodeAttribute = attributes.OfType<SourceCodeAttribute>().FirstOrDefault();
            SourceCodeUri = sourceCodeAttribute == null ? "https://github.com/DinoChan/OnePomodoro" : sourceCodeAttribute.SourceCodeUri;
        }

        public bool Pinable { get; }
        public string ScreenshotUri { get; }

        public string SourceCodeUri { get; }

        public string[] Tags { get; }
        public string Title { get; }

        public Type Type { get; }
    }

    public sealed partial class VisualSettingView : UserControl
    {
        public VisualSettingView()
        {
            this.InitializeComponent();
            Items = PomodoroView.Views.Select(v => new VisualSettingItem(v));
        }

        public event EventHandler<Tuple<Type, Image>> VisualChanged;

        public IEnumerable<VisualSettingItem> Items { get; }

        private async void OnSelectVisual(object sender, RoutedEventArgs e)
        {
            var element = (sender as FrameworkElement);
            var image = element.FindDescendant<Image>();
            var item = element.DataContext as VisualSettingItem;

            SettingsService.Current.ViewType = item.Type.Name;
            VisualChanged?.Invoke(item.Type, new Tuple<Type, Image>(item.Type, image));
            await SettingsService.SaveAsync();
        }
    }
}
