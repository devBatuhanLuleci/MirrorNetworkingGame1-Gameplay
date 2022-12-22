using System;
using System.Reflection;

public static class Extentions
{
    public static string GetStringValue(this Enum value)
    {
        Type type = value.GetType();
        FieldInfo fieldInfo = type.GetField(value.ToString());
        StringValueAttribute[] attributes = fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];

        return attributes.Length > 0 ? attributes[0].StringValue : null;
    }
}