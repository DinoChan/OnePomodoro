using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Composition;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
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
    [Title("Split To 5")]
    [Screenshot("/Assets/Screenshots/SplitTo5.png")]
    public sealed partial class SplitTo5View : PomodoroView
    {
        private readonly Compositor _compositor;
        private readonly CompositionColorBrush _colorBursh;

        private Color Red = Color.FromArgb(255, 248, 169, 162);
        private Color Blue = Color.FromArgb(255, 140, 220, 247);


        private Visual _contentAeraVisual;
        private List<Visual> _workVisuals;
        private List<Visual> _breakVisuals;

        public SplitTo5View()
        {
            this.InitializeComponent();
            _compositor = Window.Current.Compositor;

            _colorBursh = _compositor.CreateColorBrush(Colors.Black);
            var backgroundVisual = _compositor.CreateSpriteVisual();
            backgroundVisual.Brush = _colorBursh;
            ElementCompositionPreview.SetElementChildVisual(RootBackground, backgroundVisual);

            RootBackground.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize)
                    return;

                backgroundVisual.Size = e.NewSize.ToVector2();
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

            _contentAeraVisual = ElementCompositionPreview.GetElementVisual(ContentAera);

            _workVisuals = new List<Visual>();
            _workVisuals.Add(ElementCompositionPreview.GetElementVisual(WorkSection1));
            _workVisuals.Add(ElementCompositionPreview.GetElementVisual(WorkSection2));
            _workVisuals.Add(ElementCompositionPreview.GetElementVisual(WorkSection3));
            _workVisuals.Add(ElementCompositionPreview.GetElementVisual(WorkSection4));
            _workVisuals.Add(ElementCompositionPreview.GetElementVisual(WorkSection5));

            _breakVisuals = new List<Visual>();
            _breakVisuals.Add(ElementCompositionPreview.GetElementVisual(BreakSection5));
            _breakVisuals.Add(ElementCompositionPreview.GetElementVisual(BreakSection4));
            _breakVisuals.Add(ElementCompositionPreview.GetElementVisual(BreakSection3));
            _breakVisuals.Add(ElementCompositionPreview.GetElementVisual(BreakSection2));
            _breakVisuals.Add(ElementCompositionPreview.GetElementVisual(BreakSection1));
        }

        private void UpdateOffset()
        {
            var y = 0;
            var width = 1920f;
            var sectionWidth = 320f;
            if (ViewModel.IsInPomodoro)
            {
                StartOffsetAnimation(_contentAeraVisual, new Vector2(0, y));
                for (int i = 0; i < _workVisuals.Count; i++)
                {
                    StartOffsetAnimation(_workVisuals[i], new Vector2(0, y), TimeSpan.FromMilliseconds(40 * (1 + i)));
                }

                for (int i = 0; i < _breakVisuals.Count; i++)
                {
                    StartOffsetAnimation(_breakVisuals[i], new Vector2(width * 2, y), TimeSpan.FromMilliseconds(40 * (1 + i)));
                }
            }
            else
            {
                StartOffsetAnimation(_contentAeraVisual, new Vector2(-width + sectionWidth, y));

                for (int i = 0; i < _workVisuals.Count; i++)
                {
                    StartOffsetAnimation(_workVisuals[i], new Vector2(-width, y), TimeSpan.FromMilliseconds(40 * (1 + i)));
                }

                for (int i = 0; i < _breakVisuals.Count; i++)
                {
                    StartOffsetAnimation(_breakVisuals[i], new Vector2(0, y), TimeSpan.FromMilliseconds(40 * (1 + i)));
                }
            }
        }



        private void UpdateBackground()
        {
            if (ViewModel.IsInPomodoro)
                StartColorAnimation(Red);
            else
                StartColorAnimation(Blue);
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
            StartOffsetAnimation(visual, offset, TimeSpan.FromMilliseconds(50));
        }

        private void StartOffsetAnimation(Visual visual, Vector2 offset, TimeSpan period)
        {
            var springAnimation = _compositor.CreateSpringVector3Animation();
            springAnimation.DampingRatio = 0.85f;
            springAnimation.Period = period;
            springAnimation.FinalValue = new Vector3(offset, 0);
            visual.StartAnimation(nameof(visual.Offset), springAnimation);
        }
    }

}
