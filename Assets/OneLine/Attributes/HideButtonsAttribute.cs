using System;

namespace Nihil.OneLine {

    ///<summary>
    ///Hides buttons "+" and "-" of marked array.
    ///Available only on d1+arrays.
    ///You can change length of marked array by context-menu commands.
    ///</summary>
    [AttributeUsage(validOn: AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class HideButtonsAttribute : Attribute {

    }
}