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


        public static IEnumerable<Type> Views { get; } = new List<Type>
        {
            typeof(TheFirst),
            typeof(TheBigOne),
            typeof(SimpleCircle),
            typeof(Gradients),
            typeof(GradientsWithBlend),
            typeof(LongShadow),
            typeof(SpringTextView),
            typeof(KonosubaView),
            typeof(HiddenTextView),
            typeof(WhiteTextView),
            typeof(OutlineTextView),
        };

    }
}
