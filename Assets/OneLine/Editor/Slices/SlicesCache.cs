using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace OneLine {
	internal class SlicesCache {

		private Dictionary<string, Slices> cache;
		private Action<SerializedProperty, Slices> calculate;

		public bool IsDirty { get; private set; }

		public SlicesCache(Action<SerializedProperty, Slices> calculate){
			cache = new Dictionary<string, Slices>();
			this.calculate = calculate;
		}

		private string lastId = null;

		public Slices this[SerializedProperty property] {
			get {
				lastId = GetId(property);
				if (cache.ContainsKey(lastId)){
					return cache[lastId];
				}
				else {
					var slices = new Slices();
					calculate(property, slices);
					cache.Add(lastId, slices);
					IsDirty = true;
					return slices;
				}
			}
		}

		private string GetId(SerializedProperty property){
			return property.propertyPath;
		}

		public void InvalidateLastUsedId(){
			cache.Remove(lastId);
		}

	}
}