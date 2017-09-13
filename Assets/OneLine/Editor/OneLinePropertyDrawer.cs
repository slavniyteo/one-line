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
            var lineHeight = 16 + bounds * 2;

            return lineHeight + (property.IsArrayElement() ? lineHeight + 5 : 0);
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
            int indentLevel = EditorGUI.indentLevel;
            rect = new Rect(
                x: rect.x,
                y: rect.y + bounds,
                width: rect.width,
                height: rect.height - bounds * 2
            );

            if (property.IsArrayElement()){
                var rects = rect.SplitV(new float[]{1,1}, new float[]{0,0}, 5);
                EditorGUI.LabelField(rects[0],"Array header is here");
                rect = rects[1];
            }
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
