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

        private static readonly Regex REGEXP_ARRAY_INDEX = new Regex(".data\\[\\d+\\]");

        private readonly Dictionary<string, PropertyDrawer> drawers = new Dictionary<string, PropertyDrawer>();
        private static IEnumerable<TypeForDrawing> customDrawers;

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
            
            TypeForDrawing typeDrawer = null;
            Attribute drawerAttribute = null;

            var attributes = property.GetCustomAttributes<PropertyAttribute>();

            foreach (var td in customDrawers){
                foreach (var attribute in attributes){
                    if (td.IsMatch(attribute.GetType())) {
                        typeDrawer = td;
                        drawerAttribute = attribute;
                        break;
                    }
                }
            }

            if (typeDrawer == null) {
                typeDrawer = customDrawers.FirstOrDefault(x => x.IsMatch(propertyType));
            }

            if (typeDrawer == null) return null;

            var drawer = Activator.CreateInstance(typeDrawer.DrawerType) as PropertyDrawer;
            drawer.SetAttribute(drawerAttribute);
            return drawer;
        }

        private static IEnumerable<TypeForDrawing> findAllCustomDrawers(){
            return from assembly in AppDomain.CurrentDomain.GetAssemblies()
                          from type in assembly.GetTypes()
                          where type.IsSubclassOf(typeof(PropertyDrawer))
                          where type != typeof(OneLinePropertyDrawer) && ! type.IsSubclassOf(typeof(OneLinePropertyDrawer))
                          from attribute in type.GetCustomAttributes(typeof(CustomPropertyDrawer), true)
                          select new TypeForDrawing(attribute as CustomPropertyDrawer, type);
        }

        public class TypeForDrawing {
            private Type Type {get; set;}
            private bool UseForChildren {get; set;}
            public Type DrawerType {get; private set;}

            public TypeForDrawing(CustomPropertyDrawer attribute, Type drawerType) {
                Type = attribute.GetTargetType();
                UseForChildren = attribute.IsForChildren();

                DrawerType = drawerType;
            }

            public bool IsMatch(Type target){
                if (target == null) return false;

                if (UseForChildren){
                    return Type == target || target.IsSubclassOf(Type);
                }
                else {
                    return Type == target;
                }
            }
        }

    }
}
