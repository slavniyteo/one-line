using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Nihil.OneLine {
    internal class SimpleFieldDrawer : Drawer {

        public float GetWeight(SerializedProperty property) {
            var attribute = property.GetCustomAttribute<WeightAttribute>();
            return attribute != null ? attribute.Weight : 1;
        }

        public float GetFixedWidth(SerializedProperty property) {
            var attribute = property.GetCustomAttribute<WidthAttribute>();
            return attribute != null ? attribute.Width : 0;
        }

        public void Draw(Rect rect, SerializedProperty property) {
            EditorGUI.BeginProperty(rect, GUIContent.none, property);
            EditorGUI.PropertyField(rect, property, GUIContent.none);
            EditorGUI.EndProperty();
        }

    }
}