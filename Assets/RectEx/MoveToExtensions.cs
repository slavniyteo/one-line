using System;
using UnityEngine;

namespace RectEx {
    public static class MoveToExtensions {

        public static Rect MoveRight(this Rect rect, float space = 2){
            rect = rect.Abs();
            rect.x += rect.width + space;
            return rect;
        }

        public static Rect MoveLeft(this Rect rect, float space = 2){
            rect = rect.Abs();
            rect.x -= rect.width + space;
            return rect;
        }

        public static Rect MoveUp(this Rect rect, float space = 2){
            rect = rect.Abs();
            rect.y -= rect.height + space;
            return rect;
        }

        public static Rect MoveDown(this Rect rect, float space = 2){
            rect = rect.Abs();
            rect.y += rect.height + space;
            return rect;
        }
    }
}