using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class PaddingDecorator : FieldDecorator {
        private const float width = 4;

        public float GetWeight(SerializedProperty property){
            return 0;
        }
        public float GetFixedWidth(SerializedProperty property){
            return NeedPadding(property) ? width * 2 : 0;
        }

        private bool NeedPadding(SerializedProperty property){
            return property.hasVisibleChildren;
        }

        public Rect Draw(Rect[] rects, int index, SerializedProperty property){
            var rect = rects[index];
            if (NeedPadding(property)){
                var padding = new Vector2(-width,-width);
                if (rects.Length == 1) {
                    padding = new Vector2(0,0);
                }
                else if (index == 0) {
                    padding = new Vector2(0, -width*2);
                } 
                else if (index == rects.Length-1) {
                    padding = new Vector2(-width*2, 0);
                }
                rect = rect.WithBoundsH(padding);
            }
            return rect;
        }
    }
}
