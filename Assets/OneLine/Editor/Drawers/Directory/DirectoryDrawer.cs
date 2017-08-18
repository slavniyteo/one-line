using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class DirectoryDrawer : Drawer {

        protected Drawer simpleDrawer;
        protected Drawer fixedLengthArrayDrawer;
        protected Drawer arrayDrawer;
        protected Drawer directoryDrawer;

        public DirectoryDrawer() {
            simpleDrawer = new SimpleFieldDrawer();
            fixedLengthArrayDrawer = new FixedLengthArray(GetDrawer);
            arrayDrawer = new ArrayDrawer(GetDrawer);
            directoryDrawer = this;
        }

       #region Weights
        public float GetWeight(SerializedProperty property) {
            var attributes = property.GetCustomAttributes<WeightAttribute>();
            float result = 0;
            foreach (var attribute in attributes){
              result += attribute.Weight;
            }
            float multiplier = attributes.Length != 0 ? result : 1;

            return GetFieldWeights(property)
                   .Select(x => x * multiplier)
                   .Sum();
        }

        private float[] GetFieldWeights(SerializedProperty property) {
            return property
                   .GetChildren()
                   .Select(x => GetDrawer(x).GetWeight(x))
                   .ToArray();
        }

        public float GetFixedWidth(SerializedProperty property) {
            var attributes = property.GetCustomAttributes<WidthAttribute>();
            float result = 0;
            foreach (var attribute in attributes){
              result += attribute.Width;
            }
            float width = attributes.Length != 0 ? result : 0;

            return Math.Max(width, GetFieldFixedWidthes(property).Sum());
        }

        private float[] GetFieldFixedWidthes(SerializedProperty property) {
            return property
                   .GetChildren()
                   .Select(x => GetDrawer(x).GetFixedWidth(x))
                   .ToArray();
        }

        #endregion

        public virtual void Draw(Rect rect, SerializedProperty property) {
            var rects = GetRects(rect, property);
            int i = 0;
            foreach (var child in property.GetChildren()) {
                DrawField(rects[i], child);
                i++;
            }
        }

        protected Rect[] GetRects(Rect rect, SerializedProperty property) {
            return rect.Split(GetFieldWeights(property), GetFieldFixedWidthes(property));
        }

        private void DrawField(Rect rect, SerializedProperty property) {
            DrawColor(rect, property);
            GetDrawer(property).Draw(rect, property);
            DrawTooltip(rect, property);
        }

        protected void DrawColor(Rect rect, SerializedProperty child) {
            var attribute = child.GetCustomAttribute<HighlightAttribute>();
            if (attribute != null) {
                attribute.Draw(rect);
            }
        }

        protected Drawer GetDrawer(SerializedProperty property) {
            if (property.isArray && !(property.propertyType == SerializedPropertyType.String)) {
                if (property.GetCustomAttribute<ArrayLengthAttribute>() == null) {
                    return arrayDrawer;
                }
                else {
                    return fixedLengthArrayDrawer;
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
            var attribute = child.GetCustomAttribute<TooltipAttribute>();
            if (attribute != null) {
                var guiContent = new GUIContent("", attribute.tooltip);
                EditorGUI.LabelField(rect, guiContent);
            }
        }

    }
}
