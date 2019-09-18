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
    [TemplateVisualState(GroupName = CommonStatesName, Name = NormalStateName)]
    [TemplateVisualState(GroupName = CommonStatesName, Name = PointerOverStateName)]
    [TemplateVisualState(GroupName = CommonStatesName, Name = PressedStateName)]
    [TemplatePart(Name = OutlineBorderName, Type = typeof(Border))]

    public class ButtonDecorator : Control
    {
        private const string CommonStatesName = "PromodoroStates";
        private const string NormalStateName = "Normal";
        private const string PointerOverStateName = "PointerOver";
        private const string PressedStateName = "Pressed";

        private const string OutlineBorderName = "OutlineBorder";

        /// <summary>
        /// 标识 RelativeElement 依赖属性。
        /// </summary>
        public static readonly DependencyProperty RelativeElementProperty =
            DependencyProperty.Register(nameof(RelativeElement), typeof(FrameworkElement), typeof(ButtonDecorator), new PropertyMetadata(default(FrameworkElement), OnRelativeElementChanged));

        /// <summary>
        /// 标识 State 依赖属性。
        /// </summary>
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register(nameof(State), typeof(ButtonState), typeof(ButtonDecorator), new PropertyMetadata(ButtonState.Normal, OnStateChanged));



        private Border _outlineBorder;
        private readonly Compositor _compositor;
        protected SpriteVisual Visual { get; private set; }
        private CompositionMaskBrush _maskBrush;

        public ButtonDecorator()
        {
            this.DefaultStyleKey = typeof(ButtonDecorator);
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            Visual = _compositor.CreateSpriteVisual();
            _maskBrush = _compositor.CreateMaskBrush();
            Visual.Brush = _maskBrush;
        }

        /// <summary>
        /// 获取或设置RelativeElement的值
        /// </summary>
        public FrameworkElement RelativeElement
        {
            get => (FrameworkElement)GetValue(RelativeElementProperty);
            set => SetValue(RelativeElementProperty, value);
        }

        /// <summary>
        /// 获取或设置State的值
        /// </summary>
        public ButtonState State
        {
            get => (ButtonState)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _outlineBorder = GetTemplateChild(OutlineBorderName) as Border;
            if (_outlineBorder == null)
                return;

            ElementCompositionPreview.SetElementChildVisual(_outlineBorder, Visual);

            ConfigureShadowVisualForCastingElement();
            UpdateVisualStates(false);
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

        /// <summary>
        /// State 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">State 属性的旧值。</param>
        /// <param name="newValue">State 属性的新值。</param>
        protected virtual void OnStateChanged(ButtonState oldValue, ButtonState newValue)
        {
            UpdateVisualStates(true);
        }

        protected virtual void UpdateVisualStates(bool useTransitions)
        {
            var state = NormalStateName;
            switch (State)
            {
                case ButtonState.Normal:
                    state = NormalStateName;
                    break;
                case ButtonState.PointerOver:
                    state = PointerOverStateName;
                    break;
                case ButtonState.Pressed:
                    state = PressedStateName;
                    break;
                default:
                    break;
            }
            VisualStateManager.GoToState(this, state, useTransitions);
        }

        private static void OnRelativeElementChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (FrameworkElement)args.OldValue;
            var newValue = (FrameworkElement)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as ButtonDecorator;
            target?.OnRelativeElementChanged(oldValue, newValue);
        }

        private static void OnStateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (ButtonState)args.OldValue;
            var newValue = (ButtonState)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as ButtonDecorator;
            target?.OnStateChanged(oldValue, newValue);
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
