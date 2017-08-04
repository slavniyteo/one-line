using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal interface Drawer {
        float GetWeight(SerializedProperty property);
        float GetFixedWidth(SerializedProperty property);
        void Draw(Rect rect, SerializedProperty property);
    }
}
