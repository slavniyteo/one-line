using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using OneLine;
using RectEx;
using System;

namespace OneLine {
    [CustomPropertyDrawer(typeof(ExpandableAttribute))]
    public class ExpandableAttributeDrawer : PropertyDrawer {

        private GUIStyle foldoutStyle;
    
        public override void OnGUI (Rect rect, SerializedProperty property, GUIContent label) {
            rect = EditorGUI.PrefixLabel(rect, label);

            var rects = rect.CutFromLeft(14, 0);
    
            EditorGUI.PropertyField (rects[1], property, GUIContent.none, true);
    
            if (property.objectReferenceValue == null) return;
        
            DrawFoldout(rects[0], rects[1], property);
        }

        private void DrawFoldout(Rect foldoutRect, Rect propertyRect, SerializedProperty property){
            if (foldoutStyle == null) {
                foldoutStyle = new GUIStyle(EditorStyles.foldout);
                foldoutStyle.active = foldoutStyle.normal;
            }

            if (GUI.Button(foldoutRect, "", foldoutStyle)){
                PopupWindow.Show(propertyRect.MoveDownFor(0), new ExpandedObjectWindow(propertyRect, property));
            }
        }

        public class ExpandedObjectWindow : PopupWindowContent {

            private Vector2 size;
            private SerializedProperty property;

            public ExpandedObjectWindow(Rect rect, SerializedProperty property){
                this.size = new Vector2(rect.width, 0);
                this.property = property;
            }

            public override Vector2 GetWindowSize() {
                return size;
            }

            public override void OnGUI(Rect rect) {
                DrawExpandedObject(rect, new SerializedObject(property.objectReferenceValue));
            }

            private void DrawExpandedObject(Rect rect, SerializedObject target) {
                if (target == null) return; 

                rect = rect.Intend(5).FirstLine();

                DrawChildren(rect, target);
        
                target.ApplyModifiedProperties();
            }

            private void DrawChildren(Rect rect, SerializedObject target){
                var property = target.GetIterator();
                property.NextVisible (true);

                if (property.name == "m_Script"){
                    DrawScriptReference(rect, property);
                    rect = rect.MoveDown();
                    property.NextVisible(false);
                }
                
                do {
                    try {
                        rect.height = EditorGUI.GetPropertyHeight(property, true);
                        EditorGUI.PropertyField (rect, property, true);
                        rect.y += rect.height;
                    }
                    catch (StackOverflowException) {
                        property.objectReferenceValue = null;
                        Debug.LogError ("Detected self-nesting cauisng a StackOverflowException, avoid using the same " +
                            "object iside a nested structure.");
                    }
                }
                while (property.NextVisible (false));

                size.y = rect.y + 16;
            }

            private static void DrawScriptReference(Rect rect, SerializedProperty property){
                EditorGUI.BeginDisabledGroup (true);
                EditorGUI.PropertyField (rect, property, false);
                EditorGUI.EndDisabledGroup ();
            }
        }
    
    }
}
