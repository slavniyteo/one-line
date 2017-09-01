using System;
using UnityEngine;
using OneLine;

[CreateAssetMenu(menuName = "OneLine/WidthExample")]
public class WidthExample : ScriptableObject {
    [SerializeField, OneLine]
    private WidthAndWeight fixedWidth;

    [Serializable]
    public class WidthAndWeight {
        [SerializeField, Width(75)]
        private string first;
        [SerializeField]
        private string second;
        [SerializeField, Weight(2), Width(25)]
        private string third;
    }
}