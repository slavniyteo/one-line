using System;
using UnityEngine;

namespace Nihil.OneLine.Examples {

    [Serializable]
    public class Highlight {
        [Header("Before")]
        [SerializeField]
        private OneField highlightedZeroDepthFieldBefore;
        [SerializeField]
        private HighlightedFields highlightedFieldsBefore;
        [SerializeField]
        private HighlightedFields[] arrayWithHighlightedFieldsBefore;

        [Header("After")]
        [SerializeField, OneLine, Highlight]
        private OneField highlightedZeroDepthField;
        [SerializeField, OneLine]
        private HighlightedFields highlightedFields;
        [SerializeField, OneLine]
        private HighlightedFields[] arrayWithHighlightedFields;

        [Serializable]
        public class OneField {
            [SerializeField]
            private string first;
        }
        [Serializable]
        public class HighlightedFields {
            [SerializeField, Highlight(0, 1, 0)]
            private string first;
            [SerializeField, Highlight(0, 0, 1)]
            private string second;
            [SerializeField]
            private string third;
            [SerializeField, Highlight(1, 1, 0)]
            private string fourth;
        }
    }
}