using Windows.UI.Xaml.Controls;

namespace OnePomodoro.Controls
{
    public class FirstRunFlipView : FlipView
    {
        private Button _closeButton;

        public FirstRunFlipView()
        {
            DefaultStyleKey = typeof(FirstRunFlipView);
            SelectionChanged += OnSelectionChanged;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _closeButton = GetTemplateChild("CloseButton") as Button;
            UpdateButton();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButton();
        }

        private void UpdateButton()
        {
            if (_closeButton == null)
                return;

            _closeButton.Visibility = SelectedIndex == (Items.Count - 1) ? Windows.UI.Xaml.Visibility.Visible : Windows.UI.Xaml.Visibility.Collapsed;
        }
    }
}
