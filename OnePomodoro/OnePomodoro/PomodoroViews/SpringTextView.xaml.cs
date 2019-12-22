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
    [Title("SpringText")]
    [Screenshot("/Assets/Screenshots/SpringText.png")]
    [FunctionTags(Tags.SpringAnimation)]
    [CompactOverlay]
    [SourceCode("https://github.com/DinoChan/OnePomodoro/blob/master/OnePomodoro/OnePomodoro/PomodoroViews/SpringTextView.xaml.cs")]
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
                UpdateOffset();
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
            var y = 0;
            if (ViewModel.IsInPomodoro)
            {
                StartOffsetAnimation(_focusVisual, new Vector2(0, y));
                StartOffsetAnimation(_relaxVisual, new Vector2((float)(ContentArea.ActualWidth * 3), y));
            }
            else
            {
                StartOffsetAnimation(_focusVisual, new Vector2(-(float)(ContentArea.ActualWidth) * 3, y));
                StartOffsetAnimation(_relaxVisual, new Vector2(0, y));
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


        private void StartOffsetAnimation(Visual visual, Vector2 offset)
        {
            StartOffsetAnimation(visual, offset, TimeSpan.FromMilliseconds(200));
        }

        private void StartOffsetAnimation(Visual visual, Vector2 offset, TimeSpan period)
        {
            var springAnimation = _compositor.CreateSpringVector3Animation();
            springAnimation.DampingRatio = 0.5f;
            springAnimation.Period = period;
            springAnimation.FinalValue = new Vector3(offset, 0);
            visual.StartAnimation(nameof(visual.Offset), springAnimation);
        }
    }
}
