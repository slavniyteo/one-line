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
    private ThreeFields someFloats;
    [SerializeField, OneLine, Range(0, 1)]
    private float rootRange;
    [SerializeField, OneLine]
    private CustomField customDrawer;

    [Serializable]
    public class ThreeFields {
        [SerializeField, Range(-50, 50)]
        private int first;
        [SerializeField]
        private float second;
        [SerializeField]
        private CustomField third;
    }

    [Serializable]
    public class CustomField {
        [SerializeField]
        private int first;
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(CustomField))]
    public class CustomFieldDrawer : PropertyDrawer {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
            rect = EditorGUI.PrefixLabel(rect, label);

            property = property.FindPropertyRelative("first");

            label = EditorGUI.BeginProperty(rect, label, property);
            property.intValue = EditorGUI.IntSlider(rect, property.intValue, 1000, 2000);
            EditorGUI.EndProperty();
        }
    }
}
#endif
}