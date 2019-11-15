using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Composition;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using OnePomodoro.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.DirectX;
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
    [Title("Outline Text")]
    [Screenshot("/Assets/Screenshots/OutlineTextView.png")]
    public sealed partial class OutlineTextView : PomodoroView
    {
        private PointLight _inworkPointLight;
        private PointLight _breakPointLight;

        public OutlineTextView()
        {
            this.InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            //await LayoutRoot.Fade(0.0f, duration: 0, delay: 0, easingMode: Windows.UI.Xaml.Media.Animation.EasingMode.EaseIn).StartAsync();
            //await LayoutRoot.Fade(1.0f, duration: 1500, delay: 0).StartAsync();
            ShowTextShimming();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            StopTextShimming();
        }



        private void ShowTextShimming()
        {
            if (_inworkPointLight != null)
                return;

            var compositor = Window.Current.Compositor;
            if (_inworkPointLight == null)
            {
                var titleVisual = VisualExtensions.GetVisual(InworkShadow);
                _inworkPointLight = compositor.CreatePointLight();

                _inworkPointLight.Color = Color.FromArgb(255, 217, 17, 83);
                _inworkPointLight.CoordinateSpace = titleVisual;
                _inworkPointLight.Targets.Add(titleVisual);
                _inworkPointLight.Offset = new Vector3(-(float)PomodoroPanel.ActualWidth * 3, (float)PomodoroPanel.ActualHeight / 2, 300.0f);
            }
            var offsetAnimation = compositor.CreateScalarKeyFrameAnimation();
            offsetAnimation.InsertKeyFrame(1.0f, (float)PomodoroPanel.ActualWidth * 3, compositor.CreateLinearEasingFunction());
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(10000);
            offsetAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

            _inworkPointLight.StartAnimation("Offset.X", offsetAnimation);



            if (_breakPointLight == null)
            {
                var titleVisual = VisualExtensions.GetVisual(BreakShadow);
                _breakPointLight = compositor.CreatePointLight();

                _breakPointLight.Color = Color.FromArgb(255, 217, 17, 83);
                _breakPointLight.CoordinateSpace = titleVisual;
                _breakPointLight.Targets.Add(titleVisual);
                _breakPointLight.Offset = new Vector3(-(float)PomodoroPanel.ActualWidth * 3, (float)PomodoroPanel.ActualHeight / 2, 300.0f);
            }
            offsetAnimation = compositor.CreateScalarKeyFrameAnimation();
            offsetAnimation.InsertKeyFrame(1.0f, (float)PomodoroPanel.ActualWidth * 3, compositor.CreateLinearEasingFunction());
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(10000);
            offsetAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

            _breakPointLight.StartAnimation("Offset.X", offsetAnimation);
        }

        private void StopTextShimming()
        {
            //_inworkPointLight?.StopAnimation("Offset.X");
            //_breakPointLight?.StopAnimation("Offset.X");
        }

        private void DrawText(CompositionDrawingSurface surface, string text, float fontSize)
        {
            using (var session = CanvasComposition.CreateDrawingSession(surface))
            {
                session.Clear(Colors.Transparent);
                using (var textFormat = new CanvasTextFormat()
                {
                    FontSize = fontSize,
                    Direction = CanvasTextDirection.LeftToRightThenTopToBottom,
                    VerticalAlignment = CanvasVerticalAlignment.Center,
                    HorizontalAlignment = CanvasHorizontalAlignment.Center,

                })
                {
                    using (var textLayout = new CanvasTextLayout(session, text, textFormat, 500f, 500f))
                    {
                        using (var textGeometry = CanvasGeometry.CreateText(textLayout))
                        {
                            float strokeWidth = 1f;

                            session.DrawGeometry(textGeometry, Colors.White, strokeWidth, dashedStroke);
                        }
                    }
                }
            }
        }

        CanvasTextLayout textLayout;
        CanvasGeometry textGeometry;
        CanvasStrokeStyle dashedStroke = new CanvasStrokeStyle()
        {
            DashStyle = CanvasDashStyle.Solid
        };



        private void CreateColoredText(string text)
        {
            var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            var graphicsDevice = CanvasComposition.CreateCompositionGraphicsDevice(compositor, CanvasDevice.GetSharedDevice());

            var spriteTextVisual = compositor.CreateSpriteVisual();
            spriteTextVisual.Size = new Vector2(512, 512);

            var maskDrawingSurface = graphicsDevice.CreateDrawingSurface(new Size(512, 512), DirectXPixelFormat.B8G8R8A8UIntNormalized, DirectXAlphaMode.Premultiplied);

            DrawText(maskDrawingSurface, "this is a test", 40f);

            var maskSurfaceBrush = compositor.CreateSurfaceBrush(maskDrawingSurface);
            var surfaceTextBrush = compositor.CreateColorBrush(Colors.DeepPink);
            var maskBrush = compositor.CreateMaskBrush();
            maskBrush.Mask = maskSurfaceBrush;
            maskBrush.Source = surfaceTextBrush;


            spriteTextVisual.Brush = maskBrush;
            ElementCompositionPreview.SetElementChildVisual(InworkShadow, spriteTextVisual);
        }
    }



}
