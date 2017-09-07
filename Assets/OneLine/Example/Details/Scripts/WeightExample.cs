using System;
using UnityEngine;
using OneLine;

[CreateAssetMenu(menuName = "OneLine/WeightExample")]
public class WeightExample : ScriptableObject {
    [SerializeField, OneLine]
    private Weights differentWeights;

    [Serializable]
    public class Weights {
        [SerializeField, Weight(3)]
        private int first;
        [SerializeField, Weight(2)]
        private int second;
        [SerializeField, Weight(1)]
        private int third;
    }
}