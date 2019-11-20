using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
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



        private async void ShowTextShimming()
        {
            if (_inworkPointLight != null)
                return;

            _inworkPointLight = CreatePointLightAndStartAnimation(Color.FromArgb(255, 217, 17, 83), TimeSpan.Zero);
            _breakPointLight = CreatePointLightAndStartAnimation(Color.FromArgb(255, 0, 27, 171), TimeSpan.FromSeconds(1));
            var rootVisual = VisualExtensions.GetVisual(this);
            var buttonVisual = VisualExtensions.GetVisual(StateButton);
            var focusVisual = VisualExtensions.GetVisual(FocusPanel);
            var relayVisual = VisualExtensions.GetVisual(RelaxPanel);


            //_inworkPointLight.Targets.Add(focusVisual);
            //_breakPointLight.Targets.Add(focusVisual);

            //var compositor = Window.Current.Compositor;
            //var titleVisual = VisualExtensions.GetVisual(this);
            //var pointLight = compositor.CreatePointLight();
            //pointLight.CoordinateSpace = titleVisual;
            //pointLight.Color = Colors.Transparent;
            //pointLight.Targets.Add(focusVisual);
            //pointLight.Targets.Add(relayVisual);
            //pointLight.Offset = new Vector3(-100000, 0, 0);
            RelaxPanel.Visibility = Visibility.Collapsed;



            var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            var canvasDevice = CanvasDevice.GetSharedDevice();
            var graphicsDevice = CanvasComposition.CreateCompositionGraphicsDevice(compositor, canvasDevice);

            var bitmap = await CanvasBitmap.LoadAsync(canvasDevice, new Uri("ms-appx:///Assets/flutter.png", UriKind.RelativeOrAbsolute));

            var drawingSurface = graphicsDevice.CreateDrawingSurface(bitmap.Size,
                DirectXPixelFormat.B8G8R8A8UIntNormalized, DirectXAlphaMode.Premultiplied);
            using (var ds = CanvasComposition.CreateDrawingSession(drawingSurface))
            {
                ds.Clear(Colors.Transparent);
                ds.DrawImage(bitmap);
            }

            var surfaceBrush = compositor.CreateSurfaceBrush(drawingSurface);
            surfaceBrush.Stretch = CompositionStretch.None;

            var border = new BorderEffect
            {
                ExtendX = CanvasEdgeBehavior.Wrap,
                ExtendY = CanvasEdgeBehavior.Wrap,
                Source = new CompositionEffectSourceParameter("source")
            };

            var fxFactory = compositor.CreateEffectFactory(border);
            var fxBrush = fxFactory.CreateBrush();
            fxBrush.SetSourceParameter("source", surfaceBrush);

            var sprite = compositor.CreateSpriteVisual();
            sprite.Size = new Vector2(1000000);
            sprite.Brush = fxBrush;

            ElementCompositionPreview.SetElementChildVisual(Root, sprite);

            var backgroundVisual = VisualExtensions.GetVisual(Root);
            _inworkPointLight.Targets.Add(rootVisual);
            _breakPointLight.Targets.Add(rootVisual);
            _breakPointLight.Targets.Add(backgroundVisual);

            var ambientLight = compositor.CreateAmbientLight();
            ambientLight.Color = Colors.White;
            ambientLight.Targets.Add(backgroundVisual);
        }


        private PointLight CreatePointLightAndStartAnimation(Color color, TimeSpan delay)
        {
            var compositor = Window.Current.Compositor;
            var titleVisual = VisualExtensions.GetVisual(this);
            var pointLight = compositor.CreatePointLight();

            pointLight.Color = color;
            pointLight.CoordinateSpace = titleVisual;
            //pointLight.Targets.Add(titleVisual);
            pointLight.Offset = new Vector3(-(float)PomodoroPanel.ActualWidth * 2, (float)PomodoroPanel.ActualHeight / 2, 300.0f);

            var offsetAnimation = compositor.CreateScalarKeyFrameAnimation();
            offsetAnimation.InsertKeyFrame(1.0f, (float)PomodoroPanel.ActualWidth * 3, compositor.CreateLinearEasingFunction());
            offsetAnimation.Duration = TimeSpan.FromSeconds(10);
            offsetAnimation.DelayTime = delay;
            offsetAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

            pointLight.StartAnimation("Offset.X", offsetAnimation);
            return pointLight;
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
            ElementCompositionPreview.SetElementChildVisual(this, spriteTextVisual);
        }
    }



}
