using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal abstract class Drawer {

        public virtual float GetWeight(SerializedProperty property) {
            var attributes = property.GetCustomAttributes<WeightAttribute>();
            float result = 0;
            foreach (var attribute in attributes){
              result += attribute.Weight;
            }
            return attributes.Length != 0 ? result : 1;
        }

        public virtual float GetFixedWidth(SerializedProperty property) {
            var attributes = property.GetCustomAttributes<WidthAttribute>();
            float result = 0;
            foreach (var attribute in attributes){
              result += attribute.Width;
            }
            return attributes.Length != 0 ? result : 0;
        }

        public abstract void Draw(Rect rect, SerializedProperty property);

        protected void DrawHighlight(Rect rect, SerializedProperty property) {
            var attribute = property.GetCustomAttribute<HighlightAttribute>();
            if (attribute != null) {
                GuiUtil.DrawRect(rect.WithBounds(1), attribute.Color);
            }
        }
    }
}
