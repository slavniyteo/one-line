﻿using System;
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
                        string.Format("Property path'd been changed while iteration. Last iteration path: {1}, current path: {2}", lastPath, copy.propertyPath);
                    throw new InvalidOperationException(message);
                }
            }
            while (copy.Next(false) && copy.depth > depth);
        }

        public static IEnumerable<SerializedProperty> GetArrayElements(this SerializedProperty property) {
            if (!property.isArray || property.propertyType == SerializedPropertyType.String) {
                string message = string.Format("Property {1} is not array or list", property.displayName);
                throw new InvalidOperationException(message);
            }

            property = property.Copy();

            string path = property.propertyPath;
            int size = property.arraySize;
            for (int i = 0; i < size; i++) {
                if (property.propertyPath != path) {
                    string message = string.Format("Property path {1} is changed during iteration", property.displayName);
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
                field = type.GetField(path[i], BindingFlags.Public | BindingFlags.NonPublic
                                      | BindingFlags.DeclaredOnly | BindingFlags.FlattenHierarchy
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

        public static IEnumerable<TResult> Merge<TFirst, TSecond, TResult> (this IEnumerable<TFirst> first,
                                                                            IEnumerable<TSecond> second,
                                                                            Func<TFirst, TSecond, TResult> selector){
            var firstEnumerator = first.GetEnumerator();
            var secondEnumerator = second.GetEnumerator();
            while (firstEnumerator.MoveNext() && secondEnumerator.MoveNext()){
                yield return selector(firstEnumerator.Current, secondEnumerator.Current);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action){
            foreach (var value in enumerable){
                action(value);
            }
        }

        public static void ForEachExceptLast<T> (this IEnumerable<T> enumerable, Action<T> action, Action<T> lastAction = null){
            var enumerator = enumerable.GetEnumerator();
            var has = enumerator.MoveNext();
            while (has){
                var current = enumerator.Current;
                has = enumerator.MoveNext();
                if (has) {
                    action(current);
                }
                else if (lastAction != null) {
                    lastAction(current);
                }
            }
        }

    }
}
