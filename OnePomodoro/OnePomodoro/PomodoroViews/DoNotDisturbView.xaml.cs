using OnePomodoro.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace OnePomodoro.PomodoroViews
{
    [Title("Do Not Disturb")]
    [Screenshot("/Assets/Screenshots/DoNotDisturb.png")]
    public sealed partial class DoNotDisturbView
    {
        private Compositor Compositor => Window.Current.Compositor;
        public DoNotDisturbView()
        {
            this.InitializeComponent();
            ViewModel.IsInPomodoroChanged += OnIsPomodoroChanged;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            UpdateContent();
        }

        private void OnIsPomodoroChanged(object sender, EventArgs e)
        {
            UpdateContent();
        }

        private void UpdateContent()
        {
            var contentVisual = ElementCompositionPreview.GetElementVisual(ContentArea);
            var timerVisual = ElementCompositionPreview.GetElementVisual(TimerArea);

            var easing = Compositor.CreateCubicBezierEasingFunction(new Vector2(0.1f, 0.9f), new Vector2(0.2f, 1f));

            var contentVisualAnimation = Compositor.CreateVector3KeyFrameAnimation();
            contentVisualAnimation.Duration = TimeSpan.FromSeconds(1);

            var timeVisualAnimation = Compositor.CreateVector3KeyFrameAnimation();
            timeVisualAnimation.Duration = TimeSpan.FromSeconds(1);

            if (ViewModel.IsInPomodoro)
            {

                contentVisualAnimation.InsertKeyFrame(1, Vector3.Zero, easing);
                contentVisual.StartAnimation(nameof(Visual.Offset), contentVisualAnimation);

                timeVisualAnimation.InsertKeyFrame(1, new Vector3(0, 200, 0), easing);
                timerVisual.StartAnimation(nameof(Visual.Offset), timeVisualAnimation);

                ShowAnimation(WorkText1);
                ShowAnimation(WorkText2);
                ShowAnimation(WorkText3);

                HideAnimation(BreakText1);
                HideAnimation(BreakText2);
                HideAnimation(BreakText3);
            }
            else
            {
                contentVisualAnimation.InsertKeyFrame(1, new Vector3(-1480, 0, 0), easing);
                contentVisual.StartAnimation(nameof(Visual.Offset), contentVisualAnimation);

                timeVisualAnimation.InsertKeyFrame(1, new Vector3(340, -200, 0), easing);
                timerVisual.StartAnimation(nameof(Visual.Offset), timeVisualAnimation);

                ShowAnimation(BreakText1);
                ShowAnimation(BreakText2);
                ShowAnimation(BreakText3);

                HideAnimation(WorkText1);
                HideAnimation(WorkText2);
                HideAnimation(WorkText3);
            }
        }

        private void ShowAnimation(FrameworkElement element)
        {
            var visual = ElementCompositionPreview.GetElementVisual(element);
            visual.Offset = new Vector3(-(float)element.ActualWidth, 0, 0);

            var easing = Compositor.CreateCubicBezierEasingFunction(new Vector2(0.1f, 0.9f), new Vector2(0.2f, 1f));
            var offsetAnimation = Compositor.CreateVector3KeyFrameAnimation();
            offsetAnimation.Duration = TimeSpan.FromSeconds(1);
            offsetAnimation.DelayTime = TimeSpan.FromSeconds(0.3);

            offsetAnimation.InsertKeyFrame(1, Vector3.Zero, easing);
            visual.StartAnimation(nameof(Visual.Offset), offsetAnimation);
        }


        private void HideAnimation(FrameworkElement element)
        {
            var visual = ElementCompositionPreview.GetElementVisual(element);

            var easing = Compositor.CreateCubicBezierEasingFunction(new Vector2(0.1f, 0.9f), new Vector2(0.2f, 1f));
            var offsetAnimation = Compositor.CreateVector3KeyFrameAnimation();
            offsetAnimation.Duration = TimeSpan.FromSeconds(1);
            offsetAnimation.DelayTime = TimeSpan.FromSeconds(0.4);

            offsetAnimation.InsertKeyFrame(1, new Vector3(-(float)element.ActualWidth, 0, 0), easing);
            visual.StartAnimation(nameof(Visual.Offset), offsetAnimation);
        }
    }
}
