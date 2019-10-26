using OnePomodoro.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace OnePomodoro.PomodoroViews
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    [Title("SpringTextView")]
    [Screenshot("/Assets/Screenshots/LongShadow.png")]
    public sealed partial class SpringTextView : PomodoroView
    {
        private readonly Compositor _compositor;
        private readonly CompositionColorBrush _colorBursh;
        private static readonly Color Blue = Color.FromArgb(255, 44, 56, 82);
        private static readonly Color Red = Color.FromArgb(255, 81, 44, 81);
        private readonly Visual _focusVisual;
        private readonly Visual _relaxVisual;

        public SpringTextView()
        {
            this.InitializeComponent();
            _compositor = Window.Current.Compositor;
            _colorBursh = _compositor.CreateColorBrush(Colors.Black);
            var backgroundVisual = _compositor.CreateSpriteVisual();
            backgroundVisual.Brush = _colorBursh;
            ElementCompositionPreview.SetElementChildVisual(RootBackground, backgroundVisual);

            _focusVisual = ElementCompositionPreview.GetElementVisual(FocusElement);
            _relaxVisual = ElementCompositionPreview.GetElementVisual(RelaxElement);

            RootBackground.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize)
                    return;

                backgroundVisual.Size = e.NewSize.ToVector2();


            };

            ContentArea.SizeChanged += (s, e) =>
            {
                _focusVisual.CenterPoint = new Vector3(e.NewSize.ToVector2() / 2, 0);
                _relaxVisual.CenterPoint = new Vector3(e.NewSize.ToVector2() / 2, 0);
            };

            ViewModel.IsInPomodoroChanged += (s, e) =>
            {
                UpdateBackground();
                UpdateOffset();
            };

            Loaded += (s, e) =>
            {
                UpdateBackground();
                UpdateOffset();
            };
        }

        private void UpdateBackground()
        {
            if (ViewModel.IsInPomodoro)
                StartColorAnimation(Red);
            else
                StartColorAnimation(Blue);
        }

        private void UpdateOffset()
        {
            if (ViewModel.IsInPomodoro)
            {
                StartOffsetAnimation(_focusVisual, 0);
                StartOffsetAnimation(_relaxVisual, (float)(Root.ActualWidth + RelaxElement.ActualWidth) / 2);
            }
            else
            {
                StartOffsetAnimation(_focusVisual, -(float)(Root.ActualWidth + RelaxElement.ActualWidth) / 2);
                StartOffsetAnimation(_relaxVisual, 0);
            }
        }

        private void StartColorAnimation(Color color)
        {
            var colorAnimation = _compositor.CreateColorKeyFrameAnimation();
            colorAnimation.Duration = TimeSpan.FromSeconds(2);
            colorAnimation.Direction = Windows.UI.Composition.AnimationDirection.Alternate;
            colorAnimation.InsertKeyFrame(1.0f, color);
            _colorBursh.StartAnimation(nameof(_colorBursh.Color), colorAnimation);
        }


        private void StartOffsetAnimation(Visual visual, float offsetX)
        {
            var springAnimation = _compositor.CreateSpringVector3Animation();
            springAnimation.DampingRatio = 0.5f;
            springAnimation.Period = TimeSpan.FromMilliseconds(200);
            springAnimation.FinalValue = new Vector3(offsetX, 0, 0);
            visual.StartAnimation(nameof(visual.Offset), springAnimation);
        }
    }
}
