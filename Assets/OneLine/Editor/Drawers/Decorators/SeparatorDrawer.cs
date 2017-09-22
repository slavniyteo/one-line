using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class SeparatorDrawer : Drawer {
        private const float width = 2;

        public override void AddSlices(SerializedProperty property, Slices slices){
            slices.Add(new Slice(0, width, Draw));
        }

        public void Draw (Rect rect){
            GuiUtil.DrawRect(rect, Color.gray);
        }

    }
}
