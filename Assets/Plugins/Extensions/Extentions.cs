using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class Extentions
{
    public static string GetStringValue(this Enum value)
    {
        Type type = value.GetType();
        FieldInfo fieldInfo = type.GetField(value.ToString());
        StringValueAttribute[] attributes = fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];

        return attributes.Length > 0 ? attributes[0].StringValue : null;
    }
    public static float RandomRange(this float flo, float minValue, float maxValue)
    {
        float angle = UnityEngine.Random.Range(minValue, maxValue) ;
        return angle;

    
    }

}