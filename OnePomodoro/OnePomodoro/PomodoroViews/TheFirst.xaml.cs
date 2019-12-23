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
    [Title("TheFirst")]
    [Screenshot("/Assets/Screenshots/TheFirst.png")]
    [FunctionTags(Tags.ImplicitAnimation)]
    [SourceCode("https://github.com/DinoChan/OnePomodoro/blob/master/OnePomodoro/OnePomodoro/PomodoroViews/TheFirst.xaml.cs")]
    public sealed partial class TheFirst : PomodoroView
    {
        public TheFirst()
        {
            InitializeComponent();
        }



        //private void OnOptionsClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        //{
        //    RaiseNavigateToOptionsPage();
        //}

        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    base.OnNavigatedTo(e);
        //    Window.Current.SetTitleBar(null);
        //}
    }
}
