using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace OneLine.Settings {
    public enum Boolean {
        None,
        True,
        False
    }

    public interface ISettings {
        Boolean Enabled { get; }
    }

    public class Settings : ScriptableObject, ISettings {
        public DefaultSettingsLayer Defaults { get; private set; }

        [SerializeField]
        private List<SettingsLayer> layers;

        public List<SettingsLayer> Layers {
            get {return layers;}
            set {layers = value;}
        }

        private void OnEnable() {
            Defaults = new DefaultSettingsLayer();

            ApplyDirectivesInOrderToCurrentSettings();
        }

        public Boolean Enabled {
            get {
                var result = layers.Select( x => x.Enabled).Where(x => x != Boolean.None).LastOrDefault();
                if (result == Boolean.None) {
                    result = Defaults.Enabled;
                }
                return result;
            }
        }

        public void ApplyDirectivesInOrderToCurrentSettings(){
#if UNITY_EDITOR
            var allDefines = new HashSet<string>();
            var defines = new List<string>();

            allDefines.Add("ONE_LINE_DISABLED");
            if (! (Enabled == Boolean.True)) {
                defines.Add("ONE_LINE_DISABLED");
            }

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
           

#endif
        }

    }
}