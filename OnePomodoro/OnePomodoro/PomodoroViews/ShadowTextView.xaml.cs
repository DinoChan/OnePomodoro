using System;
using System.Numerics;
using Microsoft.Toolkit.Uwp.UI;
using OnePomodoro.Helpers;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace OnePomodoro.PomodoroViews
{
    [Title("Shadow Text")]
    [Screenshot("/Assets/Screenshots/ShadowText.png")]
    [CompactOverlay(CustomWidth = 288, CustomHeight = 157.5)]
    [FunctionTags(Tags.CompositionAnimation, Tags.PointLight, Tags.AmbientLight, Tags.Win2D)]
    [SourceCode("https://github.com/DinoChan/OnePomodoro/blob/master/OnePomodoro/OnePomodoro/PomodoroViews/ShadowTextView.xaml.cs")]
    public sealed partial class ShadowTextView : PomodoroView
    {
        private AmbientLight _backgroundLight;
        private Color _blueColor = Color.FromArgb(255, 70, 77, 231);
        private PointLight _blueLight;
        private AmbientLight _buttonLight;
        private Color _greenColor = Color.FromArgb(255, 125, 255, 110);
        private AmbientLight _greenLight;
        private Color _lightBlueColor = Color.FromArgb(255, 130, 157, 255);
        private Color _lightRedColor = Color.FromArgb(255, 255, 147, 213);
        private Color _redColor = Color.FromArgb(255, 255, 67, 133);
        private PointLight _redLight;

        public ShadowTextView()
        {
            this.InitializeComponent();
            OnIsInPomodoroChanged();
            Loaded += OnLoaded;
            ViewModel.IsInPomodoroChanged += (s, e) => OnIsInPomodoroChanged();
            var footBackgroundVisual = VisualExtensions.GetVisual(FootBackground);
            footBackgroundVisual.Opacity = 0;
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
            ShowBackgroundLight();
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
            HideBackgroundLight();
        }

        private AmbientLight CreateAmbientLightAndStartAnimation()
        {
            var compositor = Window.Current.Compositor;
            var ambientLight = compositor.CreateAmbientLight();
            ambientLight.Intensity = 0;
            ambientLight.Color = Colors.White;

            var intensityAnimation = compositor.CreateScalarKeyFrameAnimation();
            intensityAnimation.InsertKeyFrame(0.2f, 0, compositor.CreateLinearEasingFunction());
            intensityAnimation.InsertKeyFrame(0.5f, 0.20f, compositor.CreateLinearEasingFunction());
            intensityAnimation.InsertKeyFrame(0.8f, 0, compositor.CreateLinearEasingFunction());
            intensityAnimation.Duration = TimeSpan.FromSeconds(10);
            intensityAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

            ambientLight.StartAnimation(nameof(AmbientLight.Intensity), intensityAnimation);

            return ambientLight;
        }

        private void CreateBackgroundLight()
        {
            var compositor = Window.Current.Compositor;
            var buttonVisual = VisualExtensions.GetVisual(ButtonPanel);
            var focusVisual = VisualExtensions.GetVisual(FocusPanel);
            var relayVisual = VisualExtensions.GetVisual(RelaxPanel);
            var footVisual = VisualExtensions.GetVisual(FootBackground);

            _backgroundLight = compositor.CreateAmbientLight();
            _backgroundLight.Color = ViewModel.IsInPomodoro ? _lightRedColor : _lightBlueColor;
            _backgroundLight.Intensity = 0;
            _backgroundLight.Targets.Add(focusVisual);
            _backgroundLight.Targets.Add(relayVisual);
            _backgroundLight.Targets.Add(footVisual);

            _buttonLight = compositor.CreateAmbientLight();
            _buttonLight.Color = Colors.White;
            _buttonLight.Intensity = 0;
            _buttonLight.Targets.Add(buttonVisual);
        }

        private PointLight CreatePointLightAndStartAnimation(Color color, float startOffsetX, float endOffsetX, float height)
        {
            var compositor = Window.Current.Compositor;

            var rootVisual = VisualExtensions.GetVisual(Root);
            var pointLight = compositor.CreatePointLight();

            pointLight.Color = color;
            pointLight.CoordinateSpace = rootVisual;
            pointLight.Offset = new Vector3(startOffsetX, height, 300.0f);

            var offsetAnimation = compositor.CreateScalarKeyFrameAnimation();
            offsetAnimation.InsertKeyFrame(1.0f, endOffsetX, compositor.CreateLinearEasingFunction());
            offsetAnimation.Duration = TimeSpan.FromSeconds(10);
            offsetAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

            pointLight.StartAnimation("Offset.X", offsetAnimation);
            return pointLight;
        }

        private void HideBackgroundLight()
        {
            if (_backgroundLight == null)
                return;

            var compositor = Window.Current.Compositor;

            var scalarAnimation = compositor.CreateScalarKeyFrameAnimation();
            scalarAnimation.InsertKeyFrame(1.0f, 0, compositor.CreateLinearEasingFunction());
            scalarAnimation.Duration = TimeSpan.FromSeconds(1);
            _backgroundLight.StartAnimation(nameof(AmbientLight.Intensity), scalarAnimation);
            _buttonLight.StartAnimation(nameof(AmbientLight.Intensity), scalarAnimation);

            var footBackgroundVisual = VisualExtensions.GetVisual(FootBackground);
            footBackgroundVisual.StartAnimation(nameof(footBackgroundVisual.Opacity), scalarAnimation);
        }

        private void OnIsInPomodoroChanged()
        {
            FocusPanel.Visibility = ViewModel.IsInPomodoro ? Visibility.Visible : Visibility.Collapsed;
            RelaxPanel.Visibility = ViewModel.IsInPomodoro ? Visibility.Collapsed : Visibility.Visible;
            SwitchBackgroundLightColor();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            ShowTextShimmingAsync();
            OnIsInPomodoroChanged();
            CreateBackgroundLight();
        }

        private void ShowBackgroundLight()
        {
            if (_backgroundLight == null)
                return;

            var compositor = Window.Current.Compositor;

            var scalarAnimation = compositor.CreateScalarKeyFrameAnimation();
            scalarAnimation.InsertKeyFrame(1.0f, 0.5f, compositor.CreateLinearEasingFunction());
            scalarAnimation.Duration = TimeSpan.FromSeconds(1);
            _backgroundLight.StartAnimation(nameof(AmbientLight.Intensity), scalarAnimation);

            scalarAnimation = compositor.CreateScalarKeyFrameAnimation();
            scalarAnimation.InsertKeyFrame(1.0f, 1f, compositor.CreateLinearEasingFunction());
            scalarAnimation.Duration = TimeSpan.FromSeconds(1);
            _buttonLight.StartAnimation(nameof(AmbientLight.Intensity), scalarAnimation);

            var footBackgroundVisual = VisualExtensions.GetVisual(FootBackground);
            footBackgroundVisual.StartAnimation(nameof(footBackgroundVisual.Opacity), scalarAnimation);
        }

        private void ShowTextShimmingAsync()
        {
            var width = 1920;
            float height = 922;
            _redLight = CreatePointLightAndStartAnimation(_redColor, -width * 1, width * 2, height * 0.5f);
            _blueLight = CreatePointLightAndStartAnimation(_blueColor, width * 2, -width * 1, height * 0.75f);
            _greenLight = CreateAmbientLightAndStartAnimation();
            var focusVisual = VisualExtensions.GetVisual(FocusPanel);
            var relayVisual = VisualExtensions.GetVisual(RelaxPanel);

            _redLight.Targets.Add(focusVisual);
            _blueLight.Targets.Add(focusVisual);

            _redLight.Targets.Add(relayVisual);
            _blueLight.Targets.Add(relayVisual);

            _greenLight.Targets.Add(focusVisual);
            _greenLight.Targets.Add(relayVisual);
        }

        private void SwitchBackgroundLightColor()
        {
            if (_backgroundLight == null)
                return;

            var compositor = Window.Current.Compositor;

            var colorAnimation = compositor.CreateColorKeyFrameAnimation();
            colorAnimation.InsertKeyFrame(1.0f, ViewModel.IsInPomodoro ? _lightRedColor : _lightBlueColor, compositor.CreateLinearEasingFunction());
            colorAnimation.Duration = TimeSpan.FromSeconds(1);
            _backgroundLight.StartAnimation(nameof(AmbientLight.Color), colorAnimation);
        }
    }
}
