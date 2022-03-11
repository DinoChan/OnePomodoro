using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Composition;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace OnePomodoro.Controls
{
    public class ShadowTextControl : OutlineTextControl
    {
        private Compositor Compositor => Window.Current.Compositor;

        private CompositionMaskBrush _maskBrush;

        public ShadowTextControl()
        {
            _maskBrush = Compositor.CreateMaskBrush();
            var surfaceTextBrush = CreateBrush();
            _maskBrush.Source = surfaceTextBrush;
        }

        protected override void UpdateSpriteVisual(SpriteVisual spriteVisual, CompositionDrawingSurface drawingSurface)
        {
            var maskSurfaceBrush = Compositor.CreateSurfaceBrush(DrawingSurface);
            _maskBrush.Mask = maskSurfaceBrush;
            spriteVisual.Brush = _maskBrush;
        }

        protected virtual CompositionBrush CreateBrush()
        {
            return Compositor.CreateColorBrush(Colors.White);
        }

        protected override void DrawSurfaceCore(CompositionDrawingSurface drawingSurface, float width, float height)
        {
            using (var session = CanvasComposition.CreateDrawingSession(drawingSurface))
            {
                session.Clear(Colors.Transparent);

                StrokeWidth = 0;

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
                        var fullSizeGeometry = CanvasGeometry.CreateRectangle(session, 0, 0, width, height);
                        var textGeometry = CanvasGeometry.CreateText(textLayout);
                        var finalGeometry = fullSizeGeometry.CombineWith(textGeometry, Matrix3x2.Identity, CanvasGeometryCombine.Exclude);
                        using (var layer = session.CreateLayer(1, finalGeometry))
                        {
                            using (var bitmap = new CanvasRenderTarget(session, width, height))
                            {
                                using (var bitmapSession = bitmap.CreateDrawingSession())
                                {
                                    DrawText(bitmapSession, textLayout);
                                }
                                using (var blur = new GaussianBlurEffect
                                {
                                    BlurAmount = (float)BlurAmount,
                                    Source = bitmap,
                                    BorderMode = EffectBorderMode.Hard
                                })
                                {
                                    session.DrawImage(blur, 0, 0);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取或设置BlurAmount的值
        /// </summary>
        public double BlurAmount
        {
            get => (double)GetValue(BlurAmountProperty);
            set => SetValue(BlurAmountProperty, value);
        }

        /// <summary>
        /// 标识 BlurAmount 依赖属性。
        /// </summary>
        public static readonly DependencyProperty BlurAmountProperty =
            DependencyProperty.Register(nameof(BlurAmount), typeof(double), typeof(ShadowTextControl), new PropertyMetadata(10d, OnBlurAmountChanged));

        private static void OnBlurAmountChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (double)args.OldValue;
            var newValue = (double)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as ShadowTextControl;
            target?.OnBlurAmountChanged(oldValue, newValue);
        }

        /// <summary>
        /// BlurAmount 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">BlurAmount 属性的旧值。</param>
        /// <param name="newValue">BlurAmount 属性的新值。</param>
        protected virtual void OnBlurAmountChanged(double oldValue, double newValue)
        {
            DrawSurface();
        }
    }
}
