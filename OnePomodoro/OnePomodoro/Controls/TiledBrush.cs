using Microsoft.Graphics.Canvas.Effects;
using System;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace OnePomodoro.Controls
{
    public class TiledBrush : XamlCompositionBrushBase
    {
        private CompositionEffectFactory _borderEffectFactory;
        private CompositionEffectBrush _borderEffectBrush;
        private BorderEffect _borderEffect;
        private CompositionSurfaceBrush _surfaceBrush;
        private LoadedImageSurface _surface;

        private Compositor Compositor => Window.Current.Compositor;


        /// <summary>
        /// 获取或设置ImageSourceUri的值
        /// </summary>
        public Uri ImageSourceUri
        {
            get => (Uri)GetValue(ImageSourceUriProperty);
            set => SetValue(ImageSourceUriProperty, value);
        }

        /// <summary>
        /// 标识 ImageSourceUri 依赖属性。
        /// </summary>
        public static readonly DependencyProperty ImageSourceUriProperty =
            DependencyProperty.Register(nameof(ImageSourceUri), typeof(Uri), typeof(TiledBrush), new PropertyMetadata(default(Uri), OnImageSourceUriChanged));

        private static void OnImageSourceUriChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (Uri)args.OldValue;
            var newValue = (Uri)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as TiledBrush;
            target?.OnImageSourceUriChanged(oldValue, newValue);
        }

        /// <summary>
        /// ImageSourceUri 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">ImageSourceUri 属性的旧值。</param>
        /// <param name="newValue">ImageSourceUri 属性的新值。</param>
        protected virtual void OnImageSourceUriChanged(Uri oldValue, Uri newValue)
        {
            UpdateSurface(newValue);
        }


        private void UpdateSurface(Uri uri)
        {
            if (uri != null && _surfaceBrush != null)
            {
                _surface = LoadedImageSurface.StartLoadFromUri(uri);
                _surfaceBrush.Surface = _surface;
            }
        }


        protected override void OnConnected()
        {
            base.OnConnected();

            if (CompositionBrush == null)
            {
                _surfaceBrush = Compositor.CreateSurfaceBrush();
                _surfaceBrush.Stretch = CompositionStretch.None;

                UpdateSurface(ImageSourceUri);

                _borderEffect = new BorderEffect()
                {
                    Source = new CompositionEffectSourceParameter("source"),
                    ExtendX = Microsoft.Graphics.Canvas.CanvasEdgeBehavior.Wrap,
                    ExtendY = Microsoft.Graphics.Canvas.CanvasEdgeBehavior.Wrap
                };

                _borderEffectFactory = Compositor.CreateEffectFactory(_borderEffect);
                _borderEffectBrush = _borderEffectFactory.CreateBrush();
                _borderEffectBrush.SetSourceParameter("source", _surfaceBrush);
                CompositionBrush = _borderEffectBrush;
            }
        }


        protected override void OnDisconnected()
        {
            base.OnDisconnected();

            if (CompositionBrush != null)
            {
                CompositionBrush.Dispose();
                CompositionBrush = null;
            }

            if (_borderEffectBrush != null)
            {
                _borderEffectBrush.Dispose();
                _borderEffectBrush = null;
            }

            if (_borderEffectFactory != null)
            {
                _borderEffectFactory.Dispose();
                _borderEffectFactory = null;
            }

            if (_borderEffect != null)
            {
                _borderEffect.Dispose();
                _borderEffect = null;
            }

            if (_surface != null)
            {
                _surface.Dispose();
                _surface = null;
            }

            if (_surfaceBrush != null)
            {
                _surfaceBrush.Dispose();
                _surfaceBrush = null;
            }
        }


    }

}
