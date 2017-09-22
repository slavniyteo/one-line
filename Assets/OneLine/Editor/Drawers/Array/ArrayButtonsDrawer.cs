using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class ArrayButtonsDrawer : Drawer {

        #region Width

        public override void AddSlices(SerializedProperty property, Slices slices){
            if (NeedDrawButtons(property)){
                slices.Add(new Slice(0, 20, rect => DrawPlusButton(rect, property.Copy())));
                slices.Add(new Slice(0, 20, rect => DrawMinusButton(rect, property.Copy())));
            }
            else if (NeedDrawLabel(property)){
                slices.Add(new Slice(1,0, rect => DrawLabel(rect, property.Copy())));
            }
        }

        private bool NeedDrawButtons(SerializedProperty property) {
            var attribute = property.GetCustomAttribute<HideButtonsAttribute>();
            return attribute == null;
        }

        private bool NeedDrawLabel(SerializedProperty property) {
            return property.arraySize == 0;
        }

        #endregion

        public void DrawLabel(Rect rect, SerializedProperty array) {
            var rects = rect.Split(new float[] { 1, 0 }, new float[] { 0, 20 });

            EditorGUI.LabelField(rects[0], array.displayName);
            if (GUI.Button(rects[1], "+")) {
                array.InsertArrayElementAtIndex(0);
            }
        }

        public void DrawPlusButton(Rect rect, SerializedProperty array) {
            if (GUI.Button(rect, "+")) {
                array.InsertArrayElementAtIndex(array.arraySize);
            }
        }

        public void DrawMinusButton(Rect rect, SerializedProperty array) {
            if (array.arraySize > 0 && GUI.Button(rect, "-")) {
                array.DeleteArrayElementAtIndex(array.arraySize - 1);
            }
        }
    }
}
