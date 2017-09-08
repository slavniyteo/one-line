using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class DirectoryDrawer : ComplexFieldDrawer {

        protected Drawer simpleDrawer;
        protected Drawer fixedArrayDrawer;
        protected Drawer dynamicArrayDrawer;
        protected Drawer directoryDrawer;

        public DirectoryDrawer() {
            simpleDrawer = new SimpleFieldDrawer();
            fixedArrayDrawer = new FixedArrayDrawer(GetDrawer);
            dynamicArrayDrawer = new DynamicArrayDrawer(GetDrawer);
            directoryDrawer = this;

            getDrawer = GetDrawer;
        }

        protected override IEnumerable<SerializedProperty> GetChildren(SerializedProperty property){
            return property.GetChildren();
        }

        protected override void DrawField(Rect rect, SerializedProperty property) {
            DrawHighlight(rect, property);
            GetDrawer(property).Draw(rect, property);
            DrawTooltip(rect, property);
        }

        protected Drawer GetDrawer(SerializedProperty property) {
            if (property.isArray && !(property.propertyType == SerializedPropertyType.String)) {
                if (property.GetCustomAttribute<ArrayLengthAttribute>() == null) {
                    return dynamicArrayDrawer;
                }
                else {
                    return fixedArrayDrawer;
                }
            }
            else if (property.hasVisibleChildren) {
                return directoryDrawer;
            }
            else {
                return simpleDrawer;
            }
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
