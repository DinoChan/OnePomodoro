using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Composition;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using OnePomodoro.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.DirectX;
using Windows.Networking.BackgroundTransfer;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Text;
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
    [Title("Hidden Text")]
    [Screenshot("/Assets/Screenshots/HiddenText.png")]
    [CompactOverlay(CustomWidth = 288, CustomHeight = 157.5)]
    public sealed partial class HiddenTextView : PomodoroView
    {
        private PointLight _redLight;
        private PointLight _blueLight;
        private AmbientLight _backgroundLight;
        private AmbientLight _buttonLight;

        private Color _redColor = Color.FromArgb(255, 217, 17, 83);
        private Color _blueColor = Color.FromArgb(255, 0, 27, 171);

        private Color _lightRedColor = Color.FromArgb(255, 247, 97, 163);
        private Color _lightBlueColor = Color.FromArgb(255, 80, 107, 251);

        public HiddenTextView()
        {
            this.InitializeComponent();
            OnIsInPomodoroChanged();
            Loaded += OnLoaded;
            ViewModel.IsInPomodoroChanged += (s, e) => OnIsInPomodoroChanged();
            var footBackgroundVisual = VisualExtensions.GetVisual(FootBackground);
            footBackgroundVisual.Opacity = 0;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            ShowTextShimmingAsync();
            OnIsInPomodoroChanged();
            CreateBackgroundLight();
        }

        private void OnIsInPomodoroChanged()
        {
            FocusPanel.Visibility = ViewModel.IsInPomodoro ? Visibility.Visible : Visibility.Collapsed;
            RelaxPanel.Visibility = ViewModel.IsInPomodoro ? Visibility.Collapsed : Visibility.Visible;
            SwitchBackgroundLightColor();
            
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

        private void ShowTextShimmingAsync()
        {
            _redLight = CreatePointLightAndStartAnimation(_redColor, TimeSpan.Zero);
            _blueLight = CreatePointLightAndStartAnimation(_blueColor, TimeSpan.FromSeconds(0.25));
            var focusVisual = VisualExtensions.GetVisual(FocusPanel);
            var relayVisual = VisualExtensions.GetVisual(RelaxPanel);

            _redLight.Targets.Add(focusVisual);
            _blueLight.Targets.Add(focusVisual);

            _redLight.Targets.Add(relayVisual);
            _blueLight.Targets.Add(relayVisual);
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

        private PointLight CreatePointLightAndStartAnimation(Color color, TimeSpan delay)
        {
            var width = 960;
            var height = 461;
            var compositor = Window.Current.Compositor;

            var rootVisual = VisualExtensions.GetVisual(Root);
            var pointLight = compositor.CreatePointLight();

            pointLight.Color = color;
            pointLight.CoordinateSpace = rootVisual;
            pointLight.Offset = new Vector3(-width * 4, height / 2, 75.0f);

            var offsetAnimation = compositor.CreateScalarKeyFrameAnimation();
            offsetAnimation.InsertKeyFrame(1.0f, width * 5, compositor.CreateLinearEasingFunction());
            offsetAnimation.Duration = TimeSpan.FromSeconds(10);
            offsetAnimation.DelayTime = delay;
            offsetAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

            pointLight.StartAnimation("Offset.X", offsetAnimation);
            return pointLight;
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


    }

}
