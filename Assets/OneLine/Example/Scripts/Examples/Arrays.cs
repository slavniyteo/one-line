using System;
using UnityEngine;

namespace OneLine.Examples {
    [Serializable]
    public class Arrays {

        [Header("Before")]
        [SerializeField]
        private OneLineArray oneLineArrayBefore;
        [SerializeField]
        private TwoArrays twoArraysInOneLineBefore;
        [SerializeField]
        private ArrayHidesButtons arrayHidesButtonsBefore;
        [SerializeField]
        private ImmutableLengthArray arrayWithImmutableLengthBefore;
        [SerializeField]
        private OneLineArray[] arrayWithArraysBefore;

        [Header("After")]
        [SerializeField, OneLine]
        private OneLineArray oneLineArray;
        [SerializeField, OneLine]
        private TwoArrays twoArraysInOneLine;
        [SerializeField, OneLine]
        private ArrayHidesButtons arrayHidesButtons;
        [SerializeField, OneLine]
        private ImmutableLengthArray arrayWithImmutableLength;
        [SerializeField, OneLine]
        private OneLineArray[] arrayWithArrays;

        [Serializable]
        public class OneLineArray {
            [SerializeField]
            private string[] array;
        }
        [Serializable]
        public class TwoArrays {
            [SerializeField, Highlight(1, 0, 0)]
            private int[] first;
            [SerializeField, Highlight(0, 1, 0), Width(50)]
            private string[] second;
        }
        [Serializable]
        public class ArrayHidesButtons {
            [SerializeField, HideButtons]
            private string[] array;
        }
        [Serializable]
        public class ImmutableLengthArray {
            [SerializeField, ArrayLength(3)]
            private string[] array;
        }
    }
}
