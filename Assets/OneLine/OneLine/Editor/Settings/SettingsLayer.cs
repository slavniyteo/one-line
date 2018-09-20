using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneLine.Settings {
    public class SettingsLayer : ScriptableObject, ISettings {
        [SerializeField]
        private Boolean enabled;
        public Boolean Enabled { 
            get { return enabled; } 
            set { enabled = value; }
        }
    }
}