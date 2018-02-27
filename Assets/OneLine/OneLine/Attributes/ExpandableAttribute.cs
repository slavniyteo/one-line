using System;
using UnityEngine;

namespace OneLine {
    ///<summary>
    ///Draws horizontal or vertical separator
    ///</summary>
    [AttributeUsage(validOn: AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public sealed class ExpandableAttribute : PropertyAttribute {

        public ExpandableAttribute() {
        }

    }
}
