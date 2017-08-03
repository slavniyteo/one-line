using System;

namespace Nihil.OneLine {
    ///<summary>
    ///Hides prefix label of d0-field.Has no effect on d1+fields.
    ///Useful for expanding available space in line.
    ///Applied to arrays hides label of elements ("Element 1", "Element 2" etc)
    ///</summary>
    [AttributeUsageAttribute(validOn: AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class HideLabelAttribute : Attribute {

    }
}