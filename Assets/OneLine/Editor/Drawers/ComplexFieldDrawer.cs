using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal abstract class ComplexFieldDrawer : Drawer {

        private const float SPACE = 5;

        protected Func<SerializedProperty, Drawer> getDrawer;

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

            return Math.Max(width, childrenWidth);
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
                DrawField(rects[i], child);
                i++;
            }
        }

        protected abstract void DrawField(Rect rect, SerializedProperty property);

    }
}
