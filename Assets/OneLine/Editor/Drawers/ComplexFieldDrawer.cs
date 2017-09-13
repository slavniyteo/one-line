using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal abstract class ComplexFieldDrawer : Drawer {

        private const float SPACE = 5;
        private FieldDecorator[] fieldDecorators = new FieldDecorator[] {
            new SeparateDecorator()
            // new PaddingDecorator()
        };

        internal delegate Drawer DrawerProvider(SerializedProperty property);
        protected DrawerProvider getDrawer;

        public ComplexFieldDrawer(DrawerProvider getDrawer){
            this.getDrawer = getDrawer;
        }

        protected abstract IEnumerable<SerializedProperty> GetChildren(SerializedProperty property);

        #region Weights

        public override float GetWeight(SerializedProperty property) {
            float multiplier = base.GetWeight(property);
            float children = GetWeights(property).Sum(x => x * multiplier);
            float decorators = fieldDecorators .Sum(x => x.GetWeight(property));

            return children + decorators;
        }

        protected virtual float[] GetWeights(SerializedProperty property) {
            return GetChildren(property)
                   .Select(x => getDrawer(x).GetWeight(x))
                   .ToArray();
        }

        public override float GetFixedWidth(SerializedProperty property) {
            float width = base.GetFixedWidth(property);
            float children = GetFixedWidthes(property).Sum(x => x + SPACE) - SPACE;
            float decorators = fieldDecorators .Sum(x => x.GetFixedWidth(property));

            return Math.Max(width, children) + decorators;
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
                foreach (var decorator in fieldDecorators){
                    rects[i] = decorator.Draw(rects, i, child);
                }
                DrawField(rects[i], child);
                i++;
            }
        }

        protected abstract void DrawField(Rect rect, SerializedProperty property);

    }
}
