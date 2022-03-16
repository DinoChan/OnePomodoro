using OnePomodoro.ViewModels;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace OnePomodoro.Controls
{
    public sealed class TimeAndButtonSwitcher : Button
    {
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

        /// <summary>
        /// 标识 PomodoroColor 依赖属性。
        /// </summary>
        public static readonly DependencyProperty PomodoroColorProperty =
            DependencyProperty.Register(nameof(PomodoroColor), typeof(Color), typeof(TimeAndButtonSwitcher), new PropertyMetadata(Colors.White));

        // Using a DependencyProperty as the backing store for StateButtonLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StateButtonLengthProperty =
            DependencyProperty.Register(nameof(StateButtonLength), typeof(double), typeof(TimeAndButtonSwitcher), new PropertyMetadata(0d));

        public TimeAndButtonSwitcher()
        {
            this.DefaultStyleKey = typeof(TimeAndButtonSwitcher);
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

        /// <summary>
        /// 获取或设置PomodoroColor的值
        /// </summary>
        public Color PomodoroColor
        {
            get => (Color)GetValue(PomodoroColorProperty);
            set => SetValue(PomodoroColorProperty, value);
        }

        public PomodoroViewModel PomodoroViewModel => PomodoroViewModel.Current;

        public double StateButtonLength
        {
            get { return (double)GetValue(StateButtonLengthProperty); }
            set { SetValue(StateButtonLengthProperty, value); }
        }
    }
}
