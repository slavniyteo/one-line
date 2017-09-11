using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class SeparateDecorator : FieldDecorator {
        private const float width = 5;

        public float GetWeight(SerializedProperty property){
            return 0;
        }
        public float GetFixedWidth(SerializedProperty property){
            return Need(property) ? width : 0;
        }

        private bool Need(SerializedProperty property){
            return property.GetCustomAttribute<SeparatorAttribute>() != null;
        }

        public Rect Draw(Rect[] rects, int index, SerializedProperty property){
            var rect = rects[index];
            if (Need(property)){
                rects = rect.Split(new float[]{0,0,1}, new float[]{2, width, 0}, 0);
                GuiUtil.DrawRect(rects[0], Color.gray);
                rect = rects[2];
            }
            return rect;
        }
    }
}
