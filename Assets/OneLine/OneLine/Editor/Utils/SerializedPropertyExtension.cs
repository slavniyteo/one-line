using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;

namespace OneLine {
    internal static class SerializedPropertyExtension {

        public static IEnumerable<SerializedProperty> GetChildren(this SerializedProperty property) {
            if (!property.hasVisibleChildren) {
                yield break;
            }

            var copy = property.Copy();
            int depth = copy.depth;

            copy.Next(true);
            do {
                string lastPath = copy.propertyPath;
                yield return copy.Copy();

                if (copy.propertyPath != lastPath) {
                    var message =
                        string.Format("Property path'd been changed while iteration. Last iteration path: {0}, current path: {1}", lastPath, copy.propertyPath);
                    throw new InvalidOperationException(message);
                }
            }
            while (copy.Next(false) && copy.depth > depth);
        }

        public static int CountChildrenAndMoveNext(this SerializedProperty property){
            var depth = property.depth;
            int result = 0;
            while (property.NextVisible(true) && property.depth > depth){
                result++;
            }
            return result;
        }

        public static bool IsReallyArray(this SerializedProperty property){
            return property.isArray && property.propertyType != SerializedPropertyType.String;
        }

        public static IEnumerable<SerializedProperty> GetArrayElements(this SerializedProperty property) {
            if (!property.IsReallyArray()) {
                string message = string.Format("Property {0} is not array or list", property.displayName);
                throw new InvalidOperationException(message);
            }

            property = property.Copy();

            string path = property.propertyPath;
            int size = property.arraySize;
            for (int i = 0; i < size; i++) {
                if (property.propertyPath != path) {
                    string message = string.Format("Property path {0} is changed during iteration", property.displayName);
                    throw new InvalidOperationException(message);
                }
                yield return property.GetArrayElementAtIndex(i).Copy();
            }
        }

        public static T GetCustomAttribute<T>(this SerializedProperty property) where T : Attribute {
            return GetCustomAttributes<T>(property).FirstOrDefault();
        }

        public static T[] GetCustomAttributes<T>(this SerializedProperty property) where T : Attribute {
            return GetCustomAttributes(property, typeof(T))
                   .Cast<T>()
                   .ToArray();
        }

        public static Attribute[] GetCustomAttributes(this SerializedProperty property, Type attributeType = null) {
            if (attributeType == null) {
                attributeType = typeof(Attribute);
            }

            string[] path = property.propertyPath.Split('.');

            bool failed = false;
            Type type = property.serializedObject.targetObject.GetType();
            FieldInfo field = null;
            for (int i = 0; i < path.Length; i++) {
                field = type.GetField(path[i], BindingFlags.Public 
                                               | BindingFlags.NonPublic
                                               | BindingFlags.Instance);

                if (field != null) {
                    type = field.FieldType;
                }
                else {
                    failed = true;
                }

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

            if (failed) {
                return new Attribute[0];
            }
            else {
                return field.GetCustomAttributes(attributeType, false).Cast<Attribute>().ToArray();
            }
        }

        public static bool IsArrayElement(this SerializedProperty property){
            var path = property.propertyPath;
            return path.Substring(path.Length - 1, 1) == "]" ;
        }

        public static bool IsArrayFirstElement(this SerializedProperty property){
            var path = property.propertyPath;
            return path.Substring(path.Length - 3, 3) == "[0]" ;
        }

    }
}
