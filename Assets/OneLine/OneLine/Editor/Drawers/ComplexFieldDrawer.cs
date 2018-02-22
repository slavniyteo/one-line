using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {

    internal delegate Drawer DrawerProvider(SerializedProperty property);

    internal abstract class ComplexFieldDrawer : Drawer {

        private SeparatorDrawer separatorDrawer = new SeparatorDrawer();
        private SpaceDrawer spaceDrawer = new SpaceDrawer();
        private HeaderDrawer headerDrawer = new HeaderDrawer();

        protected DrawerProvider getDrawer;
        public int RootDepth { get; set; }

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
                    DrawChildWithDecorators(property, child, slices, false);

                    if (NeedDrawSeparator(child)){
                        separatorDrawer.AddSlices(child, slices);
                    }
                }, 
                child => DrawChildWithDecorators(property, child, slices, true)
            );
        }

        private void DrawChildWithDecorators(SerializedProperty parent, SerializedProperty child, Slices slices, bool isLast){
            int count = slices.CountPayload;

            spaceDrawer.AddSlices(child, slices);
            DrawChild(parent, child, slices);

            if (NeedDrawHeader(parent, child)){
                headerDrawer.AddSlices(slices.CountPayload - count, 0, child, slices, isLast);
            }
        }

        private bool NeedDrawHeader(SerializedProperty parent, SerializedProperty child){
            bool parentIsRootArray = child.depth == RootDepth + 2 && parent.IsArrayElement();
            bool parentIsRootField = child.depth == RootDepth + 1;
            return parentIsRootArray || parentIsRootField;
        }

        private bool NeedDrawSeparator(SerializedProperty property){
            property = property.Copy();

            bool isArray = property.IsReallyArray();
            bool isComplex = property.CountChildrenAndMoveNext() > 1;

            bool nextHasAttribute = property.GetCustomAttribute<SeparatorAttribute>() != null;
            bool nextIsArray = property.IsReallyArray();
            bool nextIsComplex = property.CountChildrenAndMoveNext() > 1;
            
            return nextHasAttribute || 
                   isComplex || nextIsComplex || 
                   isArray || nextIsArray;
        }

        protected virtual void DrawChild(SerializedProperty parent, SerializedProperty child, Slices slices){
            getDrawer(child).AddSlices(child, slices);
        }
        
        #endregion

    }
}
