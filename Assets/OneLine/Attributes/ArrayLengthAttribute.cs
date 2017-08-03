using System;

namespace Nihil.OneLine {
    ///<summary>
    ///Make marked array immutable.
    ///Marked array misses buttons "+" and "-" and context menu.
    ///</summary>
    [AttributeUsage(validOn: AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class ArrayLengthAttribute : Attribute {
        private int length;

        public ArrayLengthAttribute(int lenth) {
            this.length = lenth;
        }

        public int Length { get { return length; } }
    }
}