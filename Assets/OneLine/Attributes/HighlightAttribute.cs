using System;
using UnityEngine;

namespace Nihil.OneLine {
    ///<summary>
    ///Highlights marked field by rgb color with values in range[0..1] (red by default)
    ///Available on d0 and d1+fields.
    ///If d0-field is highlighted its label prefix is highlighted too.
    ///</summary>
    [AttributeUsageAttribute(validOn: AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class HighlightAttribute : Attribute {
        public Color color;

        public HighlightAttribute() {
            color = new Color(1, 0, 0, 0.6f);
        }

        public HighlightAttribute(float red, float green, float blue, float alpha = 0.6f) {
            color = new Color(red, green, blue, alpha);
        }

        public Color Color { get { return color; } }

        public void Draw(Rect rect) {
            var back = GUI.backgroundColor;
            GUI.backgroundColor = color;

            const int bound = 1;
            rect = new Rect(
                x: rect.x - bound,
                y: rect.y - bound,
                width: rect.width + bound * 2,
                height: rect.height + bound * 2
            );
            GUI.Box(rect, "");
            GUI.backgroundColor = back;
        }

    }
}