using OnePomodoro.Helpers;

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
