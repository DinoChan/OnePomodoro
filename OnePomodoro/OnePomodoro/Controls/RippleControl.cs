using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace OnePomodoro.Controls
{
    public class RippleControl : Control
    {
        private CompositionEllipseGeometry _geomerty;

        public RippleControl()
        {
            this.DefaultStyleKey = typeof(RippleControl);
            MakeClip();
            StartClipAnimation();
        }


        /// <summary>
        /// 获取或设置IsShwoRipple的值
        /// </summary>
        public bool IsShowRipple
        {
            get => (bool)GetValue(IsShowRippleProperty);
            set => SetValue(IsShowRippleProperty, value);
        }

        /// <summary>
        /// 标识 IsShwoRipple 依赖属性。
        /// </summary>
        public static readonly DependencyProperty IsShowRippleProperty =
            DependencyProperty.Register(nameof(IsShowRipple), typeof(bool), typeof(RippleControl), new PropertyMetadata(default(bool), OnIsShwoRippleChanged));

        private static void OnIsShwoRippleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (bool)args.OldValue;
            var newValue = (bool)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as RippleControl;
            target?.OnIsShwoRippleChanged(oldValue, newValue);
        }

        /// <summary>
        /// IsShwoRipple 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">IsShwoRipple 属性的旧值。</param>
        /// <param name="newValue">IsShwoRipple 属性的新值。</param>
        protected virtual void OnIsShwoRippleChanged(bool oldValue, bool newValue)
        {
            StartClipAnimation();
        }

        private void MakeClip()
        {
            var compositor = Window.Current.Compositor;
            var visual = ElementCompositionPreview.GetElementVisual(this);
            _geomerty = CreateEllipseGeometry();
            var clip = compositor.CreateGeometricClip(_geomerty);
            visual.Clip = clip;
        }

        private CompositionEllipseGeometry CreateEllipseGeometry()
        {
            var compositor = Window.Current.Compositor;
            var ellipseGeomerty = compositor.CreateEllipseGeometry();
            ellipseGeomerty.Center = new Vector2((float)ActualWidth / 2, (float)ActualHeight / 2);
            ellipseGeomerty.Radius = Vector2.Zero;
            this.SizeChanged += (s, e) =>
            {
                ellipseGeomerty.Center = e.NewSize.ToVector2() / 2;
            };
            return ellipseGeomerty;
        }


        private void StartClipAnimation()
        {
            if (_geomerty == null)
                return;


            var compositor = Window.Current.Compositor;
            var animation = compositor.CreateVector2KeyFrameAnimation();

            animation.DelayTime = TimeSpan.FromSeconds(0.0);
            animation.Duration = TimeSpan.FromSeconds(1.5);
            if (IsShowRipple)
                animation.InsertKeyFrame(1, new Vector2((float)ActualWidth * 1.2f, (float)ActualHeight * 1.2f));
            else
                animation.InsertKeyFrame(1, Vector2.Zero);

            _geomerty.StartAnimation(nameof(CompositionEllipseGeometry.Radius), animation);
        }

    }
}
