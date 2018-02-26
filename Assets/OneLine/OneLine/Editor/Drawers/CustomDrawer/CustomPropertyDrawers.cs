using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

namespace OneLine {
    internal class CustomPropertyDrawers {

        /*
         * Optimization
         * To avoid creating separate drawers for each array element
         */
        private static readonly Regex REGEXP_ARRAY_INDEX = new Regex(".data\\[\\d+\\]");

        private static CustomDrawerTypesCache types = new CustomDrawerTypesCache();
        private readonly Dictionary<string, PropertyDrawer> drawers = new Dictionary<string, PropertyDrawer>();

        public PropertyDrawer GetCustomPropertyDrawerFor(SerializedProperty property){
            var key = REGEXP_ARRAY_INDEX.Replace(property.propertyPath, "");
            PropertyDrawer result = null;

            if (! drawers.TryGetValue(key, out result)){
                result = CreatePropertyDrawerFor(property);
                drawers[key] = result;
            }

            return result;
        }

        private PropertyDrawer CreatePropertyDrawerFor(SerializedProperty property){
            var propertyType = property.GetRealType();
            if (propertyType == null) return null;
            
            var result = FindAttributeDrawer(property);

            if (result == null) {
                result = FindDirectDrawer(propertyType);
            }

            return result;
        }

        private PropertyDrawer FindAttributeDrawer(SerializedProperty property){
            TypeForDrawing typeDrawer = null;
            Attribute drawerAttribute = null;

            var attributes = property.GetCustomAttributes<PropertyAttribute>();

            foreach (var type in types){
                foreach (var attribute in attributes){
                    if (type.IsMatch(attribute.GetType())) {
                        typeDrawer = type;
                        drawerAttribute = attribute;
                        break;
                    }
                }
            }

            if (typeDrawer == null) return null;

            var drawer = Activator.CreateInstance(typeDrawer.DrawerType) as PropertyDrawer;
            drawer.SetAttribute(drawerAttribute);
            return drawer;
        }

        private PropertyDrawer FindDirectDrawer(Type propertyType){
            var typeDrawer = types.FirstOrDefault(x => x.IsMatch(propertyType));

            if (typeDrawer == null) return null;

            return Activator.CreateInstance(typeDrawer.DrawerType) as PropertyDrawer;
        }
    }
}
