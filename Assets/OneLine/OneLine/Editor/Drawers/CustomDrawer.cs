using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class CustomDrawer : SimpleFieldDrawer {

        public override void Draw(Rect rect, SerializedProperty property) {
            EditorGUI.BeginProperty(rect, GUIContent.none, property);
            DrawProperty(rect, property);
            EditorGUI.EndProperty();
        }

        private void DrawProperty(Rect rect, SerializedProperty property){
            if (customDrawers == null){
                customDrawers = findAllCustomDrawers();
            }

            var isDrown = TryDrawCustomPropertyDrawer(rect, property);

            if (!isDrown) {
                var message = "[OneLine] Can not draw CustomPropertyDrawer for `{0}` at property path `{1}";
                throw new Exception(string.Format(message, property.type, property.propertyPath));
            }
        }

        public bool HasCustomDrawer(SerializedProperty property){
            if (customDrawers == null){
                customDrawers = findAllCustomDrawers();
            }

            var propertyType = property.GetRealType();
            if (propertyType != null && customDrawers.ContainsKey(propertyType)){
                return true;
            }
            else {
                var attributes = property.GetCustomAttributes();
                foreach (var attribute in attributes){
                    if (customDrawers.ContainsKey(attribute.GetType())){
                        return true;
                    }
                }

            }

            return false;
        }

        private bool TryDrawCustomPropertyDrawer(Rect rect, SerializedProperty property){
            var propertyType = property.GetRealType();
            if (propertyType != null && customDrawers.ContainsKey(propertyType)){
                DrawCustomPropertyDrawer(rect, propertyType, property);
                return true;
            }
            else {
                var attributes = property.GetCustomAttributes();
                foreach (var attribute in attributes){
                    if (customDrawers.ContainsKey(attribute.GetType())){
                        DrawCustomPropertyDrawer(rect, attribute.GetType(), property, attribute);
                        return true;
                    }
                }
            }

            return false;
        }

        private void DrawCustomPropertyDrawer(Rect rect, Type targetType, SerializedProperty property, Attribute attribute = null){
            var drawer = Activator.CreateInstance(customDrawers[targetType]) as PropertyDrawer;
            drawer.SetAttribute(attribute);
            drawer.OnGUI(rect, property, GUIContent.none);
        }

        private Dictionary<Type, Type> customDrawers;

        private static Dictionary<Type, Type> findAllCustomDrawers(){
            var result = new Dictionary<Type, Type>();

            var entries = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                          from type in assembly.GetTypes()
                          where type.IsSubclassOf(typeof(PropertyDrawer))
                          where type != typeof(OneLinePropertyDrawer) && ! type.IsSubclassOf(typeof(OneLinePropertyDrawer))
                          from attribute in type.GetCustomAttributes(typeof(CustomPropertyDrawer), true)
                          select new {drawer = type, target = (attribute as CustomPropertyDrawer).GetTargetType()};

            // Debug.Log("Entries count is " + entries.Count());

            foreach (var entry in entries){
                // Debug.Log(String.Format("Type is {0}, Drawer is {1}", entry.target.Name, entry.drawer.Name));
                result[entry.target] = entry.drawer;
            }

            return result;
        }

    }

    public static class CustomPropertyDrawerExtension {
        public static Type GetTargetType(this CustomPropertyDrawer drawer){
            return typeof(CustomPropertyDrawer)
                    .GetField("m_Type", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(drawer) as Type;
        }

        public static bool IsForChildre(this CustomPropertyDrawer drawer){
            return (bool) typeof(CustomPropertyDrawer)
                    .GetField("m_UseForChildren", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(drawer);
        }

        public static void SetAttribute(this PropertyDrawer drawer, Attribute attribute){
            typeof(PropertyDrawer)
                .GetField("m_Attribute", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(drawer, attribute);
        }
    }
}
