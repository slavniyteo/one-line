using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class SimpleFieldDrawer : Drawer {

        public override float GetWeight(SerializedProperty property) {
            var attributes = property.GetCustomAttributes<WeightAttribute>();
            float result = 0;
            foreach (var attribute in attributes){
              result += attribute.Weight;
            }
            return attributes.Length != 0 ? result : 1;
        }

        public override float GetFixedWidth(SerializedProperty property) {
            var attributes = property.GetCustomAttributes<WidthAttribute>();
            float result = 0;
            foreach (var attribute in attributes){
              result += attribute.Width;
            }
            return attributes.Length != 0 ? result : 0;
        }

        public override void Draw(Rect rect, SerializedProperty property) {
            EditorGUI.BeginProperty(rect, GUIContent.none, property);
            EditorGUI.PropertyField(rect, property, GUIContent.none);
            EditorGUI.EndProperty();
        }

    }
}
