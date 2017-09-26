using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneLine {
    internal class SpaceDrawer : Drawer {

        public override void AddSlices(SerializedProperty property, Slices slices){
            property.GetCustomAttribute<SpaceAttribute>()
                    .IfPresent(x => slices.Add(new Slice(0, x.height, rect => {})));
        }

    }
}
