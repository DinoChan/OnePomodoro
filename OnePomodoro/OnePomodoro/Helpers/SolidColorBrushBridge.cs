using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace OnePomodoro.Helpers
{
    public class SolidColorBrushBridge : FrameworkElement, INotifyPropertyChanged
    {
        public SolidColorBrushBridge()
        {
            Brush = new SolidColorBrush(Color);
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
            DependencyProperty.Register(nameof(Color), typeof(Color), typeof(SolidColorBrushBridge), new PropertyMetadata(default(Color), OnColorChanged));

        public event PropertyChangedEventHandler PropertyChanged;

        private static void OnColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (Color)args.OldValue;
            var newValue = (Color)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as SolidColorBrushBridge;
            target?.OnColorChanged(oldValue, newValue);
        }

        /// <summary>
        /// Color 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">Color 属性的旧值。</param>
        /// <param name="newValue">Color 属性的新值。</param>
        protected virtual void OnColorChanged(Color oldValue, Color newValue)
        {
            Brush.Color = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Brush)));
        }

        public SolidColorBrush Brush { get; }
    }
}
