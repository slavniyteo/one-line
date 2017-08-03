using System;

namespace Nihil.OneLine {
    ///<summary>
    ///Defines additive fixed width (in GUI pixels) on marked field in the line.
    ///By default sets weight of field to 0.
    ///Available only on d1+fields and has no effect on d0.
    ///If field is marked by WidthAttribute and WeightAttribute, it gets both effects additively.
    ///Applied to arrays defines width of each element.
    ///</summary>
    [AttributeUsageAttribute(validOn: AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class WidthAttribute : WeightAttribute {
        private float width;

        public WidthAttribute(float width) : base(0) {
            this.width = width;
        }

        public float Width { get { return width; } }

    }
}