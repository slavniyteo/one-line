using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class SeparatorDrawer : Drawer {
        private const float width = 2;

        public override void AddSlices(SerializedProperty property, Slices slices){
            property = property.Copy();
            if (Count(property) > 1 || Count(property) > 1){
                slices.Add(new Slice(0, width, Draw));
            }
            
        }

        private int Count(SerializedProperty property){
            var depth = property.depth;
            int result = 0;
            while (property.NextVisible(true) && property.depth > depth){
                result++;
            }
            return result;
        }

        public void Draw (Rect rect){
            GuiUtil.DrawRect(rect, Color.gray);
        }

    }
}
