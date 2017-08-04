using System;
using UnityEngine;

namespace OneLine.Examples {
    [Serializable]
    public class Width {

        [Header("Before")]
        [SerializeField]
        private WidthAndWeight fixedWidthBefore;
        [SerializeField]
        private WidthOnly onlyFixedWidthBefore;
        [SerializeField]
        private WidthOnly[] arrayWithOnlyFixedWidthsBefore;

        [Space, Header("After")]
        [SerializeField, OneLine]
        private WidthAndWeight fixedWidth;
        [SerializeField, OneLine]
        private WidthOnly onlyFixedWidth;
        [SerializeField, OneLine]
        private WidthOnly[] arrayWithOnlyFixedWidths;

        [Serializable]
        public class WidthOnly {
            [SerializeField, Width(50)]
            private string first;
            [SerializeField, Width(75)]
            private string second;
        }
        [Serializable]
        public class WidthAndWeight {
            [SerializeField, Width(50)]
            private string first;
            [SerializeField]
            private string second;
            [SerializeField, Width(75)]
            private string third;
        }
    }
}
