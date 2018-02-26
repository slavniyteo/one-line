using System;
using UnityEngine;
using OneLine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace OneLine.Examples {
[CreateAssetMenu(menuName = "OneLine/CustomPropertyDrawerExample")]
public class CustomPropertyDrawerExample : ScriptableObject {
    [SerializeField, OneLine]
    private DirectDrawer directDrawer;
    [SerializeField, OneLine]
    private AttributeDrawer attributeDrawer;

    #region Direct Custom Drawer

    [Serializable]
    public class DirectDrawer {
        [SerializeField]
        private Parent parent;
        [SerializeField]
        private Child child;
    }

    [Serializable]
    public class Parent {
        [SerializeField]
        private string first;
    }

    [Serializable]
    public class Child : Parent {
        [SerializeField]
        private string second; //Will not be drown
    }

    #endregion

    #region Custom Drawer on the Attribute

    [Serializable]
    public class AttributeDrawer {
        [SerializeField, Range(0, 100)]
        private float pureRange;
        [SerializeField, DrawerAttribute]
        private AttributeExample parent;
        [SerializeField, DrawerAttributeChild]
        private AttributeExample child;
    }

    [Serializable]
    public class AttributeExample {
        [SerializeField]
        private string first;
    }

    public class DrawerAttribute : PropertyAttribute {

    }
    public class DrawerAttributeChild : DrawerAttribute {

    }

    #endregion

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(Parent), true)]
    [CustomPropertyDrawer(typeof(DrawerAttribute), true)]
    public class CustomFieldDrawer : PropertyDrawer {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
            rect = EditorGUI.PrefixLabel(rect, label);

            // property = property.FindPropertyRelative("first");

            label = EditorGUI.BeginProperty(rect, label, property);
            EditorGUI.LabelField(rect, property.displayName + " is drown");
            EditorGUI.EndProperty();
        }
    }
}
#endif
}