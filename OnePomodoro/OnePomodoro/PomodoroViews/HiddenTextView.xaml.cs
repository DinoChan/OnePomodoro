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
    [Title("Hidden Text")]
    [Screenshot("/Assets/Screenshots/HiddenTextView.png")]
    public sealed partial class HiddenTextView : PomodoroView
    {
        private PointLight _redLight;
        private PointLight _blueLight;

        public HiddenTextView()
        {
            this.InitializeComponent();
            OnIsInPomodoroChanged();
            Loaded += OnLoaded;
            ViewModel.IsInPomodoroChanged += (s, e) => OnIsInPomodoroChanged();
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            await ShowTextShimmingAsync();
            OnIsInPomodoroChanged();
        }

        private void OnIsInPomodoroChanged()
        {
            FocusPanel.Visibility = ViewModel.IsInPomodoro ? Visibility.Visible : Visibility.Collapsed;
            RelaxPanel.Visibility = ViewModel.IsInPomodoro ? Visibility.Collapsed : Visibility.Visible;
        }

        private async Task ShowTextShimmingAsync()
        {
            if (_redLight != null)
                return;

            _redLight = CreatePointLightAndStartAnimation(Color.FromArgb(255, 217, 17, 83), TimeSpan.Zero);
            _blueLight = CreatePointLightAndStartAnimation(Color.FromArgb(255, 0, 27, 171), TimeSpan.FromSeconds(0.5));
            var rootVisual = VisualExtensions.GetVisual(this);
            var buttonVisual = VisualExtensions.GetVisual(StateButton);
            var focusVisual = VisualExtensions.GetVisual(FocusPanel);
            var relayVisual = VisualExtensions.GetVisual(RelaxPanel);


            _redLight.Targets.Add(focusVisual);
            _blueLight.Targets.Add(focusVisual);

            _redLight.Targets.Add(relayVisual);
            _blueLight.Targets.Add(relayVisual);

            _redLight.Targets.Add(buttonVisual);
            _blueLight.Targets.Add(buttonVisual);

            //var compositor = Window.Current.Compositor;
            //var titleVisual = VisualExtensions.GetVisual(this);
            //var pointLight = compositor.CreatePointLight();
            //pointLight.CoordinateSpace = titleVisual;
            //pointLight.Color = Colors.Transparent;
            //pointLight.Targets.Add(focusVisual);
            //pointLight.Targets.Add(relayVisual);
            //pointLight.Offset = new Vector3(-100000, 0, 0);
            RelaxPanel.Visibility = Visibility.Collapsed;



            await InitializeBackgroundAsync();


            ////var backgroundVisual = VisualExtensions.GetVisual(Root);
            //_inworkPointLight.Targets.Add(rootVisual);
            //_breakPointLight.Targets.Add(rootVisual);
            ////_breakPointLight.Targets.Add(backgroundVisual);

            ////var ambientLight = compositor.CreateAmbientLight();
            ////ambientLight.Color = Colors.White;
            ////ambientLight.Targets.Add(backgroundVisual);
        }

        private async Task InitializeBackgroundAsync()
        {
            var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;

            var imageBrush = compositor.CreateSurfaceBrush();
            var loadedSurface = LoadedImageSurface.StartLoadFromUri(new Uri("ms-appx:///Assets/flutter.png"));
            imageBrush.Surface = loadedSurface;
            imageBrush.Stretch = CompositionStretch.None;

            var borderEffect = new BorderEffect
            {
                ExtendX = CanvasEdgeBehavior.Wrap,
                ExtendY = CanvasEdgeBehavior.Wrap,
                Source = new CompositionEffectSourceParameter("source")
            };

            var effectFactory = compositor.CreateEffectFactory(borderEffect);
            var effectBrush = effectFactory.CreateBrush();
            effectBrush.SetSourceParameter("source", imageBrush);

            var sprite = compositor.CreateSpriteVisual();
            sprite.Brush = effectBrush;

            var backgroundVisual = VisualExtensions.GetVisual(Background);
            var bindSizeAnimation = compositor.CreateExpressionAnimation("backgroundVisual.Size");
            bindSizeAnimation.SetReferenceParameter("backgroundVisual", backgroundVisual);
            sprite.StartAnimation("Size", bindSizeAnimation);

            ElementCompositionPreview.SetElementChildVisual(Background, sprite);

        }


        private PointLight CreatePointLightAndStartAnimation(Color color, TimeSpan delay)
        {
            var width = 1920f;
            var height = 1050f;
            var compositor = Window.Current.Compositor;
            var titleVisual = VisualExtensions.GetVisual(this);
            var pointLight = compositor.CreatePointLight();

            pointLight.Color = color;
            pointLight.CoordinateSpace = titleVisual;
            //pointLight.Targets.Add(titleVisual);
            pointLight.Offset = new Vector3(-width * 3, height / 2, 150.0f);

            var offsetAnimation = compositor.CreateScalarKeyFrameAnimation();
            offsetAnimation.InsertKeyFrame(1.0f, width * 4, compositor.CreateLinearEasingFunction());
            offsetAnimation.Duration = TimeSpan.FromSeconds(10);
            offsetAnimation.DelayTime = delay;
            offsetAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

            pointLight.StartAnimation("Offset.X", offsetAnimation);
            return pointLight;
        }


    }

}
