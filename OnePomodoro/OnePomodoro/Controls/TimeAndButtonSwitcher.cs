using Microsoft.Graphics.Canvas.Effects;
using OnePomodoro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace OnePomodoro.Controls
{
    public sealed class TimeAndButtonSwitcher : Button
    {
        public TimeAndButtonSwitcher()
        {
            this.DefaultStyleKey = typeof(TimeAndButtonSwitcher);
        }

        public PomodoroViewModel PomodoroViewModel => PomodoroViewModel.Current;


        // Using a DependencyProperty as the backing store for StateButtonLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StateButtonLengthProperty =
            DependencyProperty.Register(nameof(StateButtonLength), typeof(double), typeof(TimeAndButtonSwitcher), new PropertyMetadata(0d));



        /// <summary>
        /// 标识 PomodoroColor 依赖属性。
        /// </summary>
        public static readonly DependencyProperty PomodoroColorProperty =
            DependencyProperty.Register(nameof(PomodoroColor), typeof(Color), typeof(TimeAndButtonSwitcher), new PropertyMetadata(Colors.White));

        /// <summary>
        /// 标识 BreakColor 依赖属性。
        /// </summary>
        public static readonly DependencyProperty BreakColorProperty =
            DependencyProperty.Register(nameof(BreakColor), typeof(Color), typeof(TimeAndButtonSwitcher), new PropertyMetadata(Colors.White));

        /// <summary>
        /// 标识 OutlineColor 依赖属性。
        /// </summary>
        public static readonly DependencyProperty OutlineColorProperty =
            DependencyProperty.Register(nameof(OutlineColor), typeof(Color), typeof(TimeAndButtonSwitcher), new PropertyMetadata(Colors.White));

        public double StateButtonLength
        {
            get { return (double)GetValue(StateButtonLengthProperty); }
            set { SetValue(StateButtonLengthProperty, value); }
        }

        /// <summary>
        /// 获取或设置PomodoroColor的值
        /// </summary>
        public Color PomodoroColor
        {
            get => (Color)GetValue(PomodoroColorProperty);
            set => SetValue(PomodoroColorProperty, value);
        }

        /// <summary>
        /// 获取或设置BreakColor的值
        /// </summary>
        public Color BreakColor
        {
            get => (Color)GetValue(BreakColorProperty);
            set => SetValue(BreakColorProperty, value);
        }

        /// <summary>
        /// 获取或设置BreakColor的值
        /// </summary>
        public Color OutlineColor
        {
            get => (Color)GetValue(OutlineColorProperty);
            set => SetValue(OutlineColorProperty, value);
        }
    }
}
