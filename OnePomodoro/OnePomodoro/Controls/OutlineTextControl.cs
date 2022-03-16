using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Composition;
using Windows.Graphics.DirectX;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;

namespace OnePomodoro.Controls
{
    public class OutlineTextControl : Control
    {
        /// <summary>
        /// 标识 StrokeStyle 依赖属性。
        /// </summary>
        public static readonly DependencyProperty DashStyleProperty =
            DependencyProperty.Register(nameof(DashStyle), typeof(CanvasDashStyle), typeof(OutlineTextControl), new PropertyMetadata(default(CanvasDashStyle), OnDashStyleChanged));

        /// <summary>
        /// 标识 FontColor 依赖属性。
        /// </summary>
        public static readonly DependencyProperty FontColorProperty =
            DependencyProperty.Register(nameof(FontColor), typeof(Color), typeof(OutlineTextControl), new PropertyMetadata(Colors.Black, OnFontColorChanged));

        /// <summary>
        /// 标识 OutlineColor 依赖属性。
        /// </summary>
        public static readonly DependencyProperty OutlineColorProperty =
            DependencyProperty.Register(nameof(OutlineColor), typeof(Color), typeof(OutlineTextControl), new PropertyMetadata(Colors.Black, OnOutlineColorChanged));

        /// <summary>
        /// 标识 ShowNonOutlineText 依赖属性。
        /// </summary>
        public static readonly DependencyProperty ShowNonOutlineTextProperty =
            DependencyProperty.Register(nameof(ShowNonOutlineText), typeof(bool), typeof(OutlineTextControl), new PropertyMetadata(default(bool), OnShowNonOutlineTextChanged));

        /// <summary>
        /// 标识 StrokeWidth 依赖属性。
        /// </summary>
        public static readonly DependencyProperty StrokeWidthProperty =
            DependencyProperty.Register(nameof(StrokeWidth), typeof(double), typeof(OutlineTextControl), new PropertyMetadata(default(double), OnStrokeWidthChanged));

        /// <summary>
        /// 标识 Text 依赖属性。
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(OutlineTextControl), new PropertyMetadata(default(string), OnTextChanged));

        public OutlineTextControl()
        {
            var graphicsDevice = CanvasComposition.CreateCompositionGraphicsDevice(Compositor, CanvasDevice.GetSharedDevice());
            var spriteTextVisual = Compositor.CreateSpriteVisual();

            ElementCompositionPreview.SetElementChildVisual(this, spriteTextVisual);
            SizeChanged += (s, e) =>
            {
                DrawingSurface = graphicsDevice.CreateDrawingSurface(e.NewSize, DirectXPixelFormat.B8G8R8A8UIntNormalized, DirectXAlphaMode.Premultiplied);
                DrawSurface();
                UpdateSpriteVisual(spriteTextVisual, DrawingSurface);
                spriteTextVisual.Size = e.NewSize.ToVector2();
            };
            RegisterPropertyChangedCallback(FontSizeProperty, new DependencyPropertyChangedCallback((s, e) =>
             {
                 DrawSurface();
             }));
        }

        /// <summary>
        /// 获取或设置StrokeStyle的值
        /// </summary>
        public CanvasDashStyle DashStyle
        {
            get => (CanvasDashStyle)GetValue(DashStyleProperty);
            set => SetValue(DashStyleProperty, value);
        }

        /// <summary>
        /// 获取或设置FontColor的值
        /// </summary>
        public Color FontColor
        {
            get => (Color)GetValue(FontColorProperty);
            set => SetValue(FontColorProperty, value);
        }

        /// <summary>
        /// 获取或设置OutlineColor的值
        /// </summary>
        public Color OutlineColor
        {
            get => (Color)GetValue(OutlineColorProperty);
            set => SetValue(OutlineColorProperty, value);
        }

        /// <summary>
        /// 获取或设置ShowNonOutlineText的值
        /// </summary>
        public bool ShowNonOutlineText
        {
            get => (bool)GetValue(ShowNonOutlineTextProperty);
            set => SetValue(ShowNonOutlineTextProperty, value);
        }

        /// <summary>
        /// 获取或设置StrokeWidth的值
        /// </summary>
        public double StrokeWidth
        {
            get => (double)GetValue(StrokeWidthProperty);
            set => SetValue(StrokeWidthProperty, value);
        }

        /// <summary>
        /// 获取或设置Text的值
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        protected CompositionDrawingSurface DrawingSurface { get; private set; }
        private Compositor Compositor => Window.Current.Compositor;

        protected void DrawSurface()
        {
            if (ActualHeight == 0 || ActualWidth == 0 || string.IsNullOrWhiteSpace(Text) || DrawingSurface == null)
                return;

            var width = (float)ActualWidth;
            var height = (float)ActualHeight;

            DrawSurfaceCore(DrawingSurface, width, height);
        }

