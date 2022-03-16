using System;
using OnePomodoro.Helpers;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace OnePomodoro.PomodoroViews
{
    [Title("SimpleCircle")]
    [Screenshot("/Assets/Screenshots/SimpleCircle.png")]
    [CompactOverlay]
    [SourceCode("https://github.com/DinoChan/OnePomodoro/blob/master/OnePomodoro/OnePomodoro/PomodoroViews/SimpleCircle.xaml.cs")]
    public sealed partial class SimpleCircle : PomodoroView
    {
        public SimpleCircle() : base()
        {
            InitializeComponent();
            ViewModel.RemainingPomodoroTimeChanged += OnRemainingPomodoroTimeChanged;
            ViewModel.RemainingBreakTimeChanged += OnRemainingBreakTimeChanged;
        }

        private void OnRemainingPomodoroTimeChanged(object sender, EventArgs e)
        {
            InworkEllipseTransform.Angle = 360 * (ViewModel.TotalPomodoroTime - ViewModel.RemainingPomodoroTime).TotalMilliseconds / ViewModel.TotalPomodoroTime.TotalMilliseconds;
        }

        private void OnRemainingBreakTimeChanged(object sender, EventArgs e)
        {
            if (ViewModel.TotalBreakTime.TotalMilliseconds == 0)
                BreakEllipseTransform.Angle = 0;
            else
                BreakEllipseTransform.Angle = 360 * (ViewModel.TotalBreakTime - ViewModel.RemainingBreakTime).TotalMilliseconds / ViewModel.TotalBreakTime.TotalMilliseconds;
        }
    }
}
