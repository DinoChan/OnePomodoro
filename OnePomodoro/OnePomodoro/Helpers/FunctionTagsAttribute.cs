using System;

namespace OnePomodoro.Helpers
{
    public class FunctionTagsAttribute : Attribute
    {
        public FunctionTagsAttribute(params string[] tags)
        {
            Tags = tags;
        }

        public string[] Tags { get; }
    }

    public class Tags
    {
        public const string CompositionAnimation = "Composition Animation";
        public const string SpringAnimation = "Spring Animation";
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
        public const string Win2D = "Win2D";
        public const string ImplicitAnimation = "Implicit Animation";
    }
}
