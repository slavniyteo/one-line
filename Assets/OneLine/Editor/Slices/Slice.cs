using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace OneLine {
	internal class Slice {

		public float Weight { get; private set; }
		public float Width { get; private set; }

		private Action<Rect> draw;

		public Slice (float weight, float width, Action<Rect> draw) {
			Weight = weight;
			Width = width;
			this.draw = draw;
		}

		public void Draw(Rect rect){
			draw(rect);
		}

	}
}