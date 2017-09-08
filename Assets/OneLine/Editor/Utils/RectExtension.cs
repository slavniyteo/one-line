using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal static class RectExtension {

        public static Rect WithBounds(this Rect rect, float bounds){
            return new Rect(
                x: rect.x - bounds,
                y: rect.y - bounds,
                width: rect.width + 2*bounds,
                height: rect.height + 2*bounds
            );
        }
        
        public static Rect[] Split(this Rect rect, float[] weights, float[] fixedWidthes, float space = 5){
            var cells = Sequence(weights.Length)
                        .Select(i => new Cell(weights[i], fixedWidthes[i]))
                        .ToArray();

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

        private static IEnumerable<int> Sequence(int length) {
            for (int i = 0; i < length; i++) {
                yield return i;
            }
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
