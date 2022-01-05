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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace OnePomodoro.PomodoroViews
{
    [Title("LongShadow")]
    [Screenshot("/Assets/Screenshots/LongShadow.png")]
    [FunctionTags(Tags.ContainerVisual, Tags.ExpressionAnimation, Tags.CompositionMaskBrush, Tags.RotationAngle, Tags.TransformMatrix)]
    [CompactOverlay]
    [SourceCode("https://github.com/DinoChan/OnePomodoro/blob/master/OnePomodoro/OnePomodoro/PomodoroViews/LongShadow.xaml.cs")]
    public sealed partial class LongShadow : PomodoroView
    {

        public LongShadow()
        {
            InitializeComponent();
            MakeLongShadow(188, 0.3f, InWorkCountDown, InworkBackground, Color.FromArgb(255, 250, 110, 93));
            MakeLongShadow(188, 0.3f, InWorkCountDownSecond, InworkSecondBackground, Color.FromArgb(255, 250, 110, 93));

            MakeLongShadow(188, 0.3f, BreakCountDown, BreakBackground, Color.FromArgb(255, 82, 113, 194));
            MakeLongShadow(188, 0.3f, BreakCountDownSecond, BreakSecondBackground, Color.FromArgb(255, 82, 113, 194));

            MakeLongShadow(188, 0.3f, CompactInWorkCountDown, CompactInworkBackground, Color.FromArgb(255, 250, 110, 93));
            MakeLongShadow(188, 0.3f, CompactInWorkCountDownSecond, CompactInworkSecondBackground, Color.FromArgb(255, 255, 110, 93));

            MakeLongShadow(188, 0.3f, CompactBreakCountDown, CompactBreakBackground, Color.FromArgb(255, 82, 113, 194));
            MakeLongShadow(188, 0.3f, CompactBreakCountDownSecond, CompactBreakSecondBackground, Color.FromArgb(255, 82, 113, 194));

            ViewModel.IsInPomodoroChanged += (s, e) =>
              {
                  FlipSide.IsFlipped = ViewModel.IsInPomodoro == false;
                  CompactFlipSide.IsFlipped= ViewModel.IsInPomodoro == false;
              };
        }


        private void MakeLongShadow(int depth, float opacity, TextBlock textElement, FrameworkElement shadowElement, Color color)
        {
            var textVisual = ElementCompositionPreview.GetElementVisual(textElement);
            var compositor = textVisual.Compositor;
            var containerVisual = compositor.CreateContainerVisual();
            var mask = textElement.GetAlphaMask();
            Vector3 background = new Vector3(color.R, color.G, color.B);
            for (int i = 0; i < depth; i++)
            {
                var maskBrush = compositor.CreateMaskBrush();
                var shadowColor = background - ((background - new Vector3(0, 0, 0)) * opacity);
                shadowColor = Vector3.Max(Vector3.Zero, shadowColor);
                shadowColor += (background - shadowColor) * i / depth;
                maskBrush.Mask = mask;
                maskBrush.Source = compositor.CreateColorBrush(Color.FromArgb(255, (byte)shadowColor.X, (byte)shadowColor.Y, (byte)shadowColor.Z));
                var visual = compositor.CreateSpriteVisual();
                visual.Brush = maskBrush;
                visual.Offset = new Vector3(i + 1, i + 1, 0);
                var bindSizeAnimation = compositor.CreateExpressionAnimation("textVisual.Size");
                bindSizeAnimation.SetReferenceParameter("textVisual", textVisual);
                visual.StartAnimation("Size", bindSizeAnimation);

                containerVisual.Children.InsertAtBottom(visual);
            }

            ElementCompositionPreview.SetElementChildVisual(shadowElement, containerVisual);

            //void OnLoaded(object sender, RoutedEventArgs e)
            //{
            //    root.Loaded -= OnLoaded;
            //    var geometry = compositor.CreateRectangleGeometry();
            //    geometry.Size = new Vector2((float)root.RenderSize.Width, (float)root.RenderSize.Height);
            //    var clip = compositor.CreateGeometricClip(geometry);
            //    containerVisual.Clip = clip;
            //}
            //root.Loaded += OnLoaded;
        }
    }
}
