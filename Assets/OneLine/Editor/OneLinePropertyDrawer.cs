using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using System;
using System.Linq;
using System.Collections.Generic;

namespace OneLine {
    [CustomPropertyDrawer(typeof(OneLineAttribute), true)]
    public class OneLinePropertyDrawer : PropertyDrawer {

        private Drawer simpleDrawer;
        private Drawer fixedArrayDrawer;
        private Drawer dynamicArrayDrawer;
        private Drawer directoryDrawer;
        private RootDirectoryDrawer rootDirectoryDrawer;

        private SlicesCache cache;
        private InspectorUtil inspectorUtil;

        private new OneLineAttribute attribute { get { return base.attribute as OneLineAttribute; } }

        public OneLinePropertyDrawer(){
            simpleDrawer = new SimpleFieldDrawer();
            fixedArrayDrawer = new FixedArrayDrawer(GetDrawer);
            dynamicArrayDrawer = new DynamicArrayDrawer(GetDrawer, InvalidateCache);
            directoryDrawer = new DirectoryDrawer(GetDrawer);
            rootDirectoryDrawer = new RootDirectoryDrawer(GetDrawer);

            inspectorUtil = new InspectorUtil();
            ResetCache();
            Undo.undoRedoPerformed += ResetCache;
        }

        private void OnDestroy(){
            Undo.undoRedoPerformed -= ResetCache;
        }

        private Drawer GetDrawer(SerializedProperty property) {
            if (property.isArray && !(property.propertyType == SerializedPropertyType.String)) {
                if (property.GetCustomAttribute<ArrayLengthAttribute>() == null) {
                    return dynamicArrayDrawer;
                }
                else {
                    return fixedArrayDrawer;
                }
            }
            else if (property.hasVisibleChildren) {
                return directoryDrawer;
            }
            else {
                return simpleDrawer;
            }
        }

        private void ResetCache(){
            if (cache == null || cache.IsDirty){
                cache = new SlicesCache(rootDirectoryDrawer.AddSlices);
            }
        }

        private void InvalidateCache(SerializedProperty property){
            cache.Invalidate(property);
        }

#region Height

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            var lineHeight = 16;
            var headerHeight = NeedDrawHeader(property) ? lineHeight + 5 : 0;

            return lineHeight + headerHeight;
        }

        private bool NeedDrawHeader(SerializedProperty property){
            if (attribute.Header == LineHeader.None){ return false; }

            bool notArray = ! property.IsArrayElement();
            bool firstElement = property.IsArrayElement() && property.IsArrayFirstElement();
            return attribute.Header == LineHeader.Short && (notArray || firstElement);
        }

#endregion

#region OnGUI

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (inspectorUtil.IsOutOfScreen(position)){ return; }
            if (inspectorUtil.IsWindowWidthChanged()){ ResetCache(); }

            Profiler.BeginSample("OneLine.OnGUI");
            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            position = DrawHeaderIfNeed(position, property);
            DrawLine(position, property, (slice,rect) => slice.Draw(rect));

            EditorGUI.indentLevel = indentLevel;
            Profiler.EndSample();
        }

        private Rect DrawHeaderIfNeed(Rect position, SerializedProperty property){
            if (! NeedDrawHeader(property)) return position;

            var rects = position.SplitV(2);
            DrawLine(rects[0], property, (slice, rect) => slice.DrawHeader(rect));
            
            return rects[1];
        }

        private void DrawLine(Rect position, SerializedProperty property, Action<Slice, Rect> draw){
            var slices = cache[property];
            var rects = position.Split(slices.Weights, slices.Widthes, 5);

            int rectIndex = 0;
            foreach (var slice in slices){
                if (slice is MetaSlice){
                    DrawMetaSlice(slice as MetaSlice, rects, rectIndex, draw);
                }
                else {
                    draw(slice, rects[rectIndex]);
                    rectIndex++;
                }
            }
        }

        private void DrawMetaSlice(MetaSlice slice, Rect[] rects, int currentRect, Action<Slice, Rect> draw){
            var from = rects[currentRect - slice.Before];
            var to = rects[currentRect + slice.After - 1];
            var rect = from.Union(to);

            draw(slice, rect);
        }

#endregion

    }
}
