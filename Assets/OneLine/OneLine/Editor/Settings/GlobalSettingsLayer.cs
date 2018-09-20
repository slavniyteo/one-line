using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneLine.Settings {
    [Serializable]
    public class GlobalSettingsLayer : ISettings {
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

        [SerializeField]
        private TernaryBoolean expandable;
        public TernaryBoolean Expandable {
            get { return expandable; }
        }

        [SerializeField]
        private TernaryBoolean customDrawer;
        public TernaryBoolean CustomDrawer {
            get { return customDrawer; }
        }
    }
}