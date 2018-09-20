using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using RectEx;
using System;

namespace OneLine.Settings {
    [CustomEditor(typeof(Settings))]
    public class SettingsEditor : Editor {

        private new Settings target { get { return (Settings) base.target;} }

        public override void OnInspectorGUI() {
            var height = EditorGUIUtility.singleLineHeight;
            var rect = EditorGUILayout.GetControlRect(false, height);

            DrawHeader(rect);
            DrawReadOnlyLayer(rect = rect.MoveDown(), "Defaults", target.Defaults);
            DrawLayer(rect = rect.MoveDown(), "Global settings", target.Layer);
            DrawLayer(rect = rect.MoveDown(), "Local settings", target.Local);
            DrawReadOnlyLayer(rect = rect.MoveDown(20), "Results", target);

            DrawSaveButton(rect = rect.MoveDown(20));
        }

        private void DrawHeader(Rect rect) {
            var rects = Row(rect);

            EditorGUI.LabelField(rects[1], "Enabled");
            EditorGUI.LabelField(rects[2], "V Separator");
            EditorGUI.LabelField(rects[3], "H Separator");
            EditorGUI.LabelField(rects[4], "Expandable");
            EditorGUI.LabelField(rects[5], "Custom Drawer");
        }

        private void DrawReadOnlyLayer(Rect rect, string label, ISettings layer) {
            EditorGUI.BeginDisabledGroup(true);
            DrawLayer(rect, label, layer);
            EditorGUI.EndDisabledGroup();
        }

        private void DrawLayer(Rect rect, string label, ISettings layer) {
            var rects = Row(rect);

            EditorGUI.LabelField(rects[0], label);
            var content = new GUIContent(layer.Enabled.ToString(), "Enable OneLine"); 
            if (GUI.Button(rects[1], content)){
                layer.Enabled.SwitchToNext();
            }
            content = new GUIContent(layer.DrawVerticalSeparator.ToString(), "Draw Vertical Sepatator"); 
            if (GUI.Button(rects[2], content)){
                layer.DrawVerticalSeparator.SwitchToNext();
            }
            content = new GUIContent(layer.DrawHorizontalSeparator.ToString(), "Draw Horizontal Sepatator"); 
            if (GUI.Button(rects[3], content)){
                layer.DrawHorizontalSeparator.SwitchToNext();
            }
            content = new GUIContent(layer.Expandable.ToString(), "Expand Object references via [Expandable]"); 
            if (GUI.Button(rects[4], content)){
                layer.Expandable.SwitchToNext();
            }
            content = new GUIContent(layer.CustomDrawer.ToString(), "Draw custom property drawers"); 
            if (GUI.Button(rects[5], content)){
                layer.CustomDrawer.SwitchToNext();
            }
        }

        private Rect[] Row(Rect rect) {
            return rect.Row(
                new float[]{0,   0,  0,  0,  0,  0}, 
                new float[]{100, 50, 50, 50, 50, 50}
            );
        }

        private void DrawSaveButton(Rect rect) {
            if (GUI.Button(rect.CutFromLeft(50)[0], "Save")){
                EditorUtility.SetDirty(target);
                target.Local.Save();
                target.ApplyDirectivesInOrderToCurrentSettings();
            }
        }

    }
}