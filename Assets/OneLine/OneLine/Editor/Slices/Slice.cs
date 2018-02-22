using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace OneLine {
	internal class Slice {

		public float Weight { get; private set; }
		public float Width { get; private set; }

		private Action<Rect> draw;
		private Action<Rect> drawHeader;
		private string header;

		public Slice (float weight, float width, Action<Rect> draw)
		: this(weight, width, draw, null){
		}

		public Slice (float weight, float width, Action<Rect> draw, Action<Rect> drawHeader) {
			Weight = weight;
			Width = width;
			this.draw = draw;
			this.drawHeader = drawHeader;
		}

		public void Draw(Rect rect){
			if (draw != null){
				draw(rect);
			}
		}

		public void DrawHeader(Rect rect){
			if (drawHeader != null) {
				drawHeader(rect);
			}
		}

	}
}