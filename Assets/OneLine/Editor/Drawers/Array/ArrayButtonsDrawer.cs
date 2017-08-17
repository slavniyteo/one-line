using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class ArrayButtonsDrawer : Drawer {

        #region Width

        public float GetWeight(SerializedProperty property) {
            bool needDrawLabel = NeedDrawLabel(property);
            bool needDrawButtons = NeedDrawButtons(property);
            return needDrawLabel && !needDrawButtons ? 1 : 0;
        }

        public float GetFixedWidth(SerializedProperty property) {
            return NeedDrawButtons(property) ? 45 : 0;
        }

        private bool NeedDrawButtons(SerializedProperty property) {
            var attribute = property.GetCustomAttribute<HideButtonsAttribute>();
            return attribute == null;
        }

        private bool NeedDrawLabel(SerializedProperty property) {
            return property.arraySize == 0;
        }

        #endregion

        public void Draw(Rect rect, SerializedProperty array) {
            if (NeedDrawButtons(array)) {
                DrawButtons(rect, array);
            }
            else if (NeedDrawLabel(array)) {
                DrawLabel(rect, array);
            }
        }

        public void DrawLabel(Rect rect, SerializedProperty array) {
            var rects = rect.Split(new float[] { 1, 0 }, new float[] { 0, 20 });

            EditorGUI.LabelField(rects[0], array.displayName);
            if (GUI.Button(rects[1], "+")) {
                array.InsertArrayElementAtIndex(0);
            }
        }

        public void DrawButtons(Rect rect, SerializedProperty array) {
            var rects = rect.Split(new float[] { 0, 0 }, new float[] { 20, 20 });

            if (GUI.Button(rects[0], "+")) {
                array.InsertArrayElementAtIndex(array.arraySize);
            }
            if (GUI.Button(rects[1], "-")) {
                array.DeleteArrayElementAtIndex(array.arraySize - 1);
            }
        }

    }
}
