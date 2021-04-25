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
using Microsoft.Toolkit.Uwp.UI;

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

        public event EventHandler<Tuple<Type, Image>> VisualChanged;


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

    public class VisualSettingItem
    {
        public string ScreenshotUri { get; }

        public string SourceCodeUri { get; }

        public string Title { get; }

        public Type Type { get; }

        public string[] Tags { get; }

        public bool Pinable { get; }

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
    }
}
