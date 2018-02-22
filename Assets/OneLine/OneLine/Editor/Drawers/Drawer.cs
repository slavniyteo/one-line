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

        protected MetaSlice DrawHighlight(SerializedProperty property, Slices slices, int before, int after) {
            var attribute = property.GetCustomAttribute<HighlightAttribute>();
            if (attribute == null) return null;

            var slice = new MetaSlice(before, after, 
                                      rect => GuiUtil.DrawRect(rect.Extend(1), attribute.Color));
            slices.Add(slice);
            return slice;
        }

        protected void DrawTooltip(SerializedProperty property, Slices slices, int before, int after) {
            var attribute = property.GetCustomAttribute<TooltipAttribute>();
            if (attribute == null) return;

            var slice = new MetaSlice(before, after,
                                      rect => EditorGUI.LabelField(rect, new GUIContent("", attribute.tooltip)));
            slices.Add(slice);
        }
    }
}
