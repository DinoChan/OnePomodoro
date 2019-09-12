using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Shapes;

namespace OnePomodoro.Controls
{
    public partial class DropShadowPanel : ButtonDecorator
    {
        private readonly Compositor _compositor;
        private CompositionMaskBrush _maskBrush;
        private readonly DropShadow _dropShadow;

        public DropShadowPanel() : base()
        {
            DefaultStyleKey = typeof(DropShadowPanel);
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _dropShadow = _compositor.CreateDropShadow();
            //_dropShadow.BlurRadius = 4;
            //_dropShadow.Color = Colors.White;
            //_dropShadow.Opacity = 0.7f;
            Visual.Shadow = _dropShadow;
            //Visual.Scale = new System.Numerics.Vector3(0.91f);
        }

        protected override void UpdateOutlineMask()
        {
            if (RelativeElement is Shape shape)
            {
                var mask = shape.GetAlphaMask();
                _dropShadow.Mask = mask;
            }
            else if (RelativeElement is TextBlock textBlock)
            {
                var mask = textBlock.GetAlphaMask();
                _dropShadow.Mask = mask;
            }
        }

        private void OnBlurRadiusChanged(double newValue)
        {
            if (_dropShadow != null)
            {
                _dropShadow.BlurRadius = (float)newValue;
            }
        }

        private void OnColorChanged(Color newValue)
        {
            if (_dropShadow != null)
            {
                _dropShadow.Color = newValue;
            }
        }

        private void OnOffsetXChanged(double newValue)
        {
            if (_dropShadow != null)
            {
                UpdateShadowOffset((float)newValue, _dropShadow.Offset.Y, _dropShadow.Offset.Z);
            }
        }

        private void OnOffsetYChanged(double newValue)
        {
            if (_dropShadow != null)
            {
                UpdateShadowOffset(_dropShadow.Offset.X, (float)newValue, _dropShadow.Offset.Z);
            }
        }

        private void OnOffsetZChanged(double newValue)
        {
            if ( _dropShadow != null)
            {
                UpdateShadowOffset(_dropShadow.Offset.X, _dropShadow.Offset.Y, (float)newValue);
            }
        }

        private void OnShadowOpacityChanged(double newValue)
        {
            if (_dropShadow != null)
            {
                _dropShadow.Opacity = (float)newValue;
            }
        }

        private void UpdateShadowOffset(float x, float y, float z)
        {
            if (_dropShadow != null)
            {
                _dropShadow.Offset = new Vector3(x, y, z);
            }
        }

    }
}
