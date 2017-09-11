using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal interface FieldDecorator {
        float GetWeight(SerializedProperty property);
        float GetFixedWidth(SerializedProperty property);
        Rect Draw(Rect[] rects, int index, SerializedProperty property);
    }
}