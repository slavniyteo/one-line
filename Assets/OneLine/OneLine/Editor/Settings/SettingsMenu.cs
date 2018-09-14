using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace OneLine.Settings {
	public class SettingsMenu {

		private const string PATH = "Assets/Editor/Resources/OneLine";
		private const string SETTINGS_FILE_NAME = "OneLineSettings";
		private const string SETTINGS_FILE_NAME_WITH_EXTENSION = SETTINGS_FILE_NAME + ".asset";
		private const string SETTINGS_RESOURCES_PATH = "OneLine/" + SETTINGS_FILE_NAME;

		[MenuItem(itemName: "Window/OneLine Settings")]
		public static void OpenSettings(){
			var settings = Resources.Load<Settings>(SETTINGS_RESOURCES_PATH);
			if (settings == null) {
				settings = CreateSettings();
			}
			Selection.activeObject = settings;
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

		public static SettingsLayer CreateSettingsLayer() {
			PrepareResourcesDirectory();

			var result = Settings.CreateInstance<SettingsLayer>();

			int i = 0;
			string path = "";
			while (++i < 100) {
				path = PATH + "/SettingsLayer " + i + ".asset";
				if (AssetDatabase.LoadAssetAtPath<SettingsLayer>(path) == null) {
					break;
				}
			}
			AssetDatabase.CreateAsset(result, path);
			AssetDatabase.SaveAssets();
			return result;
		}


	}
}