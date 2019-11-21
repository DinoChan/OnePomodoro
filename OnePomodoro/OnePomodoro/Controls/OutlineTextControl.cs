using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Composition;
using Windows.Graphics.DirectX;
using Windows.Foundation;
using System.Numerics;
using Windows.UI.Xaml.Hosting;

namespace OnePomodoro.Controls
{
    public class OutlineTextControl : Control
    {
        private Compositor Compositor => Window.Current.Compositor;

        private CompositionDrawingSurface _drawingSurface;

        public OutlineTextControl()
        {
            var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            var graphicsDevice = CanvasComposition.CreateCompositionGraphicsDevice(compositor, CanvasDevice.GetSharedDevice());

            var spriteTextVisual = compositor.CreateSpriteVisual();
            spriteTextVisual.Size = new Vector2(512, 512);


            ElementCompositionPreview.SetElementChildVisual(this, spriteTextVisual);
            SizeChanged += (s, e) =>
            {
                _drawingSurface = graphicsDevice.CreateDrawingSurface(new Size(512, 512), DirectXPixelFormat.B8G8R8A8UIntNormalized, DirectXAlphaMode.Premultiplied);
                DrawText();
                var maskSurfaceBrush = compositor.CreateSurfaceBrush(_drawingSurface);
                spriteTextVisual.Brush = maskSurfaceBrush;
                DrawText();
            };
            RegisterPropertyChangedCallback(FontSizeProperty, new DependencyPropertyChangedCallback((s, e) =>
             {
                 DrawText();
             }));

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
        /// 标识 OutlineColor 依赖属性。
        /// </summary>
        public static readonly DependencyProperty OutlineColorProperty =
            DependencyProperty.Register(nameof(OutlineColor), typeof(Color), typeof(OutlineTextControl), new PropertyMetadata(Colors.Black, OnOutlineColorChanged));

        private static void OnOutlineColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (Color)args.OldValue;
            var newValue = (Color)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as OutlineTextControl;
            target?.OnOutlineColorChanged(oldValue, newValue);
        }

        /// <summary>
        /// OutlineColor 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">OutlineColor 属性的旧值。</param>
        /// <param name="newValue">OutlineColor 属性的新值。</param>
        protected virtual void OnOutlineColorChanged(Color oldValue, Color newValue)
        {
            DrawText();
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
        /// 标识 FontColor 依赖属性。
        /// </summary>
        public static readonly DependencyProperty FontColorProperty =
            DependencyProperty.Register(nameof(FontColor), typeof(Color), typeof(OutlineTextControl), new PropertyMetadata(Colors.Black, OnFontColorChanged));

        private static void OnFontColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (Color)args.OldValue;
            var newValue = (Color)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as OutlineTextControl;
            target?.OnFontColorChanged(oldValue, newValue);
        }

        /// <summary>
        /// FontColor 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">FontColor 属性的旧值。</param>
        /// <param name="newValue">FontColor 属性的新值。</param>
        protected virtual void OnFontColorChanged(Color oldValue, Color newValue)
        {
            DrawText();
        }


        /// <summary>
        /// 获取或设置Text的值
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        /// 标识 Text 依赖属性。
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(OutlineTextControl), new PropertyMetadata(default(string), OnTextChanged));

        private static void OnTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (string)args.OldValue;
            var newValue = (string)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as OutlineTextControl;
            target?.OnTextChanged(oldValue, newValue);
        }

        /// <summary>
        /// Text 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">Text 属性的旧值。</param>
        /// <param name="newValue">Text 属性的新值。</param>
        protected virtual void OnTextChanged(string oldValue, string newValue)
        {
            DrawText();
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
        /// 标识 StrokeStyle 依赖属性。
        /// </summary>
        public static readonly DependencyProperty DashStyleProperty =
            DependencyProperty.Register(nameof(DashStyle), typeof(CanvasDashStyle), typeof(OutlineTextControl), new PropertyMetadata(default(CanvasDashStyle), OnDashStyleChanged));

        private static void OnDashStyleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (CanvasDashStyle)args.OldValue;
            var newValue = (CanvasDashStyle)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as OutlineTextControl;
            target?.OnDashStyleChanged(oldValue, newValue);
        }

        /// <summary>
        /// StrokeStyle 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">StrokeStyle 属性的旧值。</param>
        /// <param name="newValue">StrokeStyle 属性的新值。</param>
        protected virtual void OnDashStyleChanged(CanvasDashStyle oldValue, CanvasDashStyle newValue)
        {
            DrawText();
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
        /// 标识 ShowNonOutlineText 依赖属性。
        /// </summary>
        public static readonly DependencyProperty ShowNonOutlineTextProperty =
            DependencyProperty.Register(nameof(ShowNonOutlineText), typeof(bool), typeof(OutlineTextControl), new PropertyMetadata(default(bool), OnShowNonOutlineTextChanged));

        private static void OnShowNonOutlineTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (bool)args.OldValue;
            var newValue = (bool)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as OutlineTextControl;
            target?.OnShowNonOutlineTextChanged(oldValue, newValue);
        }

        /// <summary>
        /// ShowNonOutlineText 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">ShowNonOutlineText 属性的旧值。</param>
        /// <param name="newValue">ShowNonOutlineText 属性的新值。</param>
        protected virtual void OnShowNonOutlineTextChanged(bool oldValue, bool newValue)
        {
            DrawText();
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
        /// 标识 StrokeWidth 依赖属性。
        /// </summary>
        public static readonly DependencyProperty StrokeWidthProperty =
            DependencyProperty.Register(nameof(StrokeWidth), typeof(double), typeof(OutlineTextControl), new PropertyMetadata(default(double), OnStrokeWidthChanged));

        private static void OnStrokeWidthChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (double)args.OldValue;
            var newValue = (double)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as OutlineTextControl;
            target?.OnStrokeWidthChanged(oldValue, newValue);
        }

        /// <summary>
        /// StrokeWidth 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">StrokeWidth 属性的旧值。</param>
        /// <param name="newValue">StrokeWidth 属性的新值。</param>
        protected virtual void OnStrokeWidthChanged(double oldValue, double newValue)
        {
            DrawText();
        }

        private void DrawText()
        {
            if (ActualHeight == 0 || ActualWidth == 0 || string.IsNullOrWhiteSpace(Text) || _drawingSurface == null)
                return;

            var width = (float)ActualWidth;
            var height = (float)ActualHeight;
            using (var session = CanvasComposition.CreateDrawingSession(_drawingSurface))
            {
                session.Clear(Colors.Transparent);
                using (var textFormat = new CanvasTextFormat()
                {
                    FontSize = (float)FontSize,
                    Direction = CanvasTextDirection.LeftToRightThenTopToBottom,
                    VerticalAlignment = CanvasVerticalAlignment.Center,
                    HorizontalAlignment = CanvasHorizontalAlignment.Center,

                })
                {
                    using (var textLayout = new CanvasTextLayout(session, Text, textFormat, width, height))
                    {
                        if (ShowNonOutlineText)
                        {
                            Color semitrans = FontColor;
                            //semitrans.A = 127;
                            session.DrawTextLayout(textLayout, 0, 0, semitrans);
                        }

                        using (var textGeometry = CanvasGeometry.CreateText(textLayout))
                        {
                            var dashedStroke = new CanvasStrokeStyle()
                            {
                                DashStyle = DashStyle
                            };
                            session.DrawGeometry(textGeometry, OutlineColor, (float)StrokeWidth, dashedStroke);
                        }
                    }
                }
            }
        }


    }
}
