using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class HeaderDrawer : Drawer {

        private GUIStyle tableHeaderStyle;

        public HeaderDrawer() {
            tableHeaderStyle = new GUIStyle(EditorStyles.boldLabel);
            tableHeaderStyle.alignment = TextAnchor.MiddleCenter;
            tableHeaderStyle.normal.textColor = GuiUtil.GrayColor;
        }

        public override void AddSlices(SerializedProperty property, Slices slices){
            AddSlices(0, 1, property, slices);
        }

        public void AddSlices(int before, int after, SerializedProperty property, Slices slices){
            var slice = new MetaSlice(before, after, null, rect => DrawHeader(rect, property.displayName));
            slices.Add(slice);
        }

        public void DrawHeader (Rect rect, string header){
            int fittedWidth = (int) Math.Round(rect.width / 6); //units for one litera
            header = header.Substring(0, Math.Min(header.Length, fittedWidth));

            EditorGUI.LabelField(rect, header, tableHeaderStyle);
        }

    }
}
