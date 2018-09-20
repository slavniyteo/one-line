using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneLine.Settings {
    public class DefaultSettingsLayer : ISettings {
        public Boolean Enabled { 
            get { return Boolean.True; } 
        }
    }
}