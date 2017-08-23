using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal abstract class Drawer {
        public abstract float GetWeight(SerializedProperty property);
        public abstract float GetFixedWidth(SerializedProperty property);
        public abstract void Draw(Rect rect, SerializedProperty property);

        protected void DrawHighlight(Rect rect, SerializedProperty property) {
            var attribute = property.GetCustomAttribute<HighlightAttribute>();
            if (attribute != null) {
                GuiUtil.DrawRect(rect.WithBounds(1), attribute.Color);
            }
        }
    }
}
