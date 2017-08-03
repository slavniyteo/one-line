using System;
using UnityEngine;

namespace Nihil.OneLine.Examples {
    [Serializable]
    public class HideLabel {
        [Header("Before")]
        [SerializeField]
        private OneField oneFieldHidesLabelBefore;
        [SerializeField]
        private TwoFields twoFieldsHidesLabelBefore;
        [SerializeField]
        private TwoFields[] arrayHidesLabelsBefore;

        [Space, Header("After")]
        [SerializeField, OneLine, HideLabel]
        private OneField oneFieldHidesLabel;
        [SerializeField, OneLine, HideLabel]
        private TwoFields twoFieldsHidesLabel;
        [SerializeField, OneLine, HideLabel]
        private TwoFields[] arrayHidesLabels;

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
    }

}