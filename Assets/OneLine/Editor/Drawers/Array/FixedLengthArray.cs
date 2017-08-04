using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class FixedLengthArray : Drawer {

        private RectUtil rectUtil;
        private Func<SerializedProperty, Drawer> getDrawer;

        public FixedLengthArray(Func<SerializedProperty, Drawer> drawer) {
            this.rectUtil = new RectUtil();
            this.getDrawer = drawer;
        }

        #region Width

        public virtual float GetWeight(SerializedProperty property) {
            return property.GetArrayElements()
                   .Select(element => getDrawer(element).GetWeight(element))
                   .Sum();
        }

        public virtual float GetFixedWidth(SerializedProperty property) {
            return property.GetArrayElements()
                   .Select(element => getDrawer(element).GetFixedWidth(element) + 5)
                   .Sum();
        }

        #endregion

        #region Draw

        public virtual void Draw(Rect rect, SerializedProperty property) {
            int length = GetLength(property);
            Rect[] rects = SplitRects(rect, property);

            for (int i = 0; i < length; i++) {
                DrawElement(rects[i], property.GetArrayElementAtIndex(i));
            }
        }

        protected Rect[] SplitRects(Rect rect, SerializedProperty property) {
            return rectUtil.Split(rect, GetWeights(property), GetFixedWidthes(property));
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
