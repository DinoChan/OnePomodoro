using System;
using System.Threading.Tasks;
using OnePomodoro.Helpers;
using Windows.UI.Xaml;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace OnePomodoro.PomodoroViews
{
    [Title("Audio Call")]
    [Screenshot("/Assets/Screenshots/AudioCall.png")]
    [CompactOverlay]
    [SourceCode("https://github.com/DinoChan/OnePomodoro/blob/master/OnePomodoro/OnePomodoro/PomodoroViews/AudioCall.xaml.cs")]
    public sealed partial class AudioCall : PomodoroView
    {
        private bool _stopUpdateProgress;
        private bool _hasLoaded;

        public AudioCall() : base()
        {
            InitializeComponent();

            Loaded += AudioCall_Loaded;
            ViewModel.IsInPomodoroChanged += OnIsInPomodoroChanged;
            ViewModel.RemainingPomodoroTimeChanged += OnRemainingPomodoroTimeChanged;
            ViewModel.RemainingBreakTimeChanged += OnRemainingBreakTimeChanged;
            UpdateProgress();
            UpdateVisualState();
        }

        private void AudioCall_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= AudioCall_Loaded;
            _hasLoaded = true;
            BreathingLightsStoryboard.Begin();
        }

        private async void OnIsInPomodoroChanged(object sender, EventArgs e)
        {
            ProgressBar.Value = 0;
            _stopUpdateProgress = true;
            try
            {
                UpdateVisualState();
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            finally
            {
                _stopUpdateProgress = false;
            }
            UpdateProgress();
        }

        private void UpdateVisualState()
        {
            VisualStateManager.GoToState(this, ViewModel.IsInPomodoro ? "InPomodoro" : "InBreak", _hasLoaded);
        }

        private void OnStoryboardCompleted(object sender, object e)
        {
            //await Task.Delay(TimeSpan.FromSeconds(5));
            BreathingLightsStoryboard.Begin();
        }

        private void OnRemainingPomodoroTimeChanged(object sender, EventArgs e)
        {
            UpdateProgress();
        }

        private void OnRemainingBreakTimeChanged(object sender, EventArgs e)
        {
            UpdateProgress();

            //if (ViewModel.TotalBreakTime.TotalMilliseconds == 0)
            //    ProgressBar.Value = 0;
            //else
        }

        private void UpdateProgress()
        {
            if (_stopUpdateProgress)
                return;

            if (ViewModel.IsInPomodoro)
                ProgressBar.Value = Math.Round(ViewModel.RemainingPomodoroTime.TotalSeconds) / ViewModel.TotalPomodoroTime.TotalSeconds;
            else
                ProgressBar.Value = Math.Round(ViewModel.RemainingBreakTime.TotalSeconds) / ViewModel.TotalBreakTime.TotalSeconds;
        }
    }
}
