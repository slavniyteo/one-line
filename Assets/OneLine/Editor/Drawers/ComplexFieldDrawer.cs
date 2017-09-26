using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal abstract class ComplexFieldDrawer : Drawer {

        private SeparatorDrawer separatorDrawer = new SeparatorDrawer();
        private SpaceDrawer spaceDrawer = new SpaceDrawer();

        internal delegate Drawer DrawerProvider(SerializedProperty property);
        protected DrawerProvider getDrawer;

        public ComplexFieldDrawer(DrawerProvider getDrawer){
            this.getDrawer = getDrawer;
        }

        protected abstract IEnumerable<SerializedProperty> GetChildren(SerializedProperty property);

        #region Weights

        public override void AddSlices(SerializedProperty property, Slices slices){
            var count = slices.CountPayload;
            var highlight = DrawHighlight(property, slices, 0, 0);

            DrawChildren(property, slices);
            
            count = slices.CountPayload - count;
            highlight.IfPresent(it => it.After = count);
            DrawTooltip(property, slices, count, 0);
        }

        private void DrawChildren(SerializedProperty property, Slices slices){
            GetChildren(property)
                .ForEachExceptLast((child) => {
                    DrawChild(property, child, slices);

                    if (NeedDrawSeparator(child)){
                        separatorDrawer.AddSlices(child, slices);
                    }
                }, 
                child => {
                    DrawChild(property, child, slices);
                });
        }

        private bool NeedDrawSeparator(SerializedProperty property){
            property = property.Copy();

            bool isComplex = property.CountChildrenAndMoveNext() > 1;
            bool hasAttribute = property.GetCustomAttribute<SeparatorAttribute>() != null;
            bool nextIsComplex = property.CountChildrenAndMoveNext() > 1;
            
            return hasAttribute || isComplex || nextIsComplex;
        }

        protected virtual void DrawChild(SerializedProperty parent, SerializedProperty child, Slices slices){
            spaceDrawer.AddSlices(child, slices);
            getDrawer(child).AddSlices(child, slices);
        }
        
        #endregion

    }
}
