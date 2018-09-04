using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace OneLine {
	internal interface Slices : IEnumerable<Slice> {
		float[] Weights { get; }
		float[] Widthes { get; }
		int CountPayload { get; }

		void Add(Slice slice);
	}

	internal class SlicesImpl : Slice, Slices, IEnumerable<Slice> {

		private List<Slice> slices = new List<Slice>();

		public override float Weight { get { return Weights.Sum(); } }
		public override float Width { get { return Widthes.Sum(); } }
		
		public float[] Weights { get { return this.Select(x => x.Weight).ToArray(); } }

		public float[] Widthes { get { return this.Select(x => x.Width).ToArray(); } }

		public int CountPayload { get { return this.Where(x => ! (x is MetaSlice)).Sum(x => {
			if (x is Slices) {
				return (x as Slices).CountPayload;
			}
			else return 1;
		}); } }

		public SlicesImpl(){

		}

		public void Add(Slice slice){
			slices.Add(slice);
		}

		public IEnumerator<Slice> GetEnumerator(){
			foreach (var slice in slices) {
				if (slice is Slices) {
					var children = slice as Slices;
					foreach (var child in children) {
						yield return child;
					}
				}
				else {
					yield return slice;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator(){
			return GetEnumerator();
		}

	}
}