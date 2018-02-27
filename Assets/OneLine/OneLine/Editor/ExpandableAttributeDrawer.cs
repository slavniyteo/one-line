using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using OneLine;
using RectEx;
using System;

namespace OneLine {
    [CustomPropertyDrawer(typeof(ExpandableAttribute))]
    public class ExpandableAttributeDrawer : PropertyDrawer {

        private static readonly float FOLDOUT_WIDTH = 14;

        private GUIStyle foldoutStyle;
    
        public override void OnGUI (Rect rect, SerializedProperty property, GUIContent label) {
            rect = EditorGUI.PrefixLabel(rect, label);

            var rects = rect.CutFromLeft(FOLDOUT_WIDTH, 0);
    
            EditorGUI.ObjectField(rects[1], property, GUIContent.none);
    
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

            private static readonly float MIN_WINDOW_WIDTH = 400;
            private float max_window_height;

            private SerializedProperty property;

            private Rect contentRect;
            private Vector2 scrollPosition;
            private Rect windowRect;

            public ExpandedObjectWindow(Rect rect, SerializedProperty property){
                this.property = property;

                this.scrollPosition = Vector2.zero;
                this.windowRect = new Rect(0,0, Math.Max(rect.width, MIN_WINDOW_WIDTH), 100);
                this.contentRect = new Rect(0, 0, rect.width - 20, 0);

                this.max_window_height = Screen.height;
            }

            public override Vector2 GetWindowSize() {
                return windowRect.size;
            }

            public override void OnGUI(Rect rect) {
                EditorGUI.DrawRect(rect, Color.gray);
                DrawExpandedObject(rect, new SerializedObject(property.objectReferenceValue));
            }

            private void DrawExpandedObject(Rect rect, SerializedObject target) {
                if (target == null) return; 

                scrollPosition = GUI.BeginScrollView(windowRect, scrollPosition, contentRect);
                if (contentRect.height > windowRect.height){
                    rect = rect.CutFromRight(10)[0];
                }

                rect = rect.Intend(5).FirstLine();
                DrawChildren(rect, target);

                GUI.EndScrollView();
        
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

                contentRect.height = rect.y + 16;
                windowRect.height = Math.Min(contentRect.height, max_window_height);
            }

            private static void DrawScriptReference(Rect rect, SerializedProperty property){
                EditorGUI.BeginDisabledGroup (true);
                EditorGUI.PropertyField (rect, property, false);
                EditorGUI.EndDisabledGroup ();
            }
        }
    }
}
