using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace OneLine.Settings {
    public class LocalSettingsLayer : ISettings {
        private const string ENABLED_NAME = "ONE_LINE_SETTINGS_ENABLED";
        private TernaryBoolean enabled;
        public TernaryBoolean Enabled { 
            get { 
                if (enabled == null) {
                    enabled = new TernaryBoolean((byte) EditorPrefs.GetInt(ENABLED_NAME, 0));
                }
                return enabled; 
            } 
        }

        public void Save() {
            if (enabled != null) {
                EditorPrefs.SetInt(ENABLED_NAME, enabled.RawValue);
            }
        }
    }
}