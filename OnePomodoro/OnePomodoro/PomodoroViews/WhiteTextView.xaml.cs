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
    [Title("White Text")]
    [Screenshot("/Assets/Screenshots/WhiteText.png")]
    public sealed partial class WhiteTextView : PomodoroView
    {
        private PointLight _redLight;
        private PointLight _blueLight;
        private AmbientLight _backgroundLight;
        private AmbientLight _buttonLight;

        private Color _redColor = Color.FromArgb(255, 217, 17, 83);
        private Color _blueColor = Color.FromArgb(255, 0, 27, 171);

        private Color _lightRedColor = Color.FromArgb(255, 247, 97, 163);
        private Color _lightBlueColor = Color.FromArgb(255, 80, 107, 251);

        public WhiteTextView()
        {
            this.InitializeComponent();
            OnIsInPomodoroChanged();
            Loaded += OnLoaded;
            ViewModel.IsInPomodoroChanged += (s, e) => OnIsInPomodoroChanged();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            ShowTextShimmingAsync();
            OnIsInPomodoroChanged();
        }

        private void OnIsInPomodoroChanged()
        {
            FocusPanel.Visibility = ViewModel.IsInPomodoro ? Visibility.Visible : Visibility.Collapsed;
            RelaxPanel.Visibility = ViewModel.IsInPomodoro ? Visibility.Collapsed : Visibility.Visible;
        }
        

        private void ShowTextShimmingAsync()
        {
            _redLight = CreatePointLightAndStartAnimation(_redColor, TimeSpan.Zero);
            _blueLight = CreatePointLightAndStartAnimation(_blueColor, TimeSpan.FromSeconds(0.5));
            var backgroundVisual = VisualExtensions.GetVisual(Background);

            _redLight.Targets.Add(backgroundVisual);
            _blueLight.Targets.Add(backgroundVisual);
        }

        private PointLight CreatePointLightAndStartAnimation(Color color, TimeSpan delay)
        {
            var width = 1920f;
            var height = 1050f;
            var compositor = Window.Current.Compositor;
            var rootVisual = VisualExtensions.GetVisual(this);
            var pointLight = compositor.CreatePointLight();

            pointLight.Color = color;
            pointLight.CoordinateSpace = rootVisual;
            pointLight.Offset = new Vector3(-width * 3, height / 2, 150.0f);

            var offsetAnimation = compositor.CreateScalarKeyFrameAnimation();
            offsetAnimation.InsertKeyFrame(1.0f, width * 4, compositor.CreateLinearEasingFunction());
            offsetAnimation.Duration = TimeSpan.FromSeconds(10);
            offsetAnimation.DelayTime = delay;
            offsetAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

            pointLight.StartAnimation("Offset.X", offsetAnimation);
            return pointLight;
        }

    }

}
