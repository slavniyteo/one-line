using System;
using UnityEngine;

namespace Nihil.OneLine.Examples {
    [Serializable]
    public class OneLine {
        [Header("Before")]
        [SerializeField]
        private OneField oneFieldBefore;
        [SerializeField]
        private TwoFields twoFieldsBefore;
        [SerializeField]
        private ThreeFields threeFieldsBefore;
        [SerializeField]
        private ThreeFields[] arrayWithThreeFieldsBefore;

        [Space, Header("After")]
        [SerializeField, OneLine]
        private OneField oneField;
        [SerializeField, OneLine]
        private TwoFields twoFields;
        [SerializeField, OneLine]
        private ThreeFields threeFields;
        [SerializeField, OneLine]
        private ThreeFields[] arrayWithThreeFields;

        [Serializable]
        public class OneField {
            [SerializeField]
            private string first;
        }
        [Serializable]
        public class TwoFields {
            [SerializeField]
            private string first;
            [SerializeField]
            private string second;
        }
        [Serializable]
        public class ThreeFields {
            [SerializeField]
            private string first;
            [SerializeField]
            private string second;
            [SerializeField]
            private string third;
        }
    }

}