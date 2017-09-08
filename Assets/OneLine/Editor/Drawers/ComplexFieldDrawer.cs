using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal abstract class ComplexFieldDrawer : Drawer {

        private const float SPACE = 5;
        private Separator separator = new Separator();

        internal delegate Drawer DrawerProvider(SerializedProperty property);
        protected DrawerProvider getDrawer;

        public ComplexFieldDrawer(DrawerProvider getDrawer){
            this.getDrawer = getDrawer;
        }

        protected abstract IEnumerable<SerializedProperty> GetChildren(SerializedProperty property);

        #region Weights

        public override float GetWeight(SerializedProperty property) {
            float multiplier = base.GetWeight(property);

            return GetWeights(property)
                   .Select(x => x * multiplier)
                   .Sum();
        }

        protected virtual float[] GetWeights(SerializedProperty property) {
            return GetChildren(property)
                   .Select(x => getDrawer(x).GetWeight(x))
                   .ToArray();
        }

        public override float GetFixedWidth(SerializedProperty property) {
            float width = base.GetFixedWidth(property);
            float childrenWidth = GetFixedWidthes(property)
                                    .Select(x => x + SPACE)
                                    .Sum() - SPACE;
            float additionalSpace = separator.GetAdditionalSpace(property);

            return Math.Max(width, childrenWidth) + additionalSpace;
        }

        protected virtual float[] GetFixedWidthes(SerializedProperty property) {
            return GetChildren(property)
                   .Select(x => getDrawer(x).GetFixedWidth(x))
                   .ToArray();
        }

        protected Rect[] SplitRects(Rect rect, SerializedProperty property) {
            return rect.Split(GetWeights(property), GetFixedWidthes(property), SPACE);
        }

        #endregion

        public override void Draw(Rect rect, SerializedProperty property) {
            var rects = SplitRects(rect, property);
            int i = 0;
            foreach (var child in GetChildren(property)) {
                var childRect = separator.CutBounds(rects, i, child);
                DrawField(childRect, child);
                i++;
            }
        }

        protected abstract void DrawField(Rect rect, SerializedProperty property);


        private class Separator {
            private bool NeedSeparate(SerializedProperty property){
                return property.hasVisibleChildren;
            }

            public float GetAdditionalSpace(SerializedProperty property){
                return NeedSeparate(property) ? 10 : 0;
            }

            public Rect CutBounds(Rect[] rects, int index, SerializedProperty property){
                var rect = rects[index];
                if (NeedSeparate(property)){
                    var bounds = new Vector2(-5,-5);
                    if (index == 0) {
                            bounds = new Vector2(0, -10);
                    } 
                    else if (index == rects.Length-1) {
                            bounds = new Vector2(-10, 0);
                    }
                    rect = rect.WithBoundsH(bounds);
                }
                return rect;
            }

        }
    }
}
