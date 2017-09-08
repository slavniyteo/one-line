using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class DirectoryDrawer : ComplexFieldDrawer {


        public DirectoryDrawer(DrawerProvider getDrawer) : base(getDrawer) {
        }

        protected override IEnumerable<SerializedProperty> GetChildren(SerializedProperty property){
            return property.GetChildren();
        }

        protected override void DrawField(Rect rect, SerializedProperty property) {
            DrawHighlight(rect, property);
            getDrawer(property).Draw(rect, property);
            DrawTooltip(rect, property);
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
