﻿using OnePomodoro.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Microsoft.Toolkit.Uwp.UI.Animations;

using Windows.UI;
using System.Numerics;
using Windows.UI.Composition;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace OnePomodoro.PomodoroViews
{
    [Title("The Big One")]
    [Screenshot("/Assets/Screenshots/TheBigOne.png")]
    [FunctionTags( Tags.PointLight)]
    [CompactOverlay]
    [SourceCode("https://github.com/DinoChan/OnePomodoro/blob/master/OnePomodoro/OnePomodoro/PomodoroViews/TheBigOne.xaml.cs")]
    public sealed partial class TheBigOne : PomodoroView
    {
        private PointLight _inworkPointLight;
        private PointLight _breakPointLight;

        public TheBigOne()
        {
            this.InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await AnimationBuilder.Create().Opacity(to: 0, duration: TimeSpan.FromSeconds(0)).StartAsync(LayoutRoot);
            await AnimationBuilder.Create().Opacity(to: 1, duration: TimeSpan.FromSeconds(1.5)).StartAsync(LayoutRoot);
            ShowTextShimming();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            StopTextShimming();
        }



        private void ShowTextShimming()
        {
            if (_inworkPointLight != null)
                return;

            var compositor = Window.Current.Compositor;
            if (_inworkPointLight == null)
            {
                var titleVisual = VisualExtensions.GetVisual(InworkShadow);
                _inworkPointLight = compositor.CreatePointLight();

                _inworkPointLight.Color = Color.FromArgb(255, 217, 17, 83);
                _inworkPointLight.CoordinateSpace = titleVisual;
                _inworkPointLight.Targets.Add(titleVisual);
                _inworkPointLight.Offset = new Vector3(-(float)PomodoroPanel.ActualWidth * 3, (float)PomodoroPanel.ActualHeight / 2, 300.0f);
            }
            var offsetAnimation = compositor.CreateScalarKeyFrameAnimation();
            offsetAnimation.InsertKeyFrame(1.0f, (float)PomodoroPanel.ActualWidth * 3, compositor.CreateLinearEasingFunction());
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(10000);
            offsetAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

            _inworkPointLight.StartAnimation("Offset.X", offsetAnimation);



            if (_breakPointLight == null)
            {
                var titleVisual = VisualExtensions.GetVisual(BreakShadow);
                _breakPointLight = compositor.CreatePointLight();

                _breakPointLight.Color = Color.FromArgb(255, 217, 17, 83);
                _breakPointLight.CoordinateSpace = titleVisual;
                _breakPointLight.Targets.Add(titleVisual);
                _breakPointLight.Offset = new Vector3(-(float)PomodoroPanel.ActualWidth * 3, (float)PomodoroPanel.ActualHeight / 2, 300.0f);
            }
            offsetAnimation = compositor.CreateScalarKeyFrameAnimation();
            offsetAnimation.InsertKeyFrame(1.0f, (float)PomodoroPanel.ActualWidth * 3, compositor.CreateLinearEasingFunction());
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(10000);
            offsetAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

            _breakPointLight.StartAnimation("Offset.X", offsetAnimation);
        }

        private void StopTextShimming()
        {
            //_inworkPointLight?.StopAnimation("Offset.X");
            //_breakPointLight?.StopAnimation("Offset.X");
        }
    }
}
