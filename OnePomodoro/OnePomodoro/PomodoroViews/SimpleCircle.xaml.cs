using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace OnePomodoro.PomodoroViews
{
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
