using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OneLine.Settings {
    public interface ISettings {
        string Blame { get; }
    }

    public class Settings : ScriptableObject, ISettings {
        [SerializeField]
        private List<SettingsLayer> layers;

        public List<SettingsLayer> Layers {
            get {return layers;}
            set {layers = value;}
        }

        public string Blame {
            get {
                return layers.Select( x => x.Blame).Where(x => !string.IsNullOrEmpty(x)).LastOrDefault();
            }
        }

    }
}