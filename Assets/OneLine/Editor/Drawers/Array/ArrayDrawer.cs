using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class ArrayDrawer : FixedLengthArray {
        private Drawer buttons;

        public ArrayDrawer(Func<SerializedProperty, Drawer> drawer) : base(drawer) {
            buttons = new ArrayButtonsDrawer();
        }

        #region Width

        public override float GetWeight(SerializedProperty property) {
            return base.GetWeight(property) + buttons.GetWeight(property);
        }

        public override float GetFixedWidth(SerializedProperty property) {
            return base.GetFixedWidth(property) + buttons.GetFixedWidth(property);
        }

        #endregion

        #region Draw

        public override void Draw(Rect rect, SerializedProperty property) {
            base.Draw(rect, property);

            Rect[] rects = SplitRects(rect, property);
            buttons.Draw(rects[property.arraySize], property);
        }

        protected override int GetLength(SerializedProperty property) {
            return property.arraySize;
        }

        protected override void DrawElement(Rect rect, SerializedProperty element) {
            DrawElementContextMenu(rect, element);
            base.DrawElement(rect, element);
        }

        private void DrawElementContextMenu(Rect rect, SerializedProperty element) {
            Event current = Event.current;
            if (current.type == EventType.ContextClick && rect.Contains(current.mousePosition)) {
                current.Use();

                element = element.Copy();

                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Dublicate"), false, () => {
                    element.DuplicateCommand();
                    element.serializedObject.ApplyModifiedProperties();
                });
                menu.AddItem(new GUIContent("Delete"), false, () => {
                    element.DeleteCommand();
                    element.serializedObject.ApplyModifiedProperties();
                });
                menu.DropDown(rect);
            }
        }

        protected override float[] GetWeights(SerializedProperty property) {
            List<float> result = base.GetWeights(property).ToList();
            result.Add(buttons.GetWeight(property));
            return result.ToArray();
        }

        protected override float[] GetFixedWidthes(SerializedProperty property) {
            List<float> result = base.GetFixedWidthes(property).ToList();
            result.Add(buttons.GetFixedWidth(property));
            return result.ToArray();
        }

        #endregion

    }
}
