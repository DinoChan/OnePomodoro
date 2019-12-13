using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePomodoro.Helpers
{
    public class FunctionTagsAttribute : Attribute
    {
        public FunctionTagsAttribute(params string[] tags)
        {
            Tage = tags;
        }

        public string[] Tage { get; }
    }

    public class Tags
    {
        public const string CompositionAnimation = "CompositionAnimation";
        public const string SpringAnimation = "SpringAnimation";
        public const string CompositionGeometricClip = "CompositionGeometricClip";
        public const string CompositionLinearGradientBrush = "CompositionLinearGradientBrush";
        public const string BlendEffect = "BlendEffect";
        public const string PointLight = "PointLight";
        public const string AmbientLight = "AmbientLight";
        public const string Clip = "Clip";
        public const string ContainerVisual = "ContainerVisual";
        public const string ExpressionAnimation = "ExpressionAnimation";
        public const string CompositionMaskBrush = "CompositionMaskBrush";
        public const string RotationAngle = "RotationAngle";
        public const string TransformMatrix = "TransformMatrix";
    }
}
