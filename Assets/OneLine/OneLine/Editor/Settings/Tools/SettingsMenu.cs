using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace OneLine.Settings {
    [InitializeOnLoad]
    public class SettingsMenu {

        private const string PATH = "Assets/Editor/Resources/OneLine";
        private const string SETTINGS_FILE_NAME = "OneLineSettings";
        private const string SETTINGS_FILE_NAME_WITH_EXTENSION = SETTINGS_FILE_NAME + ".asset";
        private const string SETTINGS_RESOURCES_PATH = "OneLine/" + SETTINGS_FILE_NAME;

        static SettingsMenu() {
            LoadSettings();
        }

        [MenuItem(itemName: "Window/OneLine Settings")]
        public static void OpenSettings(){
            Selection.activeObject = LoadSettings();
        }

        private static Settings LoadSettings() {
            var settings = Resources.Load<Settings>(SETTINGS_RESOURCES_PATH);
            if (settings == null) {
                settings = CreateSettings();
            }
            return settings;
        }

        private static Settings CreateSettings() {
            PrepareResourcesDirectory();

            var result = Settings.CreateInstance<Settings>();
            AssetDatabase.CreateAsset(result, PATH + "/" + SETTINGS_FILE_NAME_WITH_EXTENSION);
            AssetDatabase.SaveAssets();
            return result;
        }

        private static void PrepareResourcesDirectory(){
            foreach (string directory in GetPathElements(PATH)) {
                if (!AssetDatabase.IsValidFolder(directory)) {
                    AssetDatabase.CreateFolder(Path.GetDirectoryName(directory), Path.GetFileName(directory));
                }
            }
        }

        private static IEnumerable<string> GetPathElements(string path) {
            var result = "";
            foreach (var part in path.Split('/')) {
                if (part.Length > 0) {
                    result = Path.Combine(result, part);
                    yield return result;
                }
            }
        }


        public static void RemoveSettingsForever() {
            var path  = PATH + "/" + SETTINGS_FILE_NAME_WITH_EXTENSION;
            AssetDatabase.DeleteAsset(path);

            ApplyDirectivesInOrderToCurrentSettings(new DefaultSettingsLayer());
        }

        public static void ApplyDirectivesInOrderToCurrentSettings(ISettings settings){
            var allDefines = new HashSet<string>();
            var defines = new List<string>();

            define(allDefines, defines, settings.Enabled, "ONE_LINE_DISABLED");
            define(allDefines, defines, settings.DrawVerticalSeparator, "ONE_LINE_VERTICAL_SEPARATOR_DISABLE");
            define(allDefines, defines, settings.DrawHorizontalSeparator, "ONE_LINE_HORIZONTAL_SEPARATOR_DISABLE");
            define(allDefines, defines, settings.Expandable, "ONE_LINE_EXPANDABLE_DISABLE");
            define(allDefines, defines, settings.CustomDrawer, "ONE_LINE_CUSTOM_DRAWER_DISABLE");

            AddDefinesForCurrentBuildTarget(allDefines, defines);
        }

        private static void define(HashSet<string> allDefines, List<string> defines, TernaryBoolean value, string key) {
            allDefines.Add(key);
            if (value.HasValue && ! value.BoolValue) {
                defines.Add(key);
            }
        }

        public static void AddDefinesForCurrentBuildTarget(IEnumerable<string> allDefines, IEnumerable<string> defines) {
            BuildTargetGroup target = EditorUserBuildSettings.selectedBuildTargetGroup;
            if (target == BuildTargetGroup.Unknown) {
                Debug.LogError("OneLine Settings Error: can not determine current BuildTargetGroup");
                return;
            }

            var currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
            var resultDefines = new List<string>();
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