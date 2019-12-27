using OnePomodoro.Helpers;
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
    [Title("Audio Call")]
    [Screenshot("/Assets/Screenshots/SimpleCircle.png")]
    [CompactOverlay]
    [SourceCode("https://github.com/DinoChan/OnePomodoro/blob/master/OnePomodoro/OnePomodoro/PomodoroViews/AudioCall.xaml.cs")]
    public sealed partial class AudioCall : PomodoroView
    {
        public AudioCall() : base()
        {
            InitializeComponent();
           
            Loaded += AudioCall_Loaded;
        }

        private void AudioCall_Loaded(object sender, RoutedEventArgs e)
        {
            Storyboard1.Begin();
        }

        //private void OnRemainingPomodoroTimeChanged(object sender, EventArgs e)
        //{
        //    InworkEllipseTransform.Angle = 360 * (ViewModel.TotalPomodoroTime - ViewModel.RemainingPomodoroTime).TotalMilliseconds / ViewModel.TotalPomodoroTime.TotalMilliseconds;
        //}

        //private void OnRemainingBreakTimeChanged(object sender, EventArgs e)
        //{
        //    if (ViewModel.TotalBreakTime.TotalMilliseconds == 0)
        //        BreakEllipseTransform.Angle = 0;
        //    else
        //        BreakEllipseTransform.Angle = 360 * (ViewModel.TotalBreakTime - ViewModel.RemainingBreakTime).TotalMilliseconds / ViewModel.TotalBreakTime.TotalMilliseconds;
        //}
    }
}
