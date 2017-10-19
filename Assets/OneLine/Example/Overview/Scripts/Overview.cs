using System.Collections;
using System;
using UnityEngine;

namespace OneLine.Examples {
    [CreateAssetMenu]
    public class Overview : ScriptableObject {

#region One Line
        [Separator(" Hey There! ")]
        [SerializeField, OneLine]
        private OneLineOneField letMeTellYouAboutTheOneLineAttribute;
        [SerializeField, OneLine]
        private OneLineSecondLine ifWillDrawAllFieldsYouWantIntoOneLineWithAllChildren;
        [SerializeField, OneLine]
        private OneLineWithSpace oneLineWillDrawAllSpaceAttributesInYourCode;
        [SerializeField, OneLine, Tooltip("I'm a tooltip on the ROOT FIELD")]
        private OneLineWithTooltip tooltipAttribute;

        [Serializable]
        public class OneLineOneField {
            [SerializeField]
            private string first;
        }
        [Serializable]
        public class OneLineSecondLine {
            [SerializeField, Width(30)]
            private string first;
            [SerializeField, Width(50)]
            private string second;
            [SerializeField, Width(45)]
            private string third;
            [SerializeField, Width(30)]
            private string forth;
            [SerializeField, Width(40)]
            private string fifth;
            [SerializeField, Width(55)]
            private string sixth;
            [SerializeField, Width(35)]
            private string seventh;
            [SerializeField, Width(35)]
            private string eighth;
            [SerializeField, Separator]
            private string ninth;
        }
        [Serializable]
        public class OneLineWithSpace {
            [SerializeField]
            private string first;
            [Space(25)]
            [SerializeField]
            private string second;
        }
        [Serializable]
        public class OneLineWithTooltip {
            [SerializeField, Weight(0.7f)]
            private string first;
            [SerializeField, Tooltip("I'm here -- tooltip on the second NESTED FIELD!")]
            private string second;
            [SerializeField, Weight(0.9f)]
            private string third;
        }
#endregion

        [Serializable]
public class OneField{ 
    public string first;
}
        [Serializable]
public class TwoFields{
    public string first;
    public string second;
}
        [Serializable]
public class ThreeFields{
    public string first;
    public string second;
    public string third;
}

#region Weights
        [Space, Separator("[ Weight Attribute ]")]
        [SerializeField, OneLine]
        private Weights1 differentWeights1;
        [SerializeField, OneLine]
        private Weights2 differentWeights2;

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
#endregion

#region Width
        [Separator("[ Width Attribute ]")]
        [SerializeField, OneLine]
        private WidthAndWeight fixedWidth;
        [SerializeField, OneLine(Header = LineHeader.Short)]
        private WidthOnly onlyFixedWidth;

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
#endregion

#region Hide Label
        [Separator("[ Hide Label Attribute ]")]
        [SerializeField, OneLine, HideLabel]
        private OneField oneFieldHidesLabel;
        [SerializeField, OneLine, HideLabel]
        private TwoFields twoFieldsHidesLabel;
#endregion

#region Highlight
        [Separator("[ Highlight Attribute ]")]
        [SerializeField, OneLine, Highlight]
        private TwoFields highlightedRootField;
        [SerializeField, OneLine, Highlight, HideLabel]
        private ThreeFields highlightAndHideLabel;
        [SerializeField, OneLine]
        private HighlightedFields highlightedFields;
        [SerializeField, OneLine]
        private HighlightedFields[] arrayWithHighlightedFields;

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
#endregion

#region Arrays
        [Separator("Arrays, [Hide Buttons Attribute], [Array Length]")]
        [SerializeField, OneLine]
        private OneLineArray oneLineArray;
        [SerializeField, OneLine]
        private TwoArrays twoArraysInOneLine;
        [SerializeField, OneLine]
        private ComplexArray arrayWithComplexFields;
        [SerializeField, OneLine]
        private ArrayHidesButtons arrayHidesButtons;
        [SerializeField, OneLine]
        private ImmutableLengthArray arrayWithImmutableLength;
        [SerializeField, OneLine(Header = LineHeader.Short)]
        private ThreeFields[] array;
        [SerializeField, OneLine(Header = LineHeader.Short)]
        private OneLineArray[] arrayWithArrays;
        [SerializeField, OneLine(Header = LineHeader.Short)]
        private OneLineArrayWithHeader[] arrayWithArraysWithHeader;

        [Serializable]
        public class OneLineArray {
            [SerializeField]
            private string first;
            [SerializeField]
            private OneField[] array;
            [SerializeField]
            private string last;
        }
        [Serializable]
        public class OneLineArrayWithHeader {
            [SerializeField, Width(50)]
            private string first;
            [SerializeField, Width(50)]
            private string[] array;
        }
        [Serializable]
        public class ComplexArray {
            [SerializeField]
            private TwoFields[] array;
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
#endregion

    }
}
