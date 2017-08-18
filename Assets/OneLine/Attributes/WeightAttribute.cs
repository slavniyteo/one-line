using System;

namespace OneLine {
    ///<summary>
    ///Defines weight of marked field in the line.
    ///Available only on d1+fields and has no effect on d0.
    ///Fields without WeightAttribute has default weight = 1.
    ///Applied to arrays defines weight of each element.
    ///</summary>
    [AttributeUsageAttribute(validOn: AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class WeightAttribute : Attribute {
        private float weight;

        public WeightAttribute(float weight) {
            this.weight = weight;
        }

        public float Weight { get { return weight; } }
    }
}
