using System;
using System.Numerics;
using OnePomodoro.Helpers;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板
/// <summary>
/// https://docs.microsoft.com/en-us/uwp/api/windows.ui.composition.compositionellipsegeometry
/// https://docs.microsoft.com/en-us/uwp/api/windows.ui.composition.compositor.createellipsegeometry
/// </summary>
namespace OnePomodoro.PomodoroViews
{
    [Title("Ellipse Clip")]
    [Screenshot("/Assets/Screenshots/EllipseClip.png")]
    [CompactOverlay(CustomWidth = 288, CustomHeight = 157.5)]
    [FunctionTags(Tags.CompositionAnimation, Tags.CompositionGeometricClip)]
    [SourceCode("https://github.com/DinoChan/OnePomodoro/blob/master/OnePomodoro/OnePomodoro/PomodoroViews/EllipseClipView.xaml.cs")]
    public sealed partial class EllipseClipView : PomodoroView
    {
        private Visual _oldVisual;

        public EllipseClipView()
        {
            this.InitializeComponent();
            ViewModel.IsInPomodoroChanged += OnIsPomodoroChanged;
            UpdateContent();
        }

        private Compositor Compositor => Window.Current.Compositor;

        private void OnIsPomodoroChanged(object sender, EventArgs e)
        {
            UpdateContent();
        }

        private void UpdateContent()
        {
            var width = (float)ActualWidth;
            var newContent = ViewModel.IsInPomodoro ? (UIElement)new EllipseClipWorkControl() : new EllipseClipPlayControl();
            Root.Children.Add(newContent);
            var visual = ElementCompositionPreview.GetElementVisual(newContent);
            visual.Offset = new Vector3(width, 0, 0);
            var animation = Compositor.CreateVector3KeyFrameAnimation();
            var easing = Compositor.CreateCubicBezierEasingFunction(new Vector2(0.1f, 0.9f), new Vector2(0.2f, 1f));

            animation.Duration = TimeSpan.FromSeconds(1);
            animation.InsertKeyFrame(1, Vector3.Zero, easing);
            visual.StartAnimation(nameof(Visual.Offset), animation);

            if (_oldVisual != null)
            {
                animation = Compositor.CreateVector3KeyFrameAnimation();
                easing = Compositor.CreateCubicBezierEasingFunction(new Vector2(0.1f, 0.9f), new Vector2(0.2f, 1f));

                animation.Duration = TimeSpan.FromSeconds(1);
                animation.InsertKeyFrame(1, new Vector3(-width, 0, 0), easing);

                CompositionScopedBatch batch = Compositor.CreateScopedBatch(CompositionBatchTypes.Animation);

                _oldVisual.StartAnimation(nameof(Visual.Offset), animation);

                batch.Completed += (s, e) => Root.Children.RemoveAt(0);
                batch.End();
            }

            _oldVisual = visual;
        }
    }
}
