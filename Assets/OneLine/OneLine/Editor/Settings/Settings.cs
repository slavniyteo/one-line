using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace OneLine.Settings {
    public class Settings : ScriptableObject, ISettings {
        public DefaultSettingsLayer Defaults { get; private set; }
        public LocalSettingsLayer Local { get; private set; }

        [SerializeField]
        private SettingsLayer layer = new SettingsLayer();
        public SettingsLayer Layer { 
            get { return layer; } 
        }

        private void OnEnable() {
            Defaults = new DefaultSettingsLayer();
            Local = new LocalSettingsLayer();

            ApplyDirectivesInOrderToCurrentSettings();
        }

        public TernaryBoolean Enabled { get { return GetBoolean(x => x.Enabled); } }
        public TernaryBoolean DrawVerticalSeparator { get { return GetBoolean(x => x.DrawVerticalSeparator); } }
        public TernaryBoolean DrawHorizontalSeparator { get { return GetBoolean(x => x.DrawHorizontalSeparator); } }
        public TernaryBoolean Expandable { get { return GetBoolean(x => x.Expandable); } }
        public TernaryBoolean CustomDrawer { get { return GetBoolean(x => x.CustomDrawer); } }

        private TernaryBoolean GetBoolean(Func<ISettings, TernaryBoolean> get) {
                var result = get(Defaults);
                if (get(layer).HasValue) {
                    result = get(layer);
                }
                if (get(Local).HasValue) {
                    result = get(Local);
                }
                return result;
        }

        private static void define(HashSet<string> allDefines, List<string> defines, TernaryBoolean value, string key) {
            allDefines.Add(key);
            if (value.HasValue && ! value.BoolValue) {
                defines.Add(key);
            }
        }

        public void ApplyDirectivesInOrderToCurrentSettings(){
            var allDefines = new HashSet<string>();
            var defines = new List<string>();

            define(allDefines, defines, Enabled, "ONE_LINE_DISABLED");
            define(allDefines, defines, DrawVerticalSeparator, "ONE_LINE_VERTICAL_SEPARATOR_DISABLE");
            define(allDefines, defines, DrawHorizontalSeparator, "ONE_LINE_HORIZONTAL_SEPARATOR_DISABLE");
            define(allDefines, defines, Expandable, "ONE_LINE_EXPANDABLE_DISABLE");
            define(allDefines, defines, CustomDrawer, "ONE_LINE_CUSTOM_DRAWER_DISABLE");

            BuildTargetGroup target = EditorUserBuildSettings.selectedBuildTargetGroup;
            if (target == BuildTargetGroup.Unknown) {
                Debug.LogError("OneLine Settings Error: can not determine current BuildTargetGroup");
                return;
            }

            var currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
            var resultDefines = new List<String>();
            foreach (var define in currentDefines.Split(';')) {
                if (!string.IsNullOrEmpty(define)) {
                    if (!allDefines.Contains(define)){
                        resultDefines.Add(define);
                    }
                }
            }

            resultDefines.AddRange(defines);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(target, string.Join(";", resultDefines.ToArray()));
        }

    }
}