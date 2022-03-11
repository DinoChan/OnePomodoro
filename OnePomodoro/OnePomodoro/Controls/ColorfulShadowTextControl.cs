using System;
using System.Numerics;
using Microsoft.Graphics.Canvas.Effects;
using OnePomodoro.ViewModels;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;

namespace OnePomodoro.Controls
{
    public class ColorfulShadowTextControl : ShadowTextControl
    {
        private static readonly Color Black = Colors.Black;
        private static readonly Color Blue = Color.FromArgb(255, 43, 210, 255);
        private static readonly Color Green = Color.FromArgb(255, 43, 255, 136);

        //private static readonly Color Pink = Color.FromArgb(255, 255, 43, 212);
        private static readonly Color Pink = Color.FromArgb(255, 142, 211, 255);

        private static readonly Color Red = Colors.Red;
        private CompositionLinearGradientBrush _backgroundBrush;
        private CompositionColorGradientStop _bottomLeftGradientStop;
        private CompositionColorGradientStop _bottomRightGradientStop;
        private CompositionEffectBrush _brush;
        private Compositor _compositor;
        private CompositionLinearGradientBrush _foregroundBrush;
        private CompositionColorGradientStop _topLeftradientStop;
        private CompositionColorGradientStop _topRightGradientStop;

        public ColorfulShadowTextControl() : base()
        {
            PomodoroViewModel.Current.IsInPomodoroChanged += (s, e) =>
              {
                  UpdateGradients();
              };

            SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize)
                    return;

                _foregroundBrush.CenterPoint = e.NewSize.ToVector2() / 2;
                _backgroundBrush.CenterPoint = e.NewSize.ToVector2() / 2;
            };

            UpdateGradients();
        }

        protected override CompositionBrush CreateBrush()
        {
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
            _brush = effectFactory.CreateBrush();
            _brush.SetSourceParameter("Main", _foregroundBrush);
            _brush.SetSourceParameter("Tint", _backgroundBrush);
            return _brush;
        }

        private void StartColorAnimation(CompositionColorGradientStop gradientOffset, Color color)
        {
            var colorAnimation = _compositor.CreateColorKeyFrameAnimation();
            colorAnimation.Duration = TimeSpan.FromSeconds(2);
            colorAnimation.Direction = Windows.UI.Composition.AnimationDirection.Alternate;
            colorAnimation.InsertKeyFrame(1.0f, color);
            gradientOffset.StartAnimation(nameof(CompositionColorGradientStop.Color), colorAnimation);
        }

        private void StartOffsetAnimation(CompositionColorGradientStop gradientOffset, float offset)
        {
            var offsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
            offsetAnimation.Duration = TimeSpan.FromSeconds(1);
            offsetAnimation.InsertKeyFrame(1.0f, offset);
            gradientOffset.StartAnimation(nameof(CompositionColorGradientStop.Offset), offsetAnimation);
        }

        private void UpdateGradients()
        {
            if (PomodoroViewModel.Current.IsInPomodoro)
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
    }
}
