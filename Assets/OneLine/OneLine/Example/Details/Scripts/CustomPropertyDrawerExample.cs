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

    [Separator("Without children")]
    [SerializeField, OneLine]
    private DirectWithoutChildren directWithoutChildren;
    [SerializeField, OneLine]
    private AttributeWithoutChildren attributeWithoutChildren;

    #region Direct Custom Drawer

    [Serializable]
    public class DirectDrawer {
        [SerializeField]
        private Parent parent;
        [SerializeField]
        private Child child;

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
    }

    #endregion

    #region Custom Drawer on the Attribute

    [Serializable]
    public class AttributeDrawer {
        [SerializeField, Range(0, 100)]
        private float pureRange;
        [SerializeField, Parent]
        private AttributeExample parent;
        [SerializeField, Child]
        private AttributeExample child;

        [Serializable]
        public class AttributeExample {
            [SerializeField]
            private string first;
        }

        public class Parent : PropertyAttribute {

        }

        public class Child : Parent {

        }
    }

    #endregion

    #region Do not drawer children

    [Serializable]
    public class DirectWithoutChildren {
        [SerializeField]
        private Parent parent;
        [SerializeField]
        private Child child;

        [Serializable]
        public class Parent {
            [SerializeField]
            private string first;
        }

        [Serializable]
        public class Child : Parent {
            [SerializeField]
            private string second; 
        }
    }

    [Serializable]
    public class AttributeWithoutChildren {
        [SerializeField, Range(0, 100)]
        private float pureRange;
        [SerializeField, Parent]
        private AttributeExample parent;
        [SerializeField, Child]
        private AttributeExample child;

        [Serializable]
        public class AttributeExample {
            [SerializeField]
            private string first;
        }

        public class Parent : PropertyAttribute {

        }

        public class Child : Parent {

        }
    }

    #endregion

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(DirectDrawer.Parent), true)]
    [CustomPropertyDrawer(typeof(AttributeDrawer.Parent), true)]
    [CustomPropertyDrawer(typeof(DirectWithoutChildren.Parent), false)]
    [CustomPropertyDrawer(typeof(AttributeWithoutChildren.Parent), false)]
    public class CustomFieldDrawer : PropertyDrawer {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
            rect = EditorGUI.PrefixLabel(rect, label);

            EditorGUI.LabelField(rect, property.displayName + " is drown");
        }
    }
}
#endif
}