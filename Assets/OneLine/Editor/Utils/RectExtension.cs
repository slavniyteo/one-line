using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal static class RectExtension {

        public static Rect Union(this Rect rect, params Rect[] other){
            if (other == null || other.Length == 0){
                return rect;
            }
            else if (other[0] == rect){
                return rect;
            }
            else {
                var xMin = Math.Min(rect.xMin, other.Select(x => x.xMin).Aggregate((x,y) => Math.Min(x,y)));
                var yMin = Math.Min(rect.yMin, other.Select(x => x.yMin).Aggregate((x,y) => Math.Min(x,y)));
                var xMax = Math.Max(rect.xMax, other.Select(x => x.xMax).Aggregate((x,y) => Math.Max(x,y)));
                var yMax = Math.Max(rect.yMax, other.Select(x => x.yMax).Aggregate((x,y) => Math.Max(x,y)));
                return new Rect(
                    x: xMin,
                    y: yMin,
                    width: xMax - xMin,
                    height: yMax - yMin
                );
            }
        }

        public static Rect Expand(this Rect rect, float bounds){
            return new Rect(
                x: rect.x - bounds,
                y: rect.y - bounds,
                width: rect.width + 2*bounds,
                height: rect.height + 2*bounds
            );
        }

        public static Rect ExpandV(this Rect rect, float top, float bottom){
            return new Rect(
                x: rect.x,
                y: rect.y - top,
                width: rect.width,
                height: rect.height + top + bottom
            );
        }

        public static Rect ExpandH(this Rect rect, float left, float right){
            return new Rect(
                x: rect.x - left,
                y: rect.y,
                width: rect.width + left + right,
                height: rect.height
            );
        }

        public static Rect[] CutFromBottom(this Rect rect, float height) {
            var left = new Rect(
                x: rect.x,
                y: rect.y,
                width: rect.width,
                height: rect.height - height
            );
            var right = new Rect(
                x: rect.x,
                y: rect.y + left.height,
                width: rect.width,
                height: height
            );
            return new Rect[]{left, right};
        }
        
        public static Rect[] CutFromTop(this Rect rect, float height) {
            var left = new Rect(
                x: rect.x,
                y: rect.y,
                width: rect.width,
                height: height
            );
            var right = new Rect(
                x: rect.x,
                y: rect.y + height,
                width: rect.width,
                height: rect.height + height
            );
            return new Rect[]{left, right};
        }
        
        public static Rect[] CutFromLeft(this Rect rect, float width) {
            var left = new Rect(
                x: rect.x,
                y: rect.y,
                width: width,
                height: rect.height
            );
            var right = new Rect(
                x: rect.x + width,
                y: rect.y,
                width: rect.width - width,
                height: rect.height
            );
            return new Rect[]{left, right};
        }

        public static Rect[] CutFromRight(this Rect rect, float width) {
            var left = new Rect(
                x: rect.x,
                y: rect.y,
                width: rect.width - width,
                height: rect.height
            );
            var right = new Rect(
                x: rect.x + left.width,
                y: rect.y,
                width: width,
                height: rect.height
            );
            return new Rect[]{left, right};
        }

        public static Rect[] SplitV(this Rect rect, int slices, float space = 5){
            var weights = Enumerable.Repeat(1f, slices).ToArray();
            return SplitV(rect, weights, null, space);
        }

        public static Rect[] SplitV(this Rect rect, IEnumerable<float> weights, IEnumerable<float> fixedWidthes = null, float space = 5){
            return rect.Invert()
                       .Split(weights, fixedWidthes, 5)
                       .Select(Invert)
                       .ToArray();
        }

        private static Rect Invert(this Rect rect){
            return new Rect(
                x: rect.y,
                y: rect.x,
                width: rect.height,
                height: rect.width
            );
        }
        
        public static Rect[] Split(this Rect rect, IEnumerable<float> weights, IEnumerable<float> fixedWidthes = null, float space = 5){
            if (fixedWidthes == null){
                fixedWidthes = weights.Select(x => 0f);
            }
            var cells = weights.Merge(fixedWidthes, (weight, width) => new Cell(weight, width)).Where( cell => cell.HasWidth);

            float weightUnit = GetWeightUnit(rect.width, cells, space);

            var result = new List<Rect>();
            float nextX = rect.x;
            foreach (var cell in cells) {
                result.Add(new Rect(
                               x: nextX,
                               y: rect.y,
                               width: cell.GetWidth(weightUnit),
                               height: rect.height
                           ));

                nextX += cell.HasWidth ? (cell.GetWidth(weightUnit) + space) : 0;
            }

            return result.ToArray();
        }

        private static float GetWeightUnit(float fullWidth, IEnumerable<Cell> cells, float space) {
            float result = 0;
            float weightsSum = cells.Sum(cell => cell.Weight);

            if (weightsSum > 0) {
                float fixedWidth = cells.Sum(cell => cell.FixedWidth);
                float spacesWidth = (cells.Count(cell => cell.HasWidth) - 1) * space;
                result = (fullWidth - fixedWidth - spacesWidth) / weightsSum;
            }

            return result;
        }

        private class Cell {
            public float Weight { get; private set; }
            public float FixedWidth { get; private set; }

            public Cell(float weight, float fixedWidth) {
                this.Weight = weight;
                this.FixedWidth = fixedWidth;

            }

            public bool HasWidth { get { return FixedWidth > 0 || Weight > 0; } }
            public float GetWidth(float weightUnit) {
                return FixedWidth + Weight * weightUnit;
            }
        }

    }
}