        protected virtual void DrawSurfaceCore(CompositionDrawingSurface drawingSurface, float width, float height)
        {
            using (var session = CanvasComposition.CreateDrawingSession(drawingSurface))
            {
                session.Clear(Colors.Transparent);
                using (var textFormat = new CanvasTextFormat()
                {
                    FontSize = (float)FontSize,
                    Direction = CanvasTextDirection.LeftToRightThenTopToBottom,
                    VerticalAlignment = CanvasVerticalAlignment.Center,
                    HorizontalAlignment = CanvasHorizontalAlignment.Center,
                    FontWeight = FontWeight,
                    FontFamily = FontFamily.Source
                })
                {
                    using (var textLayout = new CanvasTextLayout(session, Text, textFormat, width, height))
                    {
                        DrawText(session, textLayout);
                    }
                }
            }
        }

        protected void DrawText(CanvasDrawingSession session, CanvasTextLayout textLayout)
        {
            if (ShowNonOutlineText)
                session.DrawTextLayout(textLayout, 0, 0, FontColor);

            using (var textGeometry = CanvasGeometry.CreateText(textLayout))
            {
                using (var dashedStroke = new CanvasStrokeStyle())
                {
                    dashedStroke.DashStyle = DashStyle;
                    session.DrawGeometry(textGeometry, OutlineColor, (float)StrokeWidth, dashedStroke);
                }
            }
        }

        /// <summary>
        /// StrokeStyle 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">StrokeStyle 属性的旧值。</param>
        /// <param name="newValue">StrokeStyle 属性的新值。</param>
        protected virtual void OnDashStyleChanged(CanvasDashStyle oldValue, CanvasDashStyle newValue)
        {
            DrawSurface();
        }

        /// <summary>
        /// FontColor 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">FontColor 属性的旧值。</param>
        /// <param name="newValue">FontColor 属性的新值。</param>
        protected virtual void OnFontColorChanged(Color oldValue, Color newValue)
        {
            DrawSurface();
        }

        /// <summary>
        /// OutlineColor 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">OutlineColor 属性的旧值。</param>
        /// <param name="newValue">OutlineColor 属性的新值。</param>
        protected virtual void OnOutlineColorChanged(Color oldValue, Color newValue)
        {
            DrawSurface();
        }

        /// <summary>
        /// ShowNonOutlineText 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">ShowNonOutlineText 属性的旧值。</param>
        /// <param name="newValue">ShowNonOutlineText 属性的新值。</param>
        protected virtual void OnShowNonOutlineTextChanged(bool oldValue, bool newValue)
        {
            DrawSurface();
        }

        /// <summary>
        /// StrokeWidth 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">StrokeWidth 属性的旧值。</param>
        /// <param name="newValue">StrokeWidth 属性的新值。</param>
        protected virtual void OnStrokeWidthChanged(double oldValue, double newValue)
        {
            DrawSurface();
        }

        /// <summary>
        /// Text 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">Text 属性的旧值。</param>
        /// <param name="newValue">Text 属性的新值。</param>
        protected virtual void OnTextChanged(string oldValue, string newValue)
        {
            DrawSurface();
        }

        protected virtual void UpdateSpriteVisual(SpriteVisual spriteVisual, CompositionDrawingSurface drawingSurface)
        {
            var maskSurfaceBrush = Compositor.CreateSurfaceBrush(DrawingSurface);
            spriteVisual.Brush = maskSurfaceBrush;
        }

        private static void OnDashStyleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (CanvasDashStyle)args.OldValue;
            var newValue = (CanvasDashStyle)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as OutlineTextControl;
            target?.OnDashStyleChanged(oldValue, newValue);
        }

        private static void OnFontColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (Color)args.OldValue;
            var newValue = (Color)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as OutlineTextControl;
            target?.OnFontColorChanged(oldValue, newValue);
        }

        private static void OnOutlineColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (Color)args.OldValue;
            var newValue = (Color)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as OutlineTextControl;
            target?.OnOutlineColorChanged(oldValue, newValue);
        }

        private static void OnShowNonOutlineTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (bool)args.OldValue;
            var newValue = (bool)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as OutlineTextControl;
            target?.OnShowNonOutlineTextChanged(oldValue, newValue);
        }

        private static void OnStrokeWidthChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (double)args.OldValue;
            var newValue = (double)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as OutlineTextControl;
            target?.OnStrokeWidthChanged(oldValue, newValue);
        }

        private static void OnTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (string)args.OldValue;
            var newValue = (string)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as OutlineTextControl;
            target?.OnTextChanged(oldValue, newValue);
        }
    }
}
