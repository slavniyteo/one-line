using System;
using System.Collections.Generic;
using UnityEngine;

namespace OneLine.Examples {
[CreateAssetMenu]
public class Benchmark : ScriptableObject {

    [OneLine, HideLabel]
    public PureClasses[] pure;

    [Serializable]
    public class PureClasses {
        public int integerField;
        public long longField;
        public float floatField;
        public double doubleField;
        public bool booleanField;
        public string stringField;
    }
}
}