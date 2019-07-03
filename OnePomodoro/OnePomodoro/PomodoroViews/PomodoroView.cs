using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using OnePomodoro.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace OnePomodoro.PomodoroViews
{
    public class PomodoroView : UserControl
    {
        public PomodoroView()
        {
            DataContext = PomodoroViewModel.Current;
        }

        public PomodoroViewModel ViewModel => DataContext as PomodoroViewModel;

        public event EventHandler NavigateToOptionsPage;

        public static IEnumerable<Type> Views { get; } = new List<Type>
        {
            typeof(TheFirst),
            typeof(TestPomodoro)
        };

        protected void RaiseNavigateToOptionsPage()
        {
            NavigateToOptionsPage?.Invoke(this, EventArgs.Empty);
        }
    }
}
