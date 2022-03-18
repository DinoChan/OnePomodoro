using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace OnePomodoro.PomodoroViews
{
    public sealed partial class EllipseClipWorkControl : PomodoroView
    {
        private List<CompositionEllipseGeometry> _compositionEllipseGeometries;
        private List<Visual> _timeVisuals;

        public EllipseClipWorkControl()
        {
            this.InitializeComponent();

            _compositionEllipseGeometries = new List<CompositionEllipseGeometry>();
            MakeClip(Section0);
            MakeClip(Section1);
            MakeClip(Section2);
            MakeClip(Section3);
            MakeClip(Section4);

            _timeVisuals = new List<Visual>();
            MakeOffset(TimeElemnt0);
            MakeOffset(TimeElemnt1);
            MakeOffset(TimeElemnt2);
            MakeOffset(TimeElemnt3);
            MakeOffset(TimeElemnt4);

            ViewModel.RemainingPomodoroTimeChanged += OnTimeChanged;
            OnTimeChanged(null, null);
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private CompositionEllipseGeometry CreateEllipseGeometry()
        {
            var compositor = Window.Current.Compositor;
            var ellipseGeomerty = compositor.CreateEllipseGeometry();
            ellipseGeomerty.Center = new System.Numerics.Vector2(192, 525);
            ellipseGeomerty.Radius = Vector2.Zero;
            return ellipseGeomerty;
        }

        private void MakeClip(UIElement uElement)
        {
            var compositor = Window.Current.Compositor;
            var visual = ElementCompositionPreview.GetElementVisual(uElement);
            var geometry = CreateEllipseGeometry();
            var clip = compositor.CreateGeometricClip(geometry);
            visual.Clip = clip;
            _compositionEllipseGeometries.Add(geometry);
        }

        private void MakeOffset(UIElement uiElement)
        {
            var compositor = Window.Current.Compositor;
            var visual = ElementCompositionPreview.GetElementVisual(uiElement);
            visual.Offset = new Vector3(0, -100, 0);
            _timeVisuals.Add(visual);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            for (int i = 0; i < _compositionEllipseGeometries.Count; i++)
            {
                StartClipAnimation(_compositionEllipseGeometries[i], TimeSpan.FromSeconds(0.8 + i));
            }

            for (int i = 0; i < _timeVisuals.Count; i++)
            {
                StartOffsetAnimation(_timeVisuals[i], TimeSpan.FromSeconds(0.8 + i));
            }
        }

        private void OnTimeChanged(object sender, EventArgs e)
        {
            TimeElemnt1.Text = (ViewModel.RemainingPomodoroTime.Minutes / 10).ToString();
            TimeElemnt2.Text = (ViewModel.RemainingPomodoroTime.Minutes % 10).ToString();
            TimeElemnt3.Text = (ViewModel.RemainingPomodoroTime.Seconds / 10).ToString();
            TimeElemnt4.Text = (ViewModel.RemainingPomodoroTime.Seconds % 10).ToString();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.RemainingPomodoroTimeChanged -= OnTimeChanged;
        }

        private void StartClipAnimation(CompositionEllipseGeometry ellipseGeometry, TimeSpan delayTime)
        {
            var compositor = Window.Current.Compositor;
            var animation = compositor.CreateVector2KeyFrameAnimation();
            var easing = compositor.CreateCubicBezierEasingFunction(new Vector2(0.1f, 0.9f), new Vector2(0.2f, 1f));

            animation.DelayTime = delayTime;
            animation.Duration = TimeSpan.FromSeconds(0.7);
            animation.InsertKeyFrame(1, new Vector2(600, 600));
            ellipseGeometry.StartAnimation(nameof(CompositionEllipseGeometry.Radius), animation);
        }

        private void StartOffsetAnimation(Visual visual, TimeSpan delayTime)
        {
            var compositor = Window.Current.Compositor;
            var animation = compositor.CreateVector3KeyFrameAnimation();
            var easing = compositor.CreateCubicBezierEasingFunction(new Vector2(0.1f, 0.9f), new Vector2(0.2f, 1f));

            animation.DelayTime = delayTime;
            animation.Duration = TimeSpan.FromSeconds(1.3);
            animation.InsertKeyFrame(1, Vector3.Zero, easing);
            visual.StartAnimation(nameof(Visual.Offset), animation);
        }
    }
}
