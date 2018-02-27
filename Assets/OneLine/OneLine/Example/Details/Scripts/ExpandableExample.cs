using System;
using UnityEngine;
using OneLine;

namespace OneLine.Examples {
[CreateAssetMenu(menuName = "OneLine/ExpandableExample")]
public class ExpandableExample : ScriptableObject {

    [SerializeField, Expandable]
    private ExpandableExample first;

    [SerializeField, Expandable]
    private UnityEngine.Object second;

    [SerializeField, OneLine]
    private TwoFields third;

    [Serializable]
    public class TwoFields {
        [SerializeField]
        private string first;
        [SerializeField, Expandable]
        private ScriptableObject second;
        [SerializeField, Expandable]
        private UnityEngine.Object third;
    }
}
}