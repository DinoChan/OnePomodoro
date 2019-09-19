using System;
using System.Collections.Generic;
using Windows.UI;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace OnePomodoro.Controls
{
    [TemplateVisualState(GroupName = PromodoroStatesName, Name = InworkStateName)]
    [TemplateVisualState(GroupName = PromodoroStatesName, Name = BreakStateName)]
    public class GlowPanel : Control
    {
        /// <summary>
        /// 标识 IsInPomodoro 依赖属性。
        /// </summary>
        public static readonly DependencyProperty IsInPomodoroProperty =
            DependencyProperty.Register(nameof(IsInPomodoro), typeof(bool), typeof(GlowPanel), new PropertyMetadata(default(bool), OnIsInPomodoroChanged));

        /// <summary>
        /// 标识 RelativeElement 依赖属性。
        /// </summary>
        public static readonly DependencyProperty RelativeElementProperty =
            DependencyProperty.Register(nameof(RelativeElement), typeof(FrameworkElement), typeof(GlowPanel), new PropertyMetadata(default(FrameworkElement)));


        private const string PromodoroStatesName = "PromodoroStates";
        private const string InworkStateName = "Inwork";
        private const string BreakStateName = "Break";
        public GlowPanel()
        {
            DefaultStyleKey = typeof(GlowPanel);
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
        /// 获取或设置IsInPomodoro的值
        /// </summary>
        public bool IsInPomodoro
        {
            get => (bool)GetValue(IsInPomodoroProperty);
            set => SetValue(IsInPomodoroProperty, value);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateVisualStates(false);
        }

        /// <summary>
        /// IsInPomodoro 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">IsInPomodoro 属性的旧值。</param>
        /// <param name="newValue">IsInPomodoro 属性的新值。</param>
        protected virtual void OnIsInPomodoroChanged(bool oldValue, bool newValue)
        {
            UpdateVisualStates(true);
        }

        protected virtual void UpdateVisualStates(bool useTransitions)
        {
            VisualStateManager.GoToState(this, IsInPomodoro ? InworkStateName : BreakStateName, useTransitions);
        }

        private static void OnIsInPomodoroChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (bool)args.OldValue;
            var newValue = (bool)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as GlowPanel;
            target?.OnIsInPomodoroChanged(oldValue, newValue);
        }
    }
}
