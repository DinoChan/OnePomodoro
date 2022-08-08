using System;
using System.Numerics;
using Microsoft.Toolkit.Uwp.UI;
using OnePomodoro.Helpers;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace OnePomodoro.PomodoroViews
{
    [Title("White Text")]
    [Screenshot("/Assets/Screenshots/WhiteText.png")]
    [CompactOverlay(CustomWidth = 288, CustomHeight = 157.5)]
    [FunctionTags(Tags.PointLight)]
    [SourceCode("https://github.com/DinoChan/OnePomodoro/blob/master/OnePomodoro/OnePomodoro/PomodoroViews/WhiteTextView.xaml.cs")]
    public sealed partial class WhiteTextView : PomodoroView
    {
        private Color _blueColor = Color.FromArgb(255, 0, 27, 171);
        private PointLight _blueLight;
        private Color _redColor = Color.FromArgb(255, 217, 17, 83);
        private PointLight _redLight;

        public WhiteTextView()
        {
            InitializeComponent();
            OnIsInPomodoroChanged();
            Loaded += OnLoaded;
            ViewModel.IsInPomodoroChanged += (s, e) => OnIsInPomodoroChanged();
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

        private void OnIsInPomodoroChanged()
        {
            FocusPanel.Visibility = ViewModel.IsInPomodoro ? Visibility.Visible : Visibility.Collapsed;
            RelaxPanel.Visibility = ViewModel.IsInPomodoro ? Visibility.Collapsed : Visibility.Visible;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            ShowTextShimmingAsync();
            OnIsInPomodoroChanged();
        }

        private void ShowTextShimmingAsync()
        {
            _redLight = CreatePointLightAndStartAnimation(_redColor, TimeSpan.Zero);
            _blueLight = CreatePointLightAndStartAnimation(_blueColor, TimeSpan.FromSeconds(0.5));
            var backgroundVisual = VisualExtensions.GetVisual(BackgroundElement);

            _redLight.Targets.Add(backgroundVisual);
            _blueLight.Targets.Add(backgroundVisual);
        }
    }
}
