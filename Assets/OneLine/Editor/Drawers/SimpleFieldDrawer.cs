using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class SimpleFieldDrawer : Drawer {

        public override void Draw(Rect rect, SerializedProperty property) {
            EditorGUI.BeginProperty(rect, GUIContent.none, property);
            DrawProperty(rect, property);
            EditorGUI.EndProperty();
        }

        /*
         * WORKAROUND
         * Unity3d `feature`: EditorGUI.PropertyField draws field 
         * with all decorators (like Header, Space, etc) and this behaviour 
         * can not be ommited.
         * see: http://answers.unity3d.com/questions/1394991/how-to-preserve-drawing-decoratordrawer-like-heade.html
         * Headers and Separators (provided by one-line) produces artefacts.
         * We solve this problem with reflection, but we call internal method
         * and this may be dangerous: unity3d developers may change API =(
         */
        private void DrawProperty(Rect rect, SerializedProperty property){
            //EditorGUI.PropertyField(rect, property, GUIContent.none);
            typeof(EditorGUI)
                .GetMethod("DefaultPropertyField", BindingFlags.NonPublic | BindingFlags.Static)
                .Invoke(null, new object[]{rect, property, GUIContent.none});
        }

    }
}
