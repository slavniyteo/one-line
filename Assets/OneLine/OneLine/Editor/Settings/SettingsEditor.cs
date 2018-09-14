using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using RectEx;

namespace OneLine.Settings {
	[CustomEditor(typeof(Settings))]
	public class SettingsEditor : Editor {

		private new Settings target { get { return (Settings) base.target;} }
		
		private ReorderableList list;

		private void OnEnable() {
			if (target.Layers == null) {
				target.Layers = new List<SettingsLayer>();
			}
			list = new ReorderableList(target.Layers, typeof(SettingsLayer), true, false, true, true);
			list.drawElementCallback += DrawListElement;
			list.onAddCallback += x => target.Layers.Add(SettingsMenu.CreateSettingsLayer());
		}

		private void DrawListElement(Rect rect, int index, bool active, bool focused) {
			if (index < 0 || index >= target.Layers.Count) {
				return;
			}

			var layer = target.Layers[index];
			if (layer == null) {
				target.Layers.RemoveAt(index);
				return;
			}
			
			var rects = rect.Row(2);
			EditorGUI.LabelField(rects[0], layer.name);
			layer.Blame = EditorGUI.TextField(rects[1], layer.Blame);
		}

		public override void OnInspectorGUI() {
			list.DoLayoutList();

			var rect = EditorGUILayout.GetControlRect();
			var rects = rect.Row(2);
			EditorGUI.LabelField(rects[0], target.name);
			EditorGUI.LabelField(rects[1], target.Blame);
		}

	}
}