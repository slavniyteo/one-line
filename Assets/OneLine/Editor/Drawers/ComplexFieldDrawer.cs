using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal abstract class ComplexFieldDrawer : Drawer {

        private const float SPACE = 5;
        private SeparatorDrawer separatorDrawer = new SeparatorDrawer();

        internal delegate Drawer DrawerProvider(SerializedProperty property);
        protected DrawerProvider getDrawer;

        public ComplexFieldDrawer(DrawerProvider getDrawer){
            this.getDrawer = getDrawer;
        }

        protected abstract IEnumerable<SerializedProperty> GetChildren(SerializedProperty property);

        #region Weights

        protected virtual float[] GetWeights(SerializedProperty property) {
            return GetChildren(property)
                   .Select(x => getDrawer(x).GetWeight(x))
                   .ToArray();
        }

        protected virtual float[] GetFixedWidthes(SerializedProperty property) {
            return GetChildren(property)
                   .Select(x => getDrawer(x).GetFixedWidth(x))
                   .ToArray();
        }

        protected Rect[] SplitRects(Rect rect, SerializedProperty property) {
            return rect.Split(GetWeights(property), GetFixedWidthes(property), SPACE);
        }

        public override void AddSlices(SerializedProperty property, Slices slices){
            GetChildren(property)
                .ForEachExceptLast((x) => {
                    getDrawer(x).AddSlices(x, slices);
                    if (x.depth < 2){
                        separatorDrawer.AddSlices(property, slices);
                    }
                }, x => {
                    getDrawer(x).AddSlices(x, slices);
                });
        }

        #endregion

        protected abstract void DrawField(Rect rect, SerializedProperty property);

    }
}
