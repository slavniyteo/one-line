using System;
using UnityEngine;
using OneLine;

[CreateAssetMenu(menuName = "OneLine/ArrayOfArraysExample")]
public class ArrayOfArraysExample : ScriptableObject {
    [SerializeField, OneLine]
    private NestedArray[] nestedArray;

    [Serializable]
    public class NestedArray {
        [SerializeField, Width(50)]
        private string[] array;
    }
}