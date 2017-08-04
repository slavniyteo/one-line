using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneLine.Examples {
    [CreateAssetMenu]
    public class Example : ScriptableObject {

        [SerializeField]
        private OneLine oneLine;
        [SerializeField]
        private Weights weights;
        [SerializeField]
        private Width width;
        [SerializeField]
        private HideLabel hideLabel;
        [SerializeField]
        private Highlight highlight;
        [SerializeField]
        private Arrays arrays;

    }
}
