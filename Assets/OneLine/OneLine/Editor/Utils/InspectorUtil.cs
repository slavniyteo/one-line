using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace OneLine {
    // Optimization: ommit drawing properties that lie outside window.
    // Grow up performance on drawing bit (100+ arrays)
    internal class InspectorUtil {

        private const string INSPECTOR_WINDOW_ASSEMBLY_QUALIFIED_NAME =
                                "UnityEditor.InspectorWindow, UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
        private const string INITIALIZATION_ERROR_MESSAGE = @"OneLine can not initialize Inspector Window Utility. 
You may experience some performance issues. 
Please create an issue on https://github.com/slavniyteo/one-line/ and we will repair it.

";

        private bool enabled = true;

        private Type inspectorWindowType;
        private MethodInfo getWindowPositionInfo;
        private FieldInfo scrollPositionInfo;
        private object window;

        public InspectorUtil() {
            try {
                Initialize();
                enabled = true;
            }
            catch (Exception ex){
                enabled = false;
                Debug.LogError(INITIALIZATION_ERROR_MESSAGE + ex.ToString());
            }
        }

        private void Initialize(){
            inspectorWindowType = Type.GetType(INSPECTOR_WINDOW_ASSEMBLY_QUALIFIED_NAME);
            window = inspectorWindowType
                        .GetField("s_CurrentInspectorWindow", 
                                    BindingFlags.Public | BindingFlags.Static)
                        .GetValue(null);
            
            scrollPositionInfo = inspectorWindowType.GetField("m_ScrollPosition");
            getWindowPositionInfo = inspectorWindowType.GetProperty("position", typeof(Rect))
                                                        .GetGetMethod();
        }

        private Vector2 ScrollPosition { get { return (Vector2) scrollPositionInfo.GetValue(window); } }

        private Rect WindowPosition { get { return (Rect) getWindowPositionInfo.Invoke(window, null); } }

        public bool IsOutOfScreen(Rect position){
            if (! enabled) { return false; }

            bool above = (position.y + position.height) < ScrollPosition.y;
            bool below = position.y > (ScrollPosition.y + WindowPosition.height);

            return above || below;
        }

    }
}
