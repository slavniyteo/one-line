using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace OneLine.Settings {
    public class LocalSettingsLayer : ISettings {

        private Dictionary<string, TernaryBoolean> booleans = new Dictionary<string, TernaryBoolean>();

        private const string ENABLED_NAME = "ONE_LINE_SETTINGS_ENABLED";
        private const string DRAW_VERTICAL_SEPARATOR_NAME = "ONE_LINE_SETTINGS_DRAW_VERTICAL_SEPARATOR";
        private const string DRAW_HORIZONTAL_SEPARATOR_NAME = "ONE_LINE_SETTINGS_DRAW_HORIZONTAL_SEPARATOR";
        private const string EXPANDABLE_NAME = "ONE_LINE_SETTINGS_EXPANDABLE";
        private const string CUSTOM_DRAWER_NAME = "ONE_LINE_SETTINGS_CUSTOM_DRAWER";

        public TernaryBoolean Enabled { get { return getBool(ENABLED_NAME); } }
        public TernaryBoolean DrawVerticalSeparator { get { return getBool(DRAW_VERTICAL_SEPARATOR_NAME); } }
        public TernaryBoolean DrawHorizontalSeparator { get { return getBool(DRAW_HORIZONTAL_SEPARATOR_NAME); } }
        public TernaryBoolean Expandable { get { return getBool(EXPANDABLE_NAME); } }
        public TernaryBoolean CustomDrawer { get { return getBool(CUSTOM_DRAWER_NAME); } }

        private TernaryBoolean getBool(string key) {
            TernaryBoolean result = null;
            if (!booleans.TryGetValue(key, out result)) {
                result = new TernaryBoolean((byte) EditorPrefs.GetInt(key, 0));
                booleans[key] = result;
            }
            return result;
        }

        public void Save() {
            foreach (var key in booleans.Keys) {
                Save(key, booleans[key]);
            }
        }

        private void Save(string key, TernaryBoolean value) {
            if (value != null) {
                EditorPrefs.SetInt(key, value.RawValue);
            }
        }
    }
}