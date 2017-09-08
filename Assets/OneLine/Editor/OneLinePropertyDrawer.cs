using UnityEditor;
using UnityEngine;

namespace OneLine {
    [CustomPropertyDrawer(typeof(OneLineAttribute))]
    public class OneLinePropertyDrawer : PropertyDrawer {

        private const int bounds = 0;

        private Drawer simpleDrawer;
        private Drawer fixedArrayDrawer;
        private Drawer dynamicArrayDrawer;
        private Drawer directoryDrawer;
        private Drawer rootDirectoryDrawer;

        public OneLinePropertyDrawer(){
            simpleDrawer = new SimpleFieldDrawer();
            fixedArrayDrawer = new FixedArrayDrawer(GetDrawer);
            dynamicArrayDrawer = new DynamicArrayDrawer(GetDrawer);
            directoryDrawer = new DirectoryDrawer(GetDrawer);
            rootDirectoryDrawer = new RootDirectoryDrawer(GetDrawer);
        }

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
            rootDirectoryDrawer.Draw(rect, property);
            EditorGUI.indentLevel = indentLevel;
        }

        private Drawer GetDrawer(SerializedProperty property) {
            if (property.isArray && !(property.propertyType == SerializedPropertyType.String)) {
                if (property.GetCustomAttribute<ArrayLengthAttribute>() == null) {
                    return dynamicArrayDrawer;
                }
                else {
                    return fixedArrayDrawer;
                }
            }
            else if (property.hasVisibleChildren) {
                return directoryDrawer;
            }
            else {
                return simpleDrawer;
            }
        }


    }
}
