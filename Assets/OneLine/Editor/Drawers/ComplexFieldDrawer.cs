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

        public override void AddSlices(SerializedProperty property, Slices slices){
            var count = slices.CountPayload;
            var highlight = DrawHighlight(property, slices, slices.CountPayload - count, 0);

            DrawChildren(property, slices);
            
            highlight.IfPresent(it => it.After = slices.CountPayload - count);
        }

        private void DrawChildren(SerializedProperty property, Slices slices){
            GetChildren(property)
                .ForEachExceptLast((child) => {
                    DrawChild(property, child, slices);
                    separatorDrawer.AddSlices(child, slices);
                }, 
                child => {
                    DrawChild(property, child, slices);
                });
        }

        protected virtual void DrawChild(SerializedProperty parent, SerializedProperty child, Slices slices){
            getDrawer(child).AddSlices(child, slices);
        }
        
        #endregion

    }
}
