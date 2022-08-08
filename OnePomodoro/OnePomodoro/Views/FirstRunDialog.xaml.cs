using System;
using System.Collections.Generic;
using OnePomodoro.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OnePomodoro.Views
{
    public sealed partial class FirstRunDialog : ContentDialog
    {
        private FrameworkElement _backgroundElement;

        public FirstRunDialog()
        {
            // TODO WTS: Update the contents of this dialog with any important information you want to show when the app is used for the first time.
            RequestedTheme = (Window.Current.Content as FrameworkElement).RequestedTheme;
            InitializeComponent();
            SizeChanged += OnSizeChanged;
        }

        private void OnBackgroundElementLoaded(object sender, RoutedEventArgs e)
        {
            _backgroundElement = sender as FrameworkElement;
            UpdateBackgroundElement();
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateBackgroundElement();
        }

        private void UpdateBackgroundElement()
        {
            if (_backgroundElement == null)
                return;

            _backgroundElement.Width = Math.Min(ActualWidth, 1440);
            _backgroundElement.Height = Math.Min(ActualHeight, 810);
        }
    }

    public class TeachItem
    {
        public string Description { get; set; }
        public string Screenshot { get; set; }
        public string Title { get; set; }
    }

    public class TeachSource
    {
        /*
         * OnePomodoro是一个用于番茄工作法的计时器，这个计时器让你专注工作25分钟。

在专注工作25分钟以后（一个Pomodoro），别忘记休息5分钟，在工作4个Pomodoro以后休息一段长时间（15分钟）。

可以通过设置改变工作和休息时间，并且可以更改通知设置。

OnePomodoro提供了多种Pomodoro，让工作不那么无聊。

大部分番茄钟提供Pinable模式，这意味着它们可以Always on top.

About页面里提供了番茄钟做法的更多提示。

            OnePomodoro is a timer for Pomodoro Technique that lets you focus on work for 25 minutes.

After 25 minutes of focusing (a Pomodoro), don't forget to take a 5-minute break and take a long break (15 minutes) after working 4 Pomodoros.

You can change work and rest times through settings, and you can change notification settings.

OnePomodoro offers a variety of pomodoros to make your job less boring.

Most pomodoros offer Pinable mode, which means they can always on top.

More tips on the Pomodoro Technique are available on the About page.
    */

        public TeachSource()
        {
            Items = new List<TeachItem>();
            Items.Add(new TeachItem
            {
                Screenshot = "/Assets/Teach/teach1.png",
                Description = ResourceExtensions.GetLocalized("Teach1Description"),
                Title = ResourceExtensions.GetLocalized("Teach1Title"),
            });
            Items.Add(new TeachItem
            {
                Screenshot = "/Assets/Teach/teach2.png",
                Description = ResourceExtensions.GetLocalized("Teach2Description"),
                Title = ResourceExtensions.GetLocalized("Teach2Title"),
            });
            Items.Add(new TeachItem
            {
                Screenshot = "/Assets/Teach/teach3.png",
                Description = ResourceExtensions.GetLocalized("Teach3Description"),
                Title = ResourceExtensions.GetLocalized("Teach3Title"),
            });
            Items.Add(new TeachItem
            {
                Screenshot = "/Assets/Teach/teach4.png",
                Description = ResourceExtensions.GetLocalized("Teach4Description"),
                Title = ResourceExtensions.GetLocalized("Teach4Title"),
            });
            Items.Add(new TeachItem
            {
                Screenshot = "/Assets/Teach/teach5.png",
                Description = ResourceExtensions.GetLocalized("Teach5Description"),
                Title = ResourceExtensions.GetLocalized("Teach5Title"),
            });
            Items.Add(new TeachItem
            {
                Screenshot = "/Assets/Teach/teach6.png",
                Description = ResourceExtensions.GetLocalized("Teach6Description"),
                Title = ResourceExtensions.GetLocalized("Teach6Title"),
            });
        }

        public List<TeachItem> Items { get; }
    }
}
