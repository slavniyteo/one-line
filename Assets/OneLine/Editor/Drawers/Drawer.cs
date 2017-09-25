using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal abstract class Drawer {

        public abstract void AddSlices(SerializedProperty property, Slices slices);

        protected void DrawHighlight(Rect rect, SerializedProperty property) {
            property.GetCustomAttribute<HighlightAttribute>()
                    .IfPresent(x => GuiUtil.DrawRect(rect.Expand(1), x.Color));
        }

        protected void DrawTooltip(Rect rect, SerializedProperty child) {
            string tooltip = child.displayName;

            var attribute = child.GetCustomAttribute<TooltipAttribute>();
            if (attribute != null) {
                tooltip = attribute.tooltip;
            }

            EditorGUI.LabelField(rect, new GUIContent("", tooltip));
        }
    }
}
