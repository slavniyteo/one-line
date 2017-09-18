using System;
using UnityEngine;

namespace OneLine {
    ///<summary>
    ///Draws marked field into one line in InspectorWindow.
    ///Into one line will be fitted all inner fields (even arrays)
    ///which usual is presented in InspectorWindow in bulky weird view.
    ///Marked field is called zero-depth field or d0-field.
    ///Depends on depth, internal fields are called: d1, d2, etc.
    ///All internal fields in one scope are called d1+fields.
    ///</summary>
    [AttributeUsage(validOn: AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class OneLineAttribute : PropertyAttribute {
        public LineHeader Header { get; set; }
    }

    public enum LineHeader {
        None = 0,
        Short = 1
    }
}
