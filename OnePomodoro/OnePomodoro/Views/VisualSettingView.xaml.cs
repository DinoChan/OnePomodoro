using OnePomodoro.Helpers;
using OnePomodoro.PomodoroViews;
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
    }

    public class VisualSettingItem
    {
        public Uri ScreenshotUri { get; }

        public string Title { get; }

        public Type Type { get; }
        private readonly Uri DefaultScreenshotUri = new Uri("/Assets/SplashScreen.png", UriKind.RelativeOrAbsolute);

        public VisualSettingItem(Type pomodoroViewType)
        {
            Type = pomodoroViewType;

            var attributes = Type.GetCustomAttributes(true);
            var titleAttribute = attributes.OfType<TitleAttribute>().FirstOrDefault();
            if (titleAttribute != null)
                Title = titleAttribute.Title;

            var screenshotAttribute = attributes.OfType<ScreenshotAttribute>().FirstOrDefault();
            if (screenshotAttribute != null && string.IsNullOrWhiteSpace(screenshotAttribute.Uri) == false)
                ScreenshotUri = new Uri(screenshotAttribute.Uri, UriKind.RelativeOrAbsolute);
            else
                ScreenshotUri = DefaultScreenshotUri;
        }
    }
}
