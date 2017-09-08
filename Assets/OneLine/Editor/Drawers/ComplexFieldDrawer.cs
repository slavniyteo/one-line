using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal abstract class ComplexFieldDrawer : Drawer {

        protected Func<SerializedProperty, Drawer> getDrawer;

        protected abstract IEnumerable<SerializedProperty> GetChildren(SerializedProperty property);

        #region Weights

        public override float GetWeight(SerializedProperty property) {
            float multiplier = base.GetWeight(property);

            return GetFieldWeights(property)
                   .Select(x => x * multiplier)
                   .Sum();
        }

        private float[] GetFieldWeights(SerializedProperty property) {
            return GetChildren(property)
                   .Select(x => getDrawer(x).GetWeight(x))
                   .ToArray();
        }

        public override float GetFixedWidth(SerializedProperty property) {
            float width = base.GetFixedWidth(property);

            return Math.Max(width, GetFieldFixedWidthes(property).Sum());
        }

        private float[] GetFieldFixedWidthes(SerializedProperty property) {
            return GetChildren(property)
                   .Select(x => getDrawer(x).GetFixedWidth(x))
                   .ToArray();
        }

        protected Rect[] SplitRects(Rect rect, SerializedProperty property) {
            return rect.Split(GetFieldWeights(property), GetFieldFixedWidthes(property));
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
