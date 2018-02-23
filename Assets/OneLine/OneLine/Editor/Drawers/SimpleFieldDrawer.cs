using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class SimpleFieldDrawer : Drawer {

        public float GetWeight(SerializedProperty property){
            switch (property.propertyType){
                case SerializedPropertyType.Boolean: {
                    return 0;
                }
                default: {
                    var weights = property.GetCustomAttributes<WeightAttribute>()
                                        .Select(x => x.Weight)
                                        .ToArray();
                    return weights.Length > 0 ? weights.Sum() : 1;
                }
            }
        }

        public float GetFixedWidth(SerializedProperty property){
            switch (property.propertyType){
                case SerializedPropertyType.Boolean: {
                    return EditorGUIUtility.singleLineHeight - 2;
                }
                default: {
                    return property.GetCustomAttributes<WidthAttribute>()
                                .Select(x => x.Width)
                                .Sum();
                }
            }
        }

        public override void AddSlices(SerializedProperty property, Slices slices){
            DrawHighlight(property, slices, 0, 1);
            var slice = new Slice(GetWeight(property), GetFixedWidth(property), 
                                  rect => Draw(rect, property.Copy()));
            slices.Add(slice);
            DrawTooltip(property, slices, 1, 0);
        }

        public void Draw(Rect rect, SerializedProperty property) {
            EditorGUI.BeginProperty(rect, GUIContent.none, property);
            DrawProperty(rect, property);
            EditorGUI.EndProperty();
        }

        /*
         * WORKAROUND
         * Unity3d `feature`: EditorGUI.PropertyField draws field 
         * with all decorators (like Header, Space, etc) and this behaviour 
         * can not be ommited.
         * see: http://answers.unity3d.com/questions/1394991/how-to-preserve-drawing-decoratordrawer-like-heade.html
         * Headers and Separators (provided by one-line) produces artefacts.
         * We solve this problem with reflection, but we call internal method
         * and this may be dangerous: unity3d developers may change API =(
         */
        private void DrawProperty(Rect rect, SerializedProperty property){
            //EditorGUI.PropertyField(rect, property, GUIContent.none);

            if (customDrawers == null){
                customDrawers = findAllCustomDrawers();
            }

            var isDrown = TryDrawCustomPropertyDrawer(rect, property);

            if (!isDrown) {
                typeof(EditorGUI)
                    .GetMethod("DefaultPropertyField", BindingFlags.NonPublic | BindingFlags.Static)
                    .Invoke(null, new object[]{rect, property, GUIContent.none});
            }
        }

        private bool TryDrawCustomPropertyDrawer(Rect rect, SerializedProperty property){
            var propertyType = property.GetRealType();
            if (customDrawers.ContainsKey(propertyType)){
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
