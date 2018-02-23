using System;
using UnityEngine;
using OneLine;

namespace OneLine.Examples {
[CreateAssetMenu(menuName = "OneLine/RangeExample")]
public class RangeExample : ScriptableObject {
    [SerializeField, OneLine]
    private ThreeFields someFloats;
    [SerializeField, Range(0, 1)]
    private float justFloat;
    [SerializeField, OneLine, Range(0, 1)]
    private float pureRange;

    [Serializable]
    public class ThreeFields {
        [SerializeField, Range(-50, 50)]
        private int first;
        [SerializeField]
        private float second;
        [SerializeField, Range(100, 255)]
        private double third;
    }
}
}