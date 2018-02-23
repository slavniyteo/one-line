using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

namespace OneLine {
    internal class CustomDrawer : SimpleFieldDrawer {

        private readonly Dictionary<string, PropertyDrawer> drawers = new Dictionary<string, PropertyDrawer>();
        private static Dictionary<Type, Type> customDrawers;

        public bool HasCustomDrawer(SerializedProperty property){
            return GetCustomPropertyDrawerFor(property) != null;
        }

        public override void Draw(Rect rect, SerializedProperty property) {
            DrawProperty(rect, property);
        }

        private void DrawProperty(Rect rect, SerializedProperty property){
            var drawer = GetCustomPropertyDrawerFor(property);
            if (drawer != null) {
                drawer.OnGUI(rect, property, GUIContent.none);
            }
            else {
                var message = "[OneLine] Can not draw CustomPropertyDrawer for `{0}` at property path `{1}";
                throw new Exception(string.Format(message, property.type, property.propertyPath));
            }
        }

        private static readonly Regex REGEXP_ARRAY_INDEX = new Regex(".data\\[\\d+\\]");

        private PropertyDrawer GetCustomPropertyDrawerFor(SerializedProperty property){
            var key = REGEXP_ARRAY_INDEX.Replace(property.propertyPath, "");
            PropertyDrawer result = null;

            if (! drawers.TryGetValue(key, out result)){
                result = CreatePropertyDrawerFor(property);
                drawers[key] = result;
            }

            return result;
        }

        private PropertyDrawer CreatePropertyDrawerFor(SerializedProperty property){
            if (customDrawers == null){
                customDrawers = findAllCustomDrawers();
            }

            var propertyType = property.GetRealType();
            if (propertyType == null) return null;

            Type drawerType = null;
            Attribute drawerAttribute = null;
            if (! customDrawers.TryGetValue(propertyType, out drawerType)) {
                var attributes = property.GetCustomAttributes();
                foreach (var attribute in attributes){
                    if (customDrawers.TryGetValue(attribute.GetType(), out drawerType)){
                        drawerAttribute = attribute;
                        break;
                    }
                }
            }

            if (drawerType == null) return null;

            var drawer = Activator.CreateInstance(drawerType) as PropertyDrawer;
            drawer.SetAttribute(drawerAttribute);
            return drawer;
        }


        private static Dictionary<Type, Type> findAllCustomDrawers(){
            var result = new Dictionary<Type, Type>();

            var entries = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                          from type in assembly.GetTypes()
                          where type.IsSubclassOf(typeof(PropertyDrawer))
                          where type != typeof(OneLinePropertyDrawer) && ! type.IsSubclassOf(typeof(OneLinePropertyDrawer))
                          from attribute in type.GetCustomAttributes(typeof(CustomPropertyDrawer), true)
                          select new {drawer = type, target = (attribute as CustomPropertyDrawer).GetTargetType()};

            foreach (var entry in entries){
                result[entry.target] = entry.drawer;
            }

            return result;
        }

    }
}
