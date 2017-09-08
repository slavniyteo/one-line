using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class RootDirectoryDrawer : DirectoryDrawer {

        public RootDirectoryDrawer(DrawerProvider getDrawer) : base(getDrawer) {
        }

        public override void Draw(Rect totalRect, SerializedProperty property) {
            DrawHighlight(totalRect, property);
            var indentedRect = DrawLabel(totalRect, property);
            EditorGUI.indentLevel = 0;

            EditorGUI.BeginProperty(totalRect, GUIContent.none, property);
            getDrawer(property).Draw(indentedRect, property);
            EditorGUI.EndProperty();
        }

        private static Rect DrawLabel(Rect rect, SerializedProperty property) {
            var attribute = property.GetCustomAttribute<HideLabelAttribute>();
            if (attribute == null) {
                var tooltipAttribute = property.GetCustomAttribute<TooltipAttribute>();
                string tooltip = tooltipAttribute != null ? tooltipAttribute.tooltip : null;

                var label = new GUIContent(property.displayName, tooltip);
                rect = EditorGUI.PrefixLabel(rect, label);
            }
            else {
                rect = EditorGUI.IndentedRect(rect);
            }
            return rect;
        }
    }
}
