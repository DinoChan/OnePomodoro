using System;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace OnePomodoro.Controls
{
    public class AudioButton : Button
    {
        private MediaPlayer _mediaPlayer;
        private Storyboard _playStoryboard;

        public AudioButton()
        {
            this.DefaultStyleKey = typeof(AudioButton);
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.MediaEnded += OnMediaEnded;
            Click += OnClick;
        }

        /// <summary>
        /// 获取或设置AudioUri的值
        /// </summary>
        public string AudioUri
        {
            get => (string)GetValue(AudioUriProperty);
            set => SetValue(AudioUriProperty, value);
        }

        /// <summary>
        /// 标识 AudioUri 依赖属性。
        /// </summary>
        public static readonly DependencyProperty AudioUriProperty =
            DependencyProperty.Register(nameof(AudioUri), typeof(string), typeof(AudioButton), new PropertyMetadata(default(string), OnAudioUriChanged));

        private static void OnAudioUriChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (string)args.OldValue;
            var newValue = (string)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as AudioButton;
            target?.OnAudioUriChanged(oldValue, newValue);
        }

        /// <summary>
        /// AudioUri 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">AudioUri 属性的旧值。</param>
        /// <param name="newValue">AudioUri 属性的新值。</param>
        protected virtual void OnAudioUriChanged(string oldValue, string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue) == false)
                _mediaPlayer.Source = MediaSource.CreateFromUri(new Uri(newValue));

            this.Visibility = string.IsNullOrEmpty(newValue) ? Visibility.Collapsed : Visibility.Visible;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var root = this.GetTemplateChild("Root") as FrameworkElement;
            _playStoryboard = root.Resources["PlayStoryboard"] as Storyboard;
            OnAudioUriChanged(null, AudioUri);
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            _mediaPlayer.Play();
            if (_playStoryboard != null)
            {
                _playStoryboard.Stop();
                _playStoryboard.Begin();
            }
        }

        private void OnMediaEnded(MediaPlayer sender, object args)
        {
            if (_playStoryboard != null)
            {
                _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
                {
                    _playStoryboard.Stop();
                });
            }
        }
    }
}
