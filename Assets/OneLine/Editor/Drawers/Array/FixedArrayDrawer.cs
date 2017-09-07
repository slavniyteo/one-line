using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class FixedArrayDrawer : Drawer {

        private Func<SerializedProperty, Drawer> getDrawer;

        public FixedArrayDrawer(Func<SerializedProperty, Drawer> drawer) {
            this.getDrawer = drawer;
        }

        #region Width

        public override float GetWeight(SerializedProperty property) {
            return property.GetArrayElements()
                   .Select(element => getDrawer(element).GetWeight(element))
                   .Sum();
        }

        public override float GetFixedWidth(SerializedProperty property) {
            return property.GetArrayElements()
                   .Select(element => getDrawer(element).GetFixedWidth(element) + 5)
                   .Sum();
        }

        #endregion

        #region Draw

        public override void Draw(Rect rect, SerializedProperty property) {
            int length = GetLength(property);
            Rect[] rects = SplitRects(rect, property);

            for (int i = 0; i < length; i++) {
                DrawElement(rects[i], property.GetArrayElementAtIndex(i));
            }
        }

        protected Rect[] SplitRects(Rect rect, SerializedProperty property) {
            return rect.Split(GetWeights(property), GetFixedWidthes(property));
        }

        protected virtual void DrawElement(Rect rect, SerializedProperty element) {
            getDrawer(element).Draw(rect, element);
        }

        protected virtual int GetLength(SerializedProperty property) {
            var attribute = property.GetCustomAttribute<ArrayLengthAttribute>();
            if (attribute == null) {
                throw new InvalidOperationException(string.Format("Can not find ArrayLengthAttribute at property {1)", property.propertyPath));
            }
            property.arraySize = attribute.Length;
            return property.arraySize;
        }

        protected virtual float[] GetWeights(SerializedProperty property) {
            return property.GetArrayElements()
                   .Select(element => getDrawer(element).GetWeight(element))
                   .ToArray();
        }

        protected virtual float[] GetFixedWidthes(SerializedProperty property) {
            return property.GetArrayElements()
                   .Select(element => getDrawer(element).GetFixedWidth(element))
                   .ToArray();
        }

        #endregion

    }
}
