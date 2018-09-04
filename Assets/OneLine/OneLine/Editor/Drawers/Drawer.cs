using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using RectEx;

namespace OneLine {
    internal abstract class Drawer {

        public abstract void AddSlices(SerializedProperty property, Slices slices);

        protected void DrawHighlight(SerializedProperty property, Slices slices) {
            var attribute = property.GetCustomAttribute<HighlightAttribute>();
            if (attribute == null) return;

            var slice = new Drawable(rect => GuiUtil.DrawRect(rect.Extend(1), attribute.Color));
            slices.AddBefore(slice);
        }

        protected void DrawTooltip(SerializedProperty property, Slices slices) {
            var attribute = property.GetCustomAttribute<TooltipAttribute>();
            if (attribute == null) return;

            var slice = new Drawable(rect => EditorGUI.LabelField(rect, new GUIContent("", attribute.tooltip)));
            slices.AddAfter(slice);
        }
    }
}
