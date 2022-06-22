using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EnumHelper
{
    /// myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description
    public static T GetAttributeOfType<T>(this Enum enumVal) where T:System.Attribute
    {
        var type = enumVal.GetType();
        var memInfo = type.GetMember(enumVal.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
        return (attributes.Length > 0) ? (T)attributes[0] : null;
    }
}
