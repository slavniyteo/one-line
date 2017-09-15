using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class RootDirectoryDrawer : DirectoryDrawer {

        private TableHeader header;

        public RootDirectoryDrawer(DrawerProvider getDrawer) : base(getDrawer) {
            header = new TableHeader(this);
        }

        public void DrawTableHeader(Rect totalRect, SerializedProperty property){
            var indentedRect = IndentWithLabel(totalRect, property, false);

            var indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            header.Draw(indentedRect, property);

            EditorGUI.indentLevel = indentLevel;
        }

        public override void Draw(Rect totalRect, SerializedProperty property) {
            DrawHighlight(totalRect, property);
            var indentedRect = IndentWithLabel(totalRect, property, true);
            EditorGUI.indentLevel = 0;

            EditorGUI.BeginProperty(totalRect, GUIContent.none, property);
            getDrawer(property).Draw(indentedRect, property);
            EditorGUI.EndProperty();
        }

        private static Rect IndentWithLabel(Rect rect, SerializedProperty property, bool drawLabel) {
            var hideLabel = property.GetCustomAttribute<HideLabelAttribute>();
            if (hideLabel == null) {
                var rects = rect.CutFromLeft(EditorGUIUtility.labelWidth);

                if (drawLabel) {
                    DrawLabel(rects[0], property);
                }
                return rects[1];
            }
            else {
                return EditorGUI.IndentedRect(rect);
            }
        }

        private static void DrawLabel(Rect rect, SerializedProperty property){
            string tooltip =  property.GetCustomAttribute<TooltipAttribute>()
                                .IfPresent(x => x.tooltip)
                                .OrElse(null);

            var label = new GUIContent(property.displayName, tooltip);
            EditorGUI.LabelField(rect, label);
        }

        private class TableHeader{
            private static GUIStyle tableHeaderStyle;
            private RootDirectoryDrawer drawer;

            public TableHeader(RootDirectoryDrawer drawer){
                tableHeaderStyle = new GUIStyle(EditorStyles.boldLabel);
                tableHeaderStyle.padding = new RectOffset(7,7,2,2);

                this.drawer = drawer;
            }

            public void Draw(Rect rect ,SerializedProperty property){
                GuiUtil.DrawRect(rect.CutFromTop(2)[0], Color.gray);

                var slices = drawer.GetChildren(property)
                                .Merge(drawer.SplitRects(rect, property), 
                                       (c, r) => new {child = c, rect = r});
                foreach (var slice in slices){
                    GuiUtil.DrawRect(slice.rect.CutFromLeft(2)[0], Color.gray);
                    GuiUtil.DrawRect(slice.rect.CutFromRight(2)[1], Color.gray);
                    EditorGUI.LabelField(slice.rect, slice.child.displayName, tableHeaderStyle);
                }
            }
        }
    }
}
