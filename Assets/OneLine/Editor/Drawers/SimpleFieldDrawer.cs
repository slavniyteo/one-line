using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class SimpleFieldDrawer : Drawer {

        public override void Draw(Rect rect, SerializedProperty property) {
            EditorGUI.BeginProperty(rect, GUIContent.none, property);
            EditorGUI.PropertyField(rect, property, GUIContent.none);
            EditorGUI.EndProperty();
        }

    }
}
