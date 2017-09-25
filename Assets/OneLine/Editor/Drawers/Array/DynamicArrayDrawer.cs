using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class DynamicArrayDrawer : FixedArrayDrawer {
        private Drawer buttons;

        public DynamicArrayDrawer(DrawerProvider getDrawer) : base(getDrawer) {
            buttons = new ArrayButtonsDrawer();
        }

        #region Width

        public override void AddSlices(SerializedProperty property, Slices slices){
            base.AddSlices(property, slices);
            buttons.AddSlices(property, slices);
        }

        #endregion

        #region Draw

        protected override int GetLength(SerializedProperty property) {
            return property.arraySize;
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


        #endregion

    }
}
