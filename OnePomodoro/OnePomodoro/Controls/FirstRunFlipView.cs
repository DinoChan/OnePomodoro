using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace OnePomodoro.Controls
{
    public class FirstRunFlipView : FlipView
    {
        public FirstRunFlipView()
        {
            DefaultStyleKey = typeof(FirstRunFlipView);
            SelectionChanged += OnSelectionChanged;
        }



        private Button _closeButton;


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
