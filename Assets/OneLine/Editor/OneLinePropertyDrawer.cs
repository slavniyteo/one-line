using UnityEditor;
using UnityEngine;

namespace OneLine {
    [CustomPropertyDrawer(typeof(OneLineAttribute))]
    public class OneLinePropertyDrawer : PropertyDrawer {

        private Drawer simpleDrawer;
        private Drawer fixedArrayDrawer;
        private Drawer dynamicArrayDrawer;
        private Drawer directoryDrawer;
        private RootDirectoryDrawer rootDirectoryDrawer;

        private new OneLineAttribute attribute { get { return base.attribute as OneLineAttribute; } }

        public OneLinePropertyDrawer(){
            simpleDrawer = new SimpleFieldDrawer();
            fixedArrayDrawer = new FixedArrayDrawer(GetDrawer);
            dynamicArrayDrawer = new DynamicArrayDrawer(GetDrawer);
            directoryDrawer = new DirectoryDrawer(GetDrawer);
            rootDirectoryDrawer = new RootDirectoryDrawer(GetDrawer);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            var lineHeight = 16;
            var headerHeight = NeedDrawHeader(property) ? lineHeight + 5 : 0;

            return lineHeight + headerHeight;
        }

        private bool NeedDrawHeader(SerializedProperty property){
            bool notArray = ! property.IsArrayElement();
            bool firstElement = property.IsArrayElement() && property.IsArrayFirstElement();
            return attribute.Header == LineHeader.Short && (notArray || firstElement);
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
            int indentLevel = EditorGUI.indentLevel;

            if (NeedDrawHeader(property)){
                var rects = rect.SplitV(2);
                rootDirectoryDrawer.DrawTableHeader(rects[0], property);
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
