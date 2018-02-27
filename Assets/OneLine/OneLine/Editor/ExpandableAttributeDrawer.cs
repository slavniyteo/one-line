using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using OneLine;
using RectEx;
using System;

namespace OneLine {
    [CustomPropertyDrawer(typeof(ExpandableAttribute))]
    public class ExpandableAttributeDrawer : PropertyDrawer {

            // Use the following area to change the style of the expandable ScriptableObject drawers;
        #region Style Setup
        private enum BackgroundStyles {
            None,
            HelpBox,
            Darken,
            Lighten
        }
    
        /// <summary>
        /// Whether the default editor Script field should be shown.
        /// </summary>
        private static bool SHOW_SCRIPT_FIELD = false;
    
        /// <summary>
        /// The spacing on the inside of the background rect.
        /// </summary>
        private static float INNER_SPACING = 6.0f;
    
        /// <summary>
        /// The spacing on the outside of the background rect.
        /// </summary>
        private static float OUTER_SPACING = 4.0f;
    
        /// <summary>
        /// The style the background uses.
        /// </summary>
        private static BackgroundStyles BACKGROUND_STYLE = BackgroundStyles.HelpBox;
    
        /// <summary>
        /// The colour that is used to darken the background.
        /// </summary>
        private static Color DARKEN_COLOUR = new Color (0.0f, 0.0f, 0.0f, 0.2f);
    
        /// <summary>
        /// The colour that is used to lighten the background.
        /// </summary>
        private static Color LIGHTEN_COLOUR = new Color (1.0f, 1.0f, 1.0f, 0.2f);
        #endregion
    
        // public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
        // {
        //     float totalHeight = 0.0f;
    
        //     totalHeight += EditorGUIUtility.singleLineHeight;
    
        //     if (property.objectReferenceValue == null)
        //         return totalHeight;
    
        //     if (!property.isExpanded)
        //         return totalHeight;
    
        //     SerializedObject targetObject = new SerializedObject (property.objectReferenceValue);
    
        //     if (targetObject == null)
        //         return totalHeight;
        
        //     SerializedProperty field = targetObject.GetIterator ();
    
        //     field.NextVisible (true);
    
        //     if (SHOW_SCRIPT_FIELD)
        //     {
        //         totalHeight += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        //     }
    
        //     while (field.NextVisible (false))
        //     {
        //         totalHeight += EditorGUI.GetPropertyHeight (field, true) + EditorGUIUtility.standardVerticalSpacing;
        //     }
    
        //     totalHeight += INNER_SPACING * 2;
        //     totalHeight += OUTER_SPACING * 2;
    
        //     return totalHeight;
        // }
    
        public override void OnGUI (Rect rect, SerializedProperty property, GUIContent label)
        {
            var rects = rect.CutFromRight(20);
    
            EditorGUI.PropertyField (rects[0], property, label, true);
    
            if (property.objectReferenceValue == null) return;
        
            if (GUI.Button(rects[1], "E")){
                PopupWindow.Show(rect, new ExpandedObjectWindow(rect, property));
            }
    
        }

        public class ExpandedObjectWindow : PopupWindowContent {
            private Rect rect;
            private SerializedProperty property;

            public ExpandedObjectWindow(Rect rect, SerializedProperty property){
                this.rect = rect;
                this.rect.height = 300;
                this.property = property;
            }

            public override Vector2 GetWindowSize() {
                return rect.size;
            }

            public override void OnGUI(Rect rect) {
                DrawExpandedObject(rect, property);
            }

            public override void OnOpen() {
            }

            public override void OnClose() {
            }

            private void DrawExpandedObject(Rect rect, SerializedProperty property) {
                SerializedObject targetObject = new SerializedObject (property.objectReferenceValue);
        
                if (targetObject == null) {
                    Debug.Log("Target object is null");
                    return;
                }

                // First line
                rect = rect.CutFromTop(18)[0];

                int index = 0;
                SerializedProperty field = targetObject.GetIterator ();
                field.NextVisible (true);
        
                if (SHOW_SCRIPT_FIELD) {
                    //Show the disabled script field
                    EditorGUI.BeginDisabledGroup (true);
                    EditorGUI.PropertyField (rect, field, true);
                    EditorGUI.EndDisabledGroup ();
                    index++;

                    rect = rect.MoveDown();
                }
        
                //Replacement for "editor.OnInspectorGUI ();" so we have more control on how we draw the editor
                while (field.NextVisible (false)) {
                    try {
                        rect.height = EditorGUI.GetPropertyHeight(field, false);
                        EditorGUI.PropertyField (rect, field, true);
                        rect.y += rect.height;
                    }
                    catch (StackOverflowException) {
                        field.objectReferenceValue = null;
                        Debug.LogError ("Detected self-nesting cauisng a StackOverflowException, avoid using the same " +
                            "object iside a nested structure.");
                    }
        
                    index++;
                }
        
                targetObject.ApplyModifiedProperties ();
            }

            /// <summary>
            /// Draws the Background
            /// </summary>
            /// <param name="rect">The Rect where the background is drawn.</param>
            private void DrawBackground (Rect rect)
            {
                switch (BACKGROUND_STYLE) {
        
                case BackgroundStyles.HelpBox:
                    EditorGUI.HelpBox (rect, "", MessageType.None);
                    break;
        
                case BackgroundStyles.Darken:
                    EditorGUI.DrawRect (rect, DARKEN_COLOUR);
                    break;
        
                case BackgroundStyles.Lighten:
                    EditorGUI.DrawRect (rect, LIGHTEN_COLOUR);
                    break;
                }
            }
        }
    
    }
}
