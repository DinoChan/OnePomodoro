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
    [Screenshot("/Assets/Screenshots/TheFirst.png")]
    public sealed partial class LongShadow : PomodoroView
    {

        public LongShadow()
        {
            InitializeComponent();
            MackLongShadow(108, 0.3f, InWorkCountDown, InworkBackground, Color.FromArgb(255, 232, 122, 105));
            MackLongShadow(108, 0.3f, BreakCountDown, BreakBackground, Color.FromArgb(255, 82, 113, 194));

            var rootVisual = ElementCompositionPreview.GetElementVisual(Root);
            var v = rootVisual.Orientation;
            rootVisual.CenterPoint = new Vector3(400, 0, 0);
            rootVisual.BackfaceVisibility =  CompositionBackfaceVisibility.Hidden;
            Loaded += (S, E) =>
            {

                var relaxGradientStopOffsetAnimation = rootVisual.Compositor.CreateQuaternionKeyFrameAnimation();
                relaxGradientStopOffsetAnimation.Duration = TimeSpan.FromSeconds(5);
                // relaxGradientStopOffsetAnimation.DelayTime = TimeSpan.FromSeconds(2);
                relaxGradientStopOffsetAnimation.InsertKeyFrame(1.0f, new Quaternion(0,2f,0,1));
                rootVisual.StartAnimation(nameof(rootVisual.Orientation), relaxGradientStopOffsetAnimation);


            };
        }


        private void MackLongShadow(int depth, float opacity, TextBlock textElement, FrameworkElement shadowElement, Color color)
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
