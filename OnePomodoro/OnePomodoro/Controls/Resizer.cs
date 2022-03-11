﻿using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace OnePomodoro.Controls
{
    [TemplatePart(Name = InnerContentControlName, Type = typeof(InnerContentControl))]
    public class Resizer : ContentControl
    {
        /// <summary>
        /// 标识 Animation 依赖属性。
        /// </summary>
        public static readonly DependencyProperty AnimationProperty =
            DependencyProperty.Register(nameof(Animation), typeof(DoubleAnimation), typeof(Resizer), new PropertyMetadata(default(DoubleAnimation), OnAnimationChanged));

        /// <summary>
        /// 标识 ContentHeight 依赖属性。
        /// </summary>
        public static readonly DependencyProperty ContentHeightProperty =
            DependencyProperty.Register(nameof(ContentHeight), typeof(double), typeof(Resizer), new PropertyMetadata(default(double), OnContentHeightChanged));

        /// <summary>
        /// 标识 ContentWidth 依赖属性。
        /// </summary>
        public static readonly DependencyProperty ContentWidthProperty =
            DependencyProperty.Register(nameof(ContentWidth), typeof(double), typeof(Resizer), new PropertyMetadata(default(double), OnContentWidthChanged));

        private const string InnerContentControlName = "InnerContentControl";
        private readonly DoubleAnimation _defaultHeightAnimation;
        private readonly DoubleAnimation _defaultWidthAnimation;
        private readonly Storyboard _resizingStoryboard;
        private InnerContentControl _innerContentControl;
        private bool _isInnerContentMeasuring;
        private bool _isResizing;

        public Resizer()
        {
            DefaultStyleKey = typeof(Resizer);

            // SizeChanged += OnControlSizeChanged;
            _resizingStoryboard = new Storyboard
            {
                FillBehavior = FillBehavior.HoldEnd
            };
            _resizingStoryboard.Completed += OnResizingCompleted;
            _defaultHeightAnimation = new DoubleAnimation() { Duration = new Duration(TimeSpan.Zero) };
            Storyboard.SetTarget(_defaultHeightAnimation, this);
            Storyboard.SetTargetProperty(_defaultHeightAnimation, nameof(ContentHeight));

            _defaultWidthAnimation = new DoubleAnimation() { Duration = new Duration(TimeSpan.Zero) };
            Storyboard.SetTarget(_defaultWidthAnimation, this);
            Storyboard.SetTargetProperty(_defaultWidthAnimation, nameof(ContentWidth));
        }

        /// <summary>
        /// 获取或设置Animation的值
        /// </summary>
        public DoubleAnimation Animation
        {
            get => (DoubleAnimation)GetValue(AnimationProperty);
            set => SetValue(AnimationProperty, value);
        }

        /// <summary>
        /// 获取或设置ContentHeight的值
        /// </summary>
        public double ContentHeight
        {
            get => (double)GetValue(ContentHeightProperty);
            set => SetValue(ContentHeightProperty, value);
        }

        /// <summary>
        /// 获取或设置ContentWidth的值
        /// </summary>
        public double ContentWidth
        {
            get => (double)GetValue(ContentWidthProperty);
            set => SetValue(ContentWidthProperty, value);
        }

        protected InnerContentControl InnerContentControl
        {
            get
            {
                return _innerContentControl;
            }

            set
            {
                if (_innerContentControl != null)
                    _innerContentControl.Measuring -= OnInnerContentMeasuring;

                _innerContentControl = value;

                if (_innerContentControl != null)
                    _innerContentControl.Measuring += OnInnerContentMeasuring;
            }
        }

        private Storyboard ResizingStoryboard
        {
            get
            {
                DoubleAnimation heightAnimation;
                DoubleAnimation widthAnimation;
                if (Animation != null)
                {
                    heightAnimation = new DoubleAnimation
                    {
                        BeginTime = Animation.BeginTime,
                        Duration = Animation.Duration,
                        EasingFunction = Animation.EasingFunction
                    };

                    Storyboard.SetTarget(heightAnimation, this);
                    Storyboard.SetTargetProperty(heightAnimation, nameof(ContentHeight));

                    widthAnimation = new DoubleAnimation
                    {
                        BeginTime = Animation.BeginTime,
                        Duration = Animation.Duration,
                        EasingFunction = Animation.EasingFunction
                    };
                    Storyboard.SetTarget(widthAnimation, this);
                    Storyboard.SetTargetProperty(widthAnimation, nameof(ContentWidth));
                }
                else
                {
                    heightAnimation = _defaultHeightAnimation;
                    widthAnimation = _defaultWidthAnimation;
                }

                heightAnimation.From = ActualHeight;
                heightAnimation.To = InnerContentControl.DesiredSize.Height;
                widthAnimation.From = ActualWidth;
                widthAnimation.To = InnerContentControl.DesiredSize.Width;

                _resizingStoryboard.Children.Clear();
                _resizingStoryboard.Children.Add(heightAnimation);
                _resizingStoryboard.Children.Add(widthAnimation);
                return _resizingStoryboard;
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (_isResizing)
                return new Size(ContentWidth, ContentHeight);

            if (_isInnerContentMeasuring)
            {
                _isInnerContentMeasuring = false;
                ChangeSize(true);
            }

            return base.MeasureOverride(constraint);
        }

        /// <summary>
        /// Animation 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">Animation 属性的旧值。</param>
        /// <param name="newValue">Animation 属性的新值。</param>
        protected virtual void OnAnimationChanged(DoubleAnimation oldValue, DoubleAnimation newValue)
        {
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InnerContentControl = GetTemplateChild(InnerContentControlName) as InnerContentControl;
        }

        /// <summary>
        /// ContentHeight 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">ContentHeight 属性的旧值。</param>
        /// <param name="newValue">ContentHeight 属性的新值。</param>
        protected virtual void OnContentHeightChanged(double oldValue, double newValue)
        {
            InvalidateMeasure();
        }

        /// <summary>
        /// ContentWidth 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">ContentWidth 属性的旧值。</param>
        /// <param name="newValue">ContentWidth 属性的新值。</param>
        protected virtual void OnContentWidthChanged(double oldValue, double newValue)
        {
            InvalidateMeasure();
        }

        private static void OnAnimationChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (DoubleAnimation)args.OldValue;
            var newValue = (DoubleAnimation)args.NewValue;
            if (oldValue == newValue)
            {
                return;
            }

            var target = obj as Resizer;
            target?.OnAnimationChanged(oldValue, newValue);
        }

        private static void OnContentHeightChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (double)args.OldValue;
            var newValue = (double)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as Resizer;
            target?.OnContentHeightChanged(oldValue, newValue);
        }

        private static void OnContentWidthChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (double)args.OldValue;
            var newValue = (double)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as Resizer;
            target?.OnContentWidthChanged(oldValue, newValue);
        }

        private void ChangeSize(bool useAnimation)
        {
            if (InnerContentControl == null)
            {
                return;
            }

            if (useAnimation == false)
            {
                ContentHeight = InnerContentControl.ActualHeight;
                ContentWidth = InnerContentControl.ActualWidth;
            }
            else
            {
                _resizingStoryboard.Stop();

                _isResizing = true;
                ResizingStoryboard.Begin();
            }
        }

        private void OnInnerContentMeasuring(object sender, EventArgs e)
        {
            _isInnerContentMeasuring = true;
            _isResizing = false;
        }

        private void OnResizingCompleted(object sender, object e)
        {
            _isResizing = false;
        }
    }
}
