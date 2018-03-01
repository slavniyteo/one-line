using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using RectEx;

namespace OneLine {
    [CustomPropertyDrawer(typeof(OneLineAttribute), true)]
    public class OneLinePropertyDrawer : PropertyDrawer {

        private Drawer simpleDrawer;
        private Drawer fixedArrayDrawer;
        private Drawer dynamicArrayDrawer;
        private CustomDrawer customPropertyDrawer;
        private ComplexFieldDrawer directoryDrawer;
        private RootDirectoryDrawer rootDirectoryDrawer;

        private SlicesCache cache;
        private InspectorUtil inspectorUtil;
        private ArraysSizeObserver arraysSizeObserver;

        private new OneLineAttribute attribute { get { return base.attribute as OneLineAttribute; } }
        protected virtual LineHeader Header { get { return attribute != null ? attribute.Header : LineHeader.None; } }

        public OneLinePropertyDrawer(){
            simpleDrawer = new SimpleFieldDrawer();
            fixedArrayDrawer = new FixedArrayDrawer(GetDrawer);
            dynamicArrayDrawer = new DynamicArrayDrawer(GetDrawer, InvalidateCache);
            customPropertyDrawer = new CustomDrawer();
            directoryDrawer = new DirectoryDrawer(GetDrawer);
            rootDirectoryDrawer = new RootDirectoryDrawer(GetDrawer);

            inspectorUtil = new InspectorUtil();
            ResetCache();
            Undo.undoRedoPerformed += ResetCache;
            arraysSizeObserver = new ArraysSizeObserver();
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
            else if (customPropertyDrawer.HasCustomDrawer(property)){
                return customPropertyDrawer;
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
            cache.InvalidateLastUsedId();
        }

#region Height

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            var lineHeight = 16;
            var headerHeight = NeedDrawHeader(property) ? lineHeight + 2 : 0;

            return lineHeight + headerHeight;
        }

        private bool NeedDrawHeader(SerializedProperty property){
            if (Header == LineHeader.None){ return false; }

            bool notArray = ! property.IsArrayElement();
            bool firstElement = property.IsArrayElement() && property.IsArrayFirstElement();
            return (notArray || firstElement);
        }

#endregion

#region OnGUI

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (Event.current.type == EventType.Layout){ return; } // In [Expandable] popup it happens
            if (inspectorUtil.IsOutOfScreen(position)){ return; } // Culling

            if (arraysSizeObserver.IsArraySizeChanged(property)){ ResetCache(); }

            rootDirectoryDrawer.RootDepth = property.depth;
            directoryDrawer.RootDepth = property.depth;
            position = rootDirectoryDrawer.DrawPrefixLabel(position, property);

            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            position = DrawHeaderIfNeed(position, property);
            DrawLine(position, property, (slice,rect) => slice.Draw(rect));

            EditorGUI.indentLevel = indentLevel;
        }

        private Rect DrawHeaderIfNeed(Rect position, SerializedProperty property){
            if (! NeedDrawHeader(property)) return position;

            var rects = position.Column(2, 2);
            DrawLine(rects[0], property, (slice, rect) => slice.DrawHeader(rect));
            
            return rects[1];
        }

        private void DrawLine(Rect position, SerializedProperty property, Action<Slice, Rect> draw){
            var slices = cache[property];
            var rects = position.Row(slices.Weights, slices.Widthes, 2);

            int rectIndex = 0;
            foreach (var slice in slices){
                if (slice is MetaSlice){
                    var rect = CalculateMetaSliceRect(slice as MetaSlice, position, rects, rectIndex);
                    draw(slice, rect);
                }
                else {
                    draw(slice, rects[rectIndex]);
                    rectIndex++;
                }
            }
        }

        private Rect CalculateMetaSliceRect(MetaSlice slice, Rect wholeRect, Rect[] rects, int currentRect){
            var from = rects[currentRect - slice.Before];
            var to = rects[currentRect + slice.After - 1];
            var result = from.Union(to);

            if (slice.Expand && rects.Length == currentRect) {
                result.xMax = wholeRect.xMax;
            }

            return result;
        }

#endregion

    }
}
