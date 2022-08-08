using System;
using System.Numerics;
using System.Threading.Tasks;
using OnePomodoro.Helpers;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace OnePomodoro.PomodoroViews
{
    [Title("Gradients")]
    [Screenshot("/Assets/Screenshots/Gradients.png")]
    [CompactOverlay]
    [FunctionTags(Tags.CompositionAnimation, Tags.CompositionLinearGradientBrush)]
    [SourceCode("https://github.com/DinoChan/OnePomodoro/blob/master/OnePomodoro/OnePomodoro/PomodoroViews/Gradients.xaml.cs")]
    public sealed partial class Gradients : PomodoroView
    {
        private static readonly Color Blue = Color.FromArgb(255, 43, 210, 255);
        private static readonly Color Red = Color.FromArgb(255, 255, 43, 136);
        private readonly SpriteVisual _backgroundVisual;
        private readonly Compositor _compositor;
        private readonly CompositionColorGradientStop _focusGradientStop;
        private readonly CompositionLinearGradientBrush _gradientBrush;
        private readonly CompositionColorGradientStop _relaxGradientStop;

        public Gradients() : base()
        {
            InitializeComponent();

            _compositor = Window.Current.Compositor;

            _gradientBrush = _compositor.CreateLinearGradientBrush();
            _gradientBrush.StartPoint = Vector2.Zero;
            _gradientBrush.EndPoint = new Vector2(1.0f);

            _relaxGradientStop = _compositor.CreateColorGradientStop();
            _relaxGradientStop.Offset = 0.5f;
            _relaxGradientStop.Color = Blue;
            _focusGradientStop = _compositor.CreateColorGradientStop();
            _focusGradientStop.Offset = 0.5f;
            _focusGradientStop.Color = Red;
            _gradientBrush.ColorStops.Add(_relaxGradientStop);
            _gradientBrush.ColorStops.Add(_focusGradientStop);
            _backgroundVisual = _compositor.CreateSpriteVisual();
            _backgroundVisual.Brush = _gradientBrush;
            ElementCompositionPreview.SetElementChildVisual(Gradient, _backgroundVisual);

            Loaded += async (s, e) =>
            {
                await Task.Delay(2000);
                UpdateGradients();
                MiddleText.Visibility = Visibility.Collapsed;
                await Task.Delay(2000);
                UpdateText();
            };

            Gradient.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;

                _backgroundVisual.Size = e.NewSize.ToVector2();
                _gradientBrush.CenterPoint = _backgroundVisual.Size / 2;
            };

            ViewModel.IsInPomodoroChanged += (s, e) =>
              {
                  UpdateGradients();
                  UpdateText();
              };
        }

        private void UpdateGradients()
        {
            var relaxGradientStopOffsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
            relaxGradientStopOffsetAnimation.Duration = TimeSpan.FromSeconds(1);
            // relaxGradientStopOffsetAnimation.DelayTime = TimeSpan.FromSeconds(2);
            relaxGradientStopOffsetAnimation.InsertKeyFrame(1.0f, ViewModel.IsInPomodoro ? 1.0f : 0.75f);
            _relaxGradientStop.StartAnimation(nameof(_relaxGradientStop.Offset), relaxGradientStopOffsetAnimation);

            var focusGradientStopOffsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
            focusGradientStopOffsetAnimation.Duration = TimeSpan.FromSeconds(1);
            //focusGradientStopOffsetAnimation.DelayTime = TimeSpan.FromSeconds(2);
            focusGradientStopOffsetAnimation.InsertKeyFrame(1.0f, ViewModel.IsInPomodoro ? 0.25f : 0.0f);
            _focusGradientStop.StartAnimation(nameof(_focusGradientStop.Offset), focusGradientStopOffsetAnimation);
        }

        private void UpdateText()
        {
            FocusText.Visibility = ViewModel.IsInPomodoro ? Visibility.Visible : Visibility.Collapsed;
            RelaxText.Visibility = ViewModel.IsInPomodoro ? Visibility.Collapsed : Visibility.Visible;
            FocusTextCompact.Visibility = ViewModel.IsInPomodoro ? Visibility.Visible : Visibility.Collapsed;
            RelaxTextCompact.Visibility = ViewModel.IsInPomodoro ? Visibility.Collapsed : Visibility.Visible;
        }

        //private void OnOptionsClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        //{
        //    RaiseNavigateToOptionsPage();
        //}

        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    base.OnNavigatedTo(e);
        //    Window.Current.SetTitleBar(null);
        //}
    }
}
