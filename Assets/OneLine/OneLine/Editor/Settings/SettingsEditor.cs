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
#if ONE_LINE_DEFAULTS_ONLY
            PrintErrorUnusedSettingsFile();
#else
            if (SettingsMenu.LoadSettingsFromResources() != target) {
                PrintErrorUnusedSettingsFile();
                EditorGUI.BeginDisabledGroup(true);
            }

            var height = EditorGUIUtility.singleLineHeight;
            var rect = EditorGUILayout.GetControlRect(false, height);

            //In order to beat EventType.Layout
            rect.height = 16;
            var startRect = rect;

            DrawHeader(rect);
            DrawReadOnlyLayer(rect = rect.MoveDown(), "Defaults", target.Defaults);
            DrawLayer(rect = rect.MoveDown(), "Global settings", target.Layer);
            DrawLayer(rect = rect.MoveDown(), "Local settings", target.Local);
            DrawReadOnlyLayer(rect = rect.MoveDown(20), "Results", target);

            DrawSaveButton(rect = rect.MoveDown(20));
            DrawRemoveButton(rect = rect.MoveDown(20));

            //In order to beat EventType.Layout
            EditorGUILayout.GetControlRect(false, rect.yMax - startRect.yMin);
#endif
        }

        private void PrintErrorUnusedSettingsFile(){
            EditorGUILayout.HelpBox("This settings file is not actually used by OneLine.\nDelete it, please.", MessageType.Error);
        }

        private void DrawHeader(Rect rect) {
            var rects = Row(rect);

            EditorGUI.LabelField(rects[1], "Enabled");
            EditorGUI.LabelField(rects[2], "V Separator");
            EditorGUI.LabelField(rects[3], "H Separator");
            EditorGUI.LabelField(rects[4], "Expandable");
            EditorGUI.LabelField(rects[5], "Custom Drawer");
            EditorGUI.LabelField(rects[6], "Culling");
            EditorGUI.LabelField(rects[7], "Cache");
        }

        private void DrawReadOnlyLayer(Rect rect, string label, ISettings layer) {
            EditorGUI.BeginDisabledGroup(true);
            DrawLayer(rect, label, layer);
            EditorGUI.EndDisabledGroup();
        }

        private void DrawLayer(Rect rect, string label, ISettings layer) {
            var rects = Row(rect);

            EditorGUI.LabelField(rects[0], label);
            Draw(rects[1], layer.Enabled, "Enable OneLine");
            Draw(rects[2], layer.DrawVerticalSeparator, "Draw Vertical Separator");
            Draw(rects[3], layer.DrawHorizontalSeparator, "Draw Horizontal Separator");
            Draw(rects[4], layer.Expandable, "Expand Object references via [Expandable]");
            Draw(rects[5], layer.CustomDrawer, "Draw custom property drawers");
            Draw(rects[6], layer.CullingOptimization, "Use culling optimization");
            Draw(rects[7], layer.CacheOptimization, "Use cache optimization");
        }

        private void Draw(Rect rect, TernaryBoolean value, string tooltip) {
            var content = new GUIContent(value.ToString(), tooltip); 
            if (GUI.Button(rect, content)){
                value.SwitchToNext();
            }
        }

        private Rect[] Row(Rect rect) {
            return rect.Row(
                new float[]{0,   0,  0,  0,  0,  0, 0, 0}, 
                new float[]{100, 50, 50, 50, 50, 50, 50, 50}
            );
        }

        private void DrawSaveButton(Rect rect) {
            if (GUI.Button(rect.CutFromLeft(50)[0], "Save")){
                target.SaveAndApply();
            }
        }

        private void DrawRemoveButton(Rect rect) {
            var rects = rect.CutFromRight(75);

            EditorGUI.LabelField(rects[0], "Remove Settings File And Always Use Default Parameters");
            if (GUI.Button(rects[1], "Remove")){
                SettingsMenu.RemoveSettingsForever(target);
            }
        }

    }
}