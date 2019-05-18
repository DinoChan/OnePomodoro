using System;
using System.Threading;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OnePomodoro.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private DispatcherTimer _workingTimer;
        private DispatcherTimer _breakingTimer;
        private DispatcherTimer _longBreakingTimer;
        private TimeSpan _workingTime;
        private TimeSpan _breakingTime;

        public MainViewModel()
        {

        }



        public TimeSpan WorkingTime
        {
            get
            {
                return _workingTime;
            }

            set
            {
                _workingTime = value;
                RaisePropertyChanged();
            }
        }

        public TimeSpan BreakingTime
        {
            get
            {
                return _breakingTime;
            }


            set
            {
                _breakingTime = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand StartCommand 
    }
}
