using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace OneLine {
	internal class Slices : IEnumerable<Slice> {

		private List<Slice> slices = new List<Slice>();
		
		public float[] Weights { get { return slices.Select(x => x.Weight).ToArray(); } }

		public float[] Widthes { get { return slices.Select(x => x.Width).ToArray(); } }

		public Slice this[int i] { get {return slices[i];} }

		public int CountPayload { get { return slices.Where(x => ! (x is MetaSlice)).Count(); } }

		public void Add(Slice slice){
			slices.Add(slice);
		}

		public void Add(IEnumerable<Slice> slices){
			foreach (var slice in slices){
				Add(slice);
			}
		}

		public IEnumerator<Slice> GetEnumerator(){
			return slices.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator(){
			return GetEnumerator();
		}

	}
}