using System;
using UnityEngine;

namespace Nihil.OneLine.Examples {
    [Serializable]
    public class Weights {
        [Header("Before")]
        [SerializeField]
        private Weights1 differentWeights1Before;
        [SerializeField]
        private Weights2 differentWeights2Before;
        [SerializeField]
        private Weights2[] arrayWithDifferentWeightsBefore;

        [Space, Header("After")]
        [SerializeField, OneLine]
        private Weights1 differentWeights1;
        [SerializeField, OneLine]
        private Weights2 differentWeights2;
        [SerializeField, OneLine]
        private Weights2[] arrayWithDifferentWeights;

        [Serializable]
        public class Weights1 {
            [SerializeField, Weight(3)]
            private int first;
            [SerializeField, Weight(2)]
            private int second;
            [SerializeField, Weight(1)]
            private int third;
        }
        [Serializable]
        public class Weights2 {
            [SerializeField, Weight(5)]
            private int first;
            [SerializeField, Weight(1)]
            private int second;
        }
    }
}