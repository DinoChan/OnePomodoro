﻿using System;
using System.Collections.Generic;
using OnePomodoro.ViewModels;
using Windows.UI.Xaml.Controls;

namespace OnePomodoro.PomodoroViews
{
    public class PomodoroView : UserControl
    {
        public PomodoroView()
        {
            DataContext = PomodoroViewModel.Current;
        }

        public static IEnumerable<Type> Views { get; } = new List<Type>
        {
            typeof(LongShadow),
            typeof(AudioCall),
            typeof(TheFirst),
            typeof(TheBigOne),
            typeof(SimpleCircle),
            typeof(Gradients),
            typeof(GradientsWithBlend),
            typeof(SpringTextView),
            typeof(KonosubaView),
            typeof(HiddenTextView),
            typeof(WhiteTextView),
            typeof(OutlineTextView),
            typeof(ShadowTextView),
            typeof(SplitTo5View),
            typeof(EllipseClipView),
            typeof(DoNotDisturbView),
        };

        public PomodoroViewModel ViewModel => DataContext as PomodoroViewModel;
    }
}
