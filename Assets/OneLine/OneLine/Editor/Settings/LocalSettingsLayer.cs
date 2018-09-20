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
                    enabled = readBoolean(ENABLED_NAME);
                }
                return enabled; 
            } 
        }

        private const string DRAW_VERTICAL_SEPARATOR_NAME = "ONE_LINE_SETTINGS_DRAW_VERTICAL_SEPARATOR";
        private TernaryBoolean drawVerticalSeparator;
        public TernaryBoolean DrawVerticalSeparator { 
            get { 
                if (drawVerticalSeparator == null) {
                    drawVerticalSeparator = readBoolean(DRAW_VERTICAL_SEPARATOR_NAME);
                }
                return drawVerticalSeparator; 
            } 
        }

        private const string DRAW_HORIZONTAL_SEPARATOR_NAME = "ONE_LINE_SETTINGS_DRAW_HORIZONTAL_SEPARATOR";
        private TernaryBoolean drawHorizontalSeparator;
        public TernaryBoolean DrawHorizontalSeparator { 
            get { 
                if (drawHorizontalSeparator == null) {
                    drawHorizontalSeparator = readBoolean(DRAW_HORIZONTAL_SEPARATOR_NAME);
                }
                return drawHorizontalSeparator; 
            } 
        }

        private TernaryBoolean readBoolean(string key) {
            return new TernaryBoolean((byte) EditorPrefs.GetInt(key, 0));
        }

        public void Save() {
            if (enabled != null) {
                EditorPrefs.SetInt(ENABLED_NAME, enabled.RawValue);
            }
            if (drawVerticalSeparator != null) {
                EditorPrefs.SetInt(DRAW_VERTICAL_SEPARATOR_NAME, drawVerticalSeparator.RawValue);
            }
            if (drawHorizontalSeparator != null) {
                EditorPrefs.SetInt(DRAW_HORIZONTAL_SEPARATOR_NAME, drawHorizontalSeparator.RawValue);
            }
        }
    }
}