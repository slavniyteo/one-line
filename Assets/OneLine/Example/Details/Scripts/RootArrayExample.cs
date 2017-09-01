using System;
using UnityEngine;
using OneLine;

[CreateAssetMenu(menuName = "OneLine/RootArrayExample")]
public class RootArrayExample : ScriptableObject {
    [SerializeField, OneLine]
    private ThreeFields[] rootArray;
    [SerializeField, OneLine]
    private OneLineArray nestedArray;
    [SerializeField, OneLine]
    private TwoArrays twoNestedArrays;

    [Serializable]
    public class ThreeFields {
        [SerializeField]
        private string first;
        [SerializeField]
        private string second;
        [SerializeField]
        private string third;
    }
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
}