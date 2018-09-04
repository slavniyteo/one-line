using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace OneLine {
	internal class Drawable {

		private Action<Rect> draw;
		private Action<Rect> drawHeader;

		protected Drawable() {}

		public Drawable(Action<Rect> draw) 
		: this(draw, null) { 

		}

		public Drawable(Action<Rect> draw, Action<Rect> drawHeader) {
			this.draw = draw;
			this.drawHeader = drawHeader;
		}

		public virtual void Draw(Rect rect){
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

	internal class Slice : Drawable {

		public virtual float Weight { get; private set; }
		public virtual float Width { get; private set; }

		private string header;

		protected Slice() : base() {

		}

		public Slice (float weight, float width, Action<Rect> draw)
		: this(weight, width, draw, null){
		}

		public Slice (float weight, float width, Action<Rect> draw, Action<Rect> drawHeader) 
		: base(draw, drawHeader) {
			Weight = weight;
			Width = width;
		}

	}
}