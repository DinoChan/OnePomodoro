using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Shapes;

namespace OnePomodoro.Controls
{
    public class OutlinePanel : ButtonDecorator
    {
        private readonly Compositor _compositor;
        private CompositionMaskBrush _maskBrush;
        private CompositionColorBrush _maskBrushSource;

        public OutlinePanel() : base()
        {
            this.DefaultStyleKey = typeof(OutlinePanel);
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _maskBrush = _compositor.CreateMaskBrush();
            Visual.Brush = _maskBrush;
            _maskBrushSource = _compositor.CreateColorBrush(Color);
        }

        protected override void UpdateOutlineMask()
        {
            if (RelativeElement is Shape shape)
            {
                var mask = shape.GetAlphaMask();
                _maskBrush.Mask = mask;
                _maskBrush.Source = _maskBrushSource;
            }
            else if (RelativeElement is TextBlock textBlock)
            {
                var mask = textBlock.GetAlphaMask();
                _maskBrush.Mask = mask;
                _maskBrush.Source = _maskBrushSource;
            }
        }


        /// <summary>
        /// 获取或设置Color的值
        /// </summary>
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        /// <summary>
        /// 标识 Color 依赖属性。
        /// </summary>
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(nameof(Color), typeof(Color), typeof(OutlinePanel), new PropertyMetadata(Colors.White, OnColorChanged));

        private static void OnColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (Color)args.OldValue;
            var newValue = (Color)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as OutlinePanel;
            target?.OnColorChanged(oldValue, newValue);
        }

        /// <summary>
        /// Color 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">Color 属性的旧值。</param>
        /// <param name="newValue">Color 属性的新值。</param>
        protected virtual void OnColorChanged(Color oldValue, Color newValue)
        {
            _maskBrushSource = _compositor.CreateColorBrush(newValue);
        }
    }
}
