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
    [TemplateVisualState(GroupName = PromodoroStatesName, Name = InworkStateName)]
    [TemplateVisualState(GroupName = PromodoroStatesName, Name = BreakStateName)]

    public class ButtonDecorator : ElementDecorator
    {
        private const string CommonStatesName = "CommonStates";
        private const string NormalStateName = "Normal";
        private const string PointerOverStateName = "PointerOver";
        private const string PressedStateName = "Pressed";
        private const string PromodoroStatesName = "PromodoroStates";
        private const string InworkStateName = "Inwork";
        private const string BreakStateName = "Break";

        /// <summary>
        /// 标识 IsInPomodoro 依赖属性。
        /// </summary>
        public static readonly DependencyProperty IsInPomodoroProperty =
            DependencyProperty.Register(nameof(IsInPomodoro), typeof(bool), typeof(ButtonDecorator), new PropertyMetadata(default(bool), OnIsInPomodoroChanged));


        /// <summary>
        /// 标识 State 依赖属性。
        /// </summary>
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register(nameof(State), typeof(ButtonState), typeof(ButtonDecorator), new PropertyMetadata(ButtonState.Normal, OnStateChanged));

      
        public ButtonDecorator()
        {
            this.DefaultStyleKey = typeof(ButtonDecorator);
           }

       

        /// <summary>
        /// 获取或设置State的值
        /// </summary>
        public ButtonState State
        {
            get => (ButtonState)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }

        /// <summary>
        /// 获取或设置IsInPomodoro的值
        /// </summary>
        public bool IsInPomodoro
        {
            get => (bool)GetValue(IsInPomodoroProperty);
            set => SetValue(IsInPomodoroProperty, value);
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


        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
           
            UpdateVisualStates(false);
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
            VisualStateManager.GoToState(this, IsInPomodoro ? InworkStateName : BreakStateName, useTransitions);    
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

        private static void OnIsInPomodoroChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (bool)args.OldValue;
            var newValue = (bool)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as ButtonDecorator;
            target?.OnIsInPomodoroChanged(oldValue, newValue);
        }


    }
}
