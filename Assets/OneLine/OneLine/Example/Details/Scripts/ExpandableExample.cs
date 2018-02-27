using System;
using UnityEngine;
using OneLine;

namespace OneLine.Examples {
[CreateAssetMenu(menuName = "OneLine/ExpandableExample")]
public class ExpandableExample : ScriptableObject {

    [SerializeField, Expandable]
    private SeparatorExample first;

    [SerializeField, Expandable]
    private Transform second;

    [Serializable]
    public class TwoFields {
        [SerializeField]
        private string first;
        [SerializeField]
        private string second;
    }
}
}