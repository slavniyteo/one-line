using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class RootDirectoryDrawer : DirectoryDrawer {

        private HeaderDrawer headerDrawer;

        public RootDirectoryDrawer(DrawerProvider getDrawer) : base(getDrawer) {
            headerDrawer = new HeaderDrawer();
        }

        public override void AddSlices(SerializedProperty property, Slices slices){
            var hideLabel = property.GetCustomAttribute<HideLabelAttribute>();
            if (hideLabel == null) {
                slices.Add(new Slice(0, EditorGUIUtility.labelWidth - 5, 
                                     rect => IndentWithLabel(rect, property.Copy(), true) ));
            }
            base.AddSlices(property, slices);
        }

        protected override void DrawChild(SerializedProperty parent, SerializedProperty child, Slices slices){
            var count = slices.CountPayload;

            base.DrawChild(parent, child, slices);

            headerDrawer.AddSlices(slices.CountPayload - count, 0, child, slices);
        }

        private static Rect IndentWithLabel(Rect rect, SerializedProperty property, bool drawLabel) {
            var rects = rect.CutFromLeft(EditorGUIUtility.labelWidth);

            if (drawLabel) {
                DrawLabel(rects[0], property);
            }
            return rects[1];
        }

        private static void DrawLabel(Rect rect, SerializedProperty property){
            string tooltip =  property.GetCustomAttribute<TooltipAttribute>()
                                .IfPresent(x => x.tooltip)
                                .OrElse(null);

            var label = new GUIContent(property.displayName, tooltip);
            EditorGUI.LabelField(rect, label);
        }

    }
}
