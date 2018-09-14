using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneLine.Settings {
	public class SettingsLayer : ScriptableObject, ISettings {
		[SerializeField]
		private string blame;
		public string Blame { 
			get { return blame; } 
			set { blame = value; }
		}
	}
}