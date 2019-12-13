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
    [Title("konosuba")]
    [Screenshot("/Assets/Screenshots/Konosuba.png")]
    [CompactOverlay(CustomWidth = 288, CustomHeight = 157.5)]
    [FunctionTags(Tags.SpringAnimation, Tags.Clip)]
    public sealed partial class KonosubaView : PomodoroView
    {
        private readonly Compositor _compositor;
        private readonly Visual _focusTopVisual;
        private readonly Visual _relaxTopVisual;
        private readonly Visual _focusBottomVisual;
        private readonly Visual _relaxBottomVisual;

        public KonosubaView()
        {
            this.InitializeComponent();
            _compositor = Window.Current.Compositor;

            _focusTopVisual = ElementCompositionPreview.GetElementVisual(FocusElementTop);
            _focusBottomVisual = ElementCompositionPreview.GetElementVisual(FocusElementBottom);
            _relaxTopVisual = ElementCompositionPreview.GetElementVisual(RelaxElementTop);
            _relaxBottomVisual = ElementCompositionPreview.GetElementVisual(RelaxElementBottom);

            ContentArea.SizeChanged += (s, e) =>
            {
                UpdateOffsetUsingAnimation();
            };

            ViewModel.IsInPomodoroChanged += (s, e) =>
            {
                UpdateOffsetUsingAnimation();
            };

            Loaded += (s, e) =>
            {
                UpdateOffsetUsingAnimation();
            };

            UpdateOffset();
        }

        private void UpdateOffsetUsingAnimation()
        {
            var width = (float)ContentArea.Width;
            if (ViewModel.IsInPomodoro)
            {
                StartOffsetAnimation(_focusTopVisual, 0);
                StartOffsetAnimation(_focusBottomVisual, 0);
                StartOffsetAnimation(_relaxTopVisual, -2 * width);
                StartOffsetAnimation(_relaxBottomVisual, 2 * width);
            }
            else
            {
                StartOffsetAnimation(_focusTopVisual, 2 * width);
                StartOffsetAnimation(_focusBottomVisual, -2 * width);
                StartOffsetAnimation(_relaxTopVisual, 0);
                StartOffsetAnimation(_relaxBottomVisual, 0);
            }
        }

        private void UpdateOffset()
        {
            var width = (float)ContentArea.Width;
            if (ViewModel.IsInPomodoro)
            {
                _focusTopVisual.Offset = new Vector3(0, 0, 0);
                _focusBottomVisual.Offset = new Vector3(0, 0, 0);
                _relaxTopVisual.Offset = new Vector3(0, -2 * width, 0);
                _relaxBottomVisual.Offset = new Vector3(0, 2 * width, 0);
            }
            else
            {
                _focusTopVisual.Offset = new Vector3(0, 2 * width, 0);
                _focusBottomVisual.Offset = new Vector3(0, -2 * width, 0);
                _relaxTopVisual.Offset = new Vector3(0, 0, 0);
                _relaxBottomVisual.Offset = new Vector3(0, 0, 0);
            }
        }

        private void StartOffsetAnimation(Visual visual, float offsetX)
        {
            var springAnimation = _compositor.CreateSpringVector3Animation();
            springAnimation.DampingRatio = 0.85f;
            springAnimation.Period = TimeSpan.FromMilliseconds(50);
            springAnimation.FinalValue = new Vector3(offsetX, 0, 0);
            visual.StartAnimation(nameof(visual.Offset), springAnimation);
        }
    }
}
