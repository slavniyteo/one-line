using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using RectEx.Internal;

namespace RectEx {
	public static class ColumnExtensions {

        public static Rect[] Column(this Rect rect, int count, float space = 2){
            rect = rect.Invert();
            var result = rect.Row(count, space);
            return result.Select(x => x.Invert()).ToArray();
        }

		public static Rect[] Column(this Rect rect, float[] weights, float space = 2){
			return Column(rect, weights, null, space);
		}

		public static Rect[] Column(this Rect rect, float[] weights, float[] widthes, float space = 2) {
            rect = rect.Invert();
            var result = rect.Row(weights, widthes, space);
            return result.Select(x => x.Invert()).ToArray();
        }

	}
}