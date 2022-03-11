using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace OnePomodoro.Controls
{
    [TemplatePart(Name = OutlineBorderName, Type = typeof(Border))]
    public class ElementDecorator : Control
    {
        /// <summary>
        /// 标识 RelativeElement 依赖属性。
        /// </summary>
        public static readonly DependencyProperty RelativeElementProperty =
            DependencyProperty.Register(nameof(RelativeElement), typeof(FrameworkElement), typeof(ElementDecorator), new PropertyMetadata(default(FrameworkElement), OnRelativeElementChanged));

        private const string OutlineBorderName = "OutlineBorder";

        private Border _outlineBorder;
        private readonly Compositor _compositor;
        protected SpriteVisual Visual { get; private set; }

        public ElementDecorator()
        {
            this.DefaultStyleKey = typeof(ButtonDecorator);
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            Visual = _compositor.CreateSpriteVisual();
        }

        /// <summary>
        /// 获取或设置RelativeElement的值
        /// </summary>
        public FrameworkElement RelativeElement
        {
            get => (FrameworkElement)GetValue(RelativeElementProperty);
            set => SetValue(RelativeElementProperty, value);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _outlineBorder = GetTemplateChild(OutlineBorderName) as Border;
            if (_outlineBorder == null)
                return;

            ElementCompositionPreview.SetElementChildVisual(_outlineBorder, Visual);

            ConfigureShadowVisualForCastingElement();
        }

        /// <summary>
        /// RelativeElement 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">RelativeElement 属性的旧值。</param>
        /// <param name="newValue">RelativeElement 属性的新值。</param>
        protected virtual void OnRelativeElementChanged(FrameworkElement oldValue, FrameworkElement newValue)
        {
            if (oldValue != null)
            {
                oldValue.SizeChanged -= OnSizeChanged;
            }

            if (newValue != null)
            {
                newValue.SizeChanged += OnSizeChanged;
            }

            ConfigureShadowVisualForCastingElement();
        }

        private static void OnRelativeElementChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (FrameworkElement)args.OldValue;
            var newValue = (FrameworkElement)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as ElementDecorator;
            target?.OnRelativeElementChanged(oldValue, newValue);
        }

        private void ConfigureShadowVisualForCastingElement()
        {
            UpdateOutlineMask();

            UpdateOutlineSize();
        }

        protected virtual void UpdateOutlineMask()
        {
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateOutlineSize();
        }

        private void UpdateOutlineSize()
        {
            if (Visual != null)
            {
                Vector2 newSize = new Vector2(0, 0);
                Vector3 centerPoint = new Vector3(0, 0, 0);
                if (RelativeElement != null)
                {
                    newSize = new Vector2((float)RelativeElement.ActualWidth, (float)RelativeElement.ActualHeight);
                    centerPoint = new Vector3((float)RelativeElement.ActualWidth / 2, (float)RelativeElement.ActualHeight / 2, 0);
                }
                Visual.CenterPoint = centerPoint;
                Visual.Size = newSize;
            }
        }
    }
}
