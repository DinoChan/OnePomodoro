using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;


/// <summary>
/// https://docs.microsoft.com/en-us/uwp/api/windows.ui.composition.compositionellipsegeometry
/// https://docs.microsoft.com/en-us/uwp/api/windows.ui.composition.compositor.createellipsegeometry
/// </summary>
namespace OnePomodoro.PomodoroViews
{
    class EllipseClipView
    {
        public EllipseClipView()
        {
            var clip = CompositionEllipseGeometry.();
        }
    }
}
