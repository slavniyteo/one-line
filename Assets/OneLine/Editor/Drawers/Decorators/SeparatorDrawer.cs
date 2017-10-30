using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class SeparatorDrawer : Drawer {
        private const float width = 8;
        private const float lineWidth = 2;

        public override void AddSlices(SerializedProperty property, Slices slices){
            Slice slice;
            if (property.IsArrayElement()){
                slice = new Slice(0, width, Draw);
            }
            else {
                slice = new Slice(0, width, Draw, Draw);
            }
            slices.Add(slice);
        }


        public void Draw (Rect rect){
            rect.x += (rect.width - lineWidth) / 2;
            rect.width = lineWidth;
            GuiUtil.DrawRect(rect, GuiUtil.GrayColor);
        }

    }
}
