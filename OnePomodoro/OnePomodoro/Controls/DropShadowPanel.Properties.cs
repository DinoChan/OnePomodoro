using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;

namespace OnePomodoro.Controls
{
    public partial class DropShadowPanel
    {
        /// <summary>
        /// Identifies the <see cref="BlurRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BlurRadiusProperty =
             DependencyProperty.RegisterAttached("BlurRadius", typeof(double), typeof(DropShadowPanel), new PropertyMetadata(9.0, OnBlurRadiusChanged));

        /// <summary>
        /// Identifies the <see cref="Color"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(DropShadowPanel), new PropertyMetadata(Colors.Black, OnColorChanged));

        /// <summary>
        /// Identifies the <see cref="OffsetX"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OffsetXProperty =
            DependencyProperty.RegisterAttached("OffsetX", typeof(double), typeof(DropShadowPanel), new PropertyMetadata(0.0, OnOffsetXChanged));

        /// <summary>
        /// Identifies the <see cref="OffsetY"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OffsetYProperty =
            DependencyProperty.RegisterAttached("OffsetY", typeof(double), typeof(DropShadowPanel), new PropertyMetadata(0.0, OnOffsetYChanged));

        /// <summary>
        /// Identifies the <see cref="OffsetZ"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OffsetZProperty =
            DependencyProperty.RegisterAttached("OffsetZ", typeof(double), typeof(DropShadowPanel), new PropertyMetadata(0.0, OnOffsetZChanged));

        /// <summary>
        /// Identifies the <see cref="ShadowOpacity"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShadowOpacityProperty =
            DependencyProperty.RegisterAttached("ShadowOpacity", typeof(double), typeof(DropShadowPanel), new PropertyMetadata(1.0, OnShadowOpacityChanged));

        /// <summary>
        /// Identifies the <see cref="IsMasked"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsMaskedProperty =
            DependencyProperty.RegisterAttached("IsMasked", typeof(bool), typeof(DropShadowPanel), new PropertyMetadata(true, OnIsMaskedChanged));

        /// <summary>
        /// Gets DropShadow. Exposes the underlying composition object to allow custom Windows.UI.Composition animations.
        /// </summary>
        public DropShadow DropShadow => _dropShadow;

        /// <summary>
        /// Gets or sets the mask of the underlying <see cref="Windows.UI.Composition.DropShadow"/>.
        /// Allows for a custom <see cref="Windows.UI.Composition.CompositionBrush"/> to be set.
        /// </summary>
        public CompositionBrush Mask
        {
            get
            {
                return _dropShadow?.Mask;
            }

            set
            {
                if (_dropShadow != null)
                {
                    _dropShadow.Mask = value;
                }
            }
        }

        public static double GetBlurRadius(DependencyObject obj) => (double)obj.GetValue(BlurRadiusProperty);

        public static void SetBlurRadius(DependencyObject obj, double value) => obj.SetValue(BlurRadiusProperty, value);

        public static Color GetColor(DependencyObject obj) => (Color)obj.GetValue(ColorProperty);

        public static void SetColor(DependencyObject obj, Color value) => obj.SetValue(ColorProperty, value);

        public static double GetOffsetX(DependencyObject obj) => (double)obj.GetValue(OffsetXProperty);

        public static void SetOffsetX(DependencyObject obj, double value) => obj.SetValue(OffsetXProperty, value);

        public static double GetOffsetY(DependencyObject obj) => (double)obj.GetValue(OffsetYProperty);

        public static void SetOffsetY(DependencyObject obj, double value) => obj.SetValue(OffsetYProperty, value);

        public static double GetOffsetZ(DependencyObject obj) => (double)obj.GetValue(OffsetZProperty);

        public static void SetOffsetZ(DependencyObject obj, double value) => obj.SetValue(OffsetZProperty, value);

        public static double GetShadowOpacity(DependencyObject obj) => (double)obj.GetValue(ShadowOpacityProperty);

        public static void SetShadowOpacity(DependencyObject obj, double value) => obj.SetValue(ShadowOpacityProperty, value);

        public static bool GetIsMasked(DependencyObject obj) => (bool)obj.GetValue(IsMaskedProperty);

        public static void SetIsMasked(DependencyObject obj, bool value) => obj.SetValue(IsMaskedProperty, value);

        private static void OnBlurRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DropShadowPanel panel)
            {
                panel.OnBlurRadiusChanged((double)e.NewValue);
            }
        }

        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DropShadowPanel panel)
            {
                panel.OnColorChanged((Color)e.NewValue);
            }
        }

        private static void OnOffsetXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DropShadowPanel panel)
            {
                panel.OnOffsetXChanged((double)e.NewValue);
            }
        }

        private static void OnOffsetYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DropShadowPanel panel)
            {
                panel.OnOffsetYChanged((double)e.NewValue);
            }
        }

        private static void OnOffsetZChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DropShadowPanel panel)
            {
                panel.OnOffsetZChanged((double)e.NewValue);
            }
        }

        private static void OnShadowOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DropShadowPanel panel)
            {
                panel.OnShadowOpacityChanged((double)e.NewValue);
            }
        }

        private static void OnIsMaskedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DropShadowPanel panel)
            {
                panel.UpdateOutlineMask();
            }
        }
    }
}
