using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal static class SerializedPropertyReflectionExtension {

        private static readonly HashSet<Type> TYPES_WITH_MISSPELLING_PROPERTY_PATH = new HashSet<Type>(){
            typeof(Matrix4x4),
            typeof(Rect),
            typeof(Bounds)
        };

        public static T GetCustomAttribute<T>(this SerializedProperty property) where T : Attribute {
            return GetCustomAttributes<T>(property).FirstOrDefault();
        }

        public static T[] GetCustomAttributes<T>(this SerializedProperty property) where T : Attribute {
            return GetCustomAttributes(property, typeof(T))
                   .Cast<T>()
                   .ToArray();
        }

        public static Attribute[] GetCustomAttributes(this SerializedProperty property, Type attributeType = null) {
            if (property == null) throw new ArgumentNullException();
            if (attributeType == null) attributeType = typeof(Attribute); 

            var fieldInfo = GetFieldInfo(property);
            if (fieldInfo == null) {
                return new Attribute[0];
            }
            else {
                return fieldInfo.GetCustomAttributes(attributeType, false).Cast<Attribute>().ToArray();
            }
        }

        public static Type GetRealType(this SerializedProperty property){
            var fieldInfo = GetFieldInfo(property);

            return fieldInfo == null ? null : fieldInfo.FieldType;
        }

        public static FieldInfo GetFieldInfo(this SerializedProperty property) {
            string[] path = property.propertyPath.Split('.');

            Type type = property.serializedObject.targetObject.GetType();
            FieldInfo field = null;
            for (int i = 0; i < path.Length; i++) {
                field = type.GetField(path[i], BindingFlags.Public 
                                               | BindingFlags.NonPublic
                                               | BindingFlags.Instance);

                if (field == null) {
                    NotifyMisspelledPropertyPath(type, property.propertyPath, path[i]);
                    return null;
                }

                type = field.FieldType;

                CrutchIfArray(path, ref i, ref type);
            }

            return field;
        }

        private static void NotifyMisspelledPropertyPath(Type parent, string propertyPath, string fieldName){
            if (!TYPES_WITH_MISSPELLING_PROPERTY_PATH.Contains(parent)){
                var message = "[OneLine] Part `{0}` of property path `{1}` doesn't match field definitions of type `{2}`";
                Debug.LogWarning(String.Format(message, fieldName, propertyPath, parent.FullName));

                TYPES_WITH_MISSPELLING_PROPERTY_PATH.Add(parent);
            }
        }

        private static void CrutchIfArray(string[] path, ref int i, ref Type type){
            int next = i + 1;
            if (next < path.Length && path[next] == "Array") {
                i += 2;
                if (type.IsArray) {
                    type = type.GetElementType();
                }
                else {
                    type = type.GetGenericArguments()[0];
                }
            }
        }

    }
}
