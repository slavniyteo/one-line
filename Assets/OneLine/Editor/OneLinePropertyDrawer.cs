using UnityEditor;
using UnityEngine;

namespace OneLine {
    [CustomPropertyDrawer(typeof(OneLineAttribute))]
    public class OneLinePropertyDrawer : PropertyDrawer {

        private const int bounds = 0;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return 16 + bounds * 2;
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
            int indentLevel = EditorGUI.indentLevel;
            rect = new Rect(
                x: rect.x,
                y: rect.y + bounds,
                width: rect.width,
                height: rect.height - bounds * 2
            );
            new RootDirectoryDrawer().Draw(rect, property);
            EditorGUI.indentLevel = indentLevel;
        }
    }
}
