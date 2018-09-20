using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneLine.Settings {
    [Serializable]
    public class SettingsLayer : ISettings {
        [SerializeField]
        private TernaryBoolean enabled;
        public TernaryBoolean Enabled { 
            get { return enabled; } 
        }

        [SerializeField]
        private TernaryBoolean drawVerticalSeparator;
        public TernaryBoolean DrawVerticalSeparator {
            get { return drawVerticalSeparator; }
        }

        [SerializeField]
        private TernaryBoolean drawHorizontalSeparator;
        public TernaryBoolean DrawHorizontalSeparator {
            get { return drawHorizontalSeparator; }
        }
    }
}