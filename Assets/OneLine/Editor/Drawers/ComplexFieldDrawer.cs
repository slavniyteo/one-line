using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal abstract class ComplexFieldDrawer : Drawer {

        private const float SPACE = 5;
        private Padding padding = new Padding();

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
            float additionalSpace = padding.GetPadding(property);

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
                var childRect = padding.CutPadding(rects, i, child);
                DrawField(childRect, child);
                i++;
            }
        }

        protected abstract void DrawField(Rect rect, SerializedProperty property);

        private class Padding {
            private bool NeedPadding(SerializedProperty property){
                return property.hasVisibleChildren;
            }

            public float GetPadding(SerializedProperty property){
                return NeedPadding(property) ? 10 : 0;
            }

            public Rect CutPadding(Rect[] rects, int index, SerializedProperty property){
                var rect = rects[index];
                if (NeedPadding(property)){
                    var padding = new Vector2(-5,-5);
                    if (rects.Length == 1) {
                        padding = new Vector2(0,0);
                    }
                    else if (index == 0) {
                        padding = new Vector2(0, -10);
                    } 
                    else if (index == rects.Length-1) {
                        padding = new Vector2(-10, 0);
                    }
                    rect = rect.WithBoundsH(padding);
                }
                return rect;
            }
        }
    }
}
