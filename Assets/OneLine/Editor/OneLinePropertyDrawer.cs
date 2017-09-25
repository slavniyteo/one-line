using UnityEditor;
using UnityEngine;
using System.Linq;

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

#region Height

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

#endregion

#region OnGUI

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            position = DrawHeaderIfNeed(position, property);
            DrawLine(position, property, label);

            EditorGUI.indentLevel = indentLevel;
        }

        private Rect DrawHeaderIfNeed(Rect position, SerializedProperty property){
            if (! NeedDrawHeader(property)) return position;

            var rects = position.SplitV(2);
            rootDirectoryDrawer.DrawTableHeader(rects[0], property);
            return rects[1];
        }

        private void DrawLine(Rect position, SerializedProperty property, GUIContent label){
            var slices = new Slices();
            rootDirectoryDrawer.AddSlices(property, slices);

            var rects = position.Split(slices.Weights, slices.Widthes, 5);

            slices.ForEachMerged(rects, (slice, rect) =>
                slice.Draw(rect)
            );
        }

#endregion

    }
}
