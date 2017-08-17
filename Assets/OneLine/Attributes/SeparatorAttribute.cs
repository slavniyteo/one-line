using System;
using UnityEngine;

namespace OneLine {
    ///<summary>
    ///Draws horizontal or vertical separators
    ///</summary>
    [AttributeUsage(validOn: AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class SeparatorAttribute : PropertyAttribute {

        public SeparatorAttribute() {
            Text = "";
            Thickness = 2;
        }

        public SeparatorAttribute(string text) : this() {
            Text = text;
        }

        public string Text { get; set; }
        public int Thickness { get; set; }

    }
}
