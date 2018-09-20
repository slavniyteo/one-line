using System.IO;
using System.Collections;
using System.Collections.Generic;
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

    }
}