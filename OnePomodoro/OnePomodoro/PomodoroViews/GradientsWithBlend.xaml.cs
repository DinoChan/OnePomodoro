using System;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas.Effects;
using OnePomodoro.Helpers;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace OnePomodoro.PomodoroViews
{
    [Title("GradientsWithBlend")]
    [Screenshot("/Assets/Screenshots/GradientsWithBlend.png")]
    [CompactOverlay(CustomHeight = 150, CustomWidth = 150)]
    [FunctionTags(Tags.CompositionAnimation, Tags.CompositionLinearGradientBrush, Tags.BlendEffect)]
    [SourceCode("https://github.com/DinoChan/OnePomodoro/blob/master/OnePomodoro/OnePomodoro/PomodoroViews/GradientsWithBlend.xaml.cs")]
    public sealed partial class GradientsWithBlend : PomodoroView
    {
        private readonly CompositionLinearGradientBrush _foregroundBrush;
        private readonly CompositionLinearGradientBrush _backgroundBrush;
        private readonly SpriteVisual _backgroundVisual;
        private static readonly Color Blue = Color.FromArgb(255, 43, 210, 255);
        private static readonly Color Green = Color.FromArgb(255, 43, 255, 136);
        private static readonly Color Red = Colors.Red;

        //private static readonly Color Pink = Color.FromArgb(255, 255, 43, 212);
        private static readonly Color Pink = Color.FromArgb(255, 142, 211, 255);

        private static readonly Color Black = Colors.Black;

        private readonly Compositor _compositor;
        private readonly CompositionColorGradientStop _topLeftradientStop;
        private readonly CompositionColorGradientStop _bottomRightGradientStop;

        private readonly CompositionColorGradientStop _bottomLeftGradientStop;
        private readonly CompositionColorGradientStop _topRightGradientStop;

        public GradientsWithBlend() : base()
        {
            InitializeComponent();

            _compositor = Window.Current.Compositor;

            _foregroundBrush = _compositor.CreateLinearGradientBrush();
            _foregroundBrush.StartPoint = Vector2.Zero;
            _foregroundBrush.EndPoint = new Vector2(1.0f);

            _bottomRightGradientStop = _compositor.CreateColorGradientStop();
            _bottomRightGradientStop.Offset = 0.5f;
            _bottomRightGradientStop.Color = Green;
            _topLeftradientStop = _compositor.CreateColorGradientStop();
            _topLeftradientStop.Offset = 0.5f;
            _topLeftradientStop.Color = Blue;
            _foregroundBrush.ColorStops.Add(_bottomRightGradientStop);
            _foregroundBrush.ColorStops.Add(_topLeftradientStop);

            _backgroundBrush = _compositor.CreateLinearGradientBrush();
            _backgroundBrush.StartPoint = new Vector2(1.0f, 0);
            _backgroundBrush.EndPoint = new Vector2(0, 1.0f);

            _topRightGradientStop = _compositor.CreateColorGradientStop();
            _topRightGradientStop.Offset = 0.25f;
            _topRightGradientStop.Color = Black;
            _bottomLeftGradientStop = _compositor.CreateColorGradientStop();
            _bottomLeftGradientStop.Offset = 1.0f;
            _bottomLeftGradientStop.Color = Black;
            _backgroundBrush.ColorStops.Add(_topRightGradientStop);
            _backgroundBrush.ColorStops.Add(_bottomLeftGradientStop);

            var graphicsEffect = new BlendEffect()
            {
                Mode = BlendEffectMode.Screen,
                Foreground = new CompositionEffectSourceParameter("Main"),
                Background = new CompositionEffectSourceParameter("Tint"),
            };

            var effectFactory = _compositor.CreateEffectFactory(graphicsEffect);
            var brush = effectFactory.CreateBrush();
            brush.SetSourceParameter("Main", _foregroundBrush);
            brush.SetSourceParameter("Tint", _backgroundBrush);

            _backgroundVisual = _compositor.CreateSpriteVisual();
            _backgroundVisual.Brush = brush;

            ElementCompositionPreview.SetElementChildVisual(Gradient, _backgroundVisual);

            //var leftToRightAnimation = _compositor.CreateScalarKeyFrameAnimation();
            //leftToRightAnimation.Duration = TimeSpan.FromSeconds(2);
            //leftToRightAnimation.DelayTime = TimeSpan.FromSeconds(1);
            //leftToRightAnimation.InsertKeyFrame(1.0f, 1.0f);
            //_bottomRightGradientStop.StartAnimation(nameof(_bottomRightGradientStop.Offset), leftToRightAnimation);

            Loaded += async (s, e) =>
            {
                await Task.Delay(2000);
                UpdateGradients();
                MiddleText.Visibility = Visibility.Collapsed;
                await Task.Delay(2000);
                UpdateText();
                ButtonRoot.Visibility = Visibility.Visible;
            };

            Gradient.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;

                _backgroundVisual.Size = e.NewSize.ToVector2();
                _foregroundBrush.CenterPoint = _backgroundVisual.Size / 2;
                _backgroundBrush.CenterPoint = _backgroundVisual.Size / 2;
            };

            ViewModel.IsInPomodoroChanged += (s, e) =>
              {
                  UpdateGradients();
                  UpdateText();
              };
        }

        private void UpdateText()
        {
            FocusText.Visibility = ViewModel.IsInPomodoro ? Visibility.Visible : Visibility.Collapsed;
            RelaxText.Visibility = ViewModel.IsInPomodoro ? Visibility.Collapsed : Visibility.Visible;
            FocusTextCompact.Visibility = ViewModel.IsInPomodoro ? Visibility.Visible : Visibility.Collapsed;
            RelaxTextCompact.Visibility = ViewModel.IsInPomodoro ? Visibility.Collapsed : Visibility.Visible;
        }

        private void UpdateGradients()
        {
            if (ViewModel.IsInPomodoro)
            {
                StartOffsetAnimation(_bottomRightGradientStop, 0.6f);
                StartColorAnimation(_bottomRightGradientStop, Blue);

                StartOffsetAnimation(_topLeftradientStop, 0f);
                StartColorAnimation(_topLeftradientStop, Green);

                StartOffsetAnimation(_topRightGradientStop, 0.25f);
                StartColorAnimation(_topRightGradientStop, Red);

                StartOffsetAnimation(_bottomLeftGradientStop, 1f);
                StartColorAnimation(_bottomLeftGradientStop, Black);
            }
            else
            {
                StartOffsetAnimation(_bottomRightGradientStop, 1f);
                StartColorAnimation(_bottomRightGradientStop, Green);

                StartOffsetAnimation(_topLeftradientStop, 0.25f);
                StartColorAnimation(_topLeftradientStop, Blue);

                StartOffsetAnimation(_topRightGradientStop, 0f);
                StartColorAnimation(_topRightGradientStop, Red);

                StartOffsetAnimation(_bottomLeftGradientStop, 0.75f);
                StartColorAnimation(_bottomLeftGradientStop, Pink);
            }
        }

        private void StartOffsetAnimation(CompositionColorGradientStop gradientOffset, float offset)
        {
            var offsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
            offsetAnimation.Duration = TimeSpan.FromSeconds(1);
            offsetAnimation.InsertKeyFrame(1.0f, offset);
            gradientOffset.StartAnimation(nameof(CompositionColorGradientStop.Offset), offsetAnimation);
        }

        private void StartColorAnimation(CompositionColorGradientStop gradientOffset, Color color)
        {
            var colorAnimation = _compositor.CreateColorKeyFrameAnimation();
            colorAnimation.Duration = TimeSpan.FromSeconds(2);
            colorAnimation.Direction = Windows.UI.Composition.AnimationDirection.Alternate;
            colorAnimation.InsertKeyFrame(1.0f, color);
            gradientOffset.StartAnimation(nameof(CompositionColorGradientStop.Color), colorAnimation);
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
