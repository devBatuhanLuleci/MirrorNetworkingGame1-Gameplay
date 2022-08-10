using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class FormSerializer
{
    /// <summary>
    /// Serialize Object ot WWWForm 
    /// Object musth have FormField Attribute
    /// </summary>
    /// <param name="form"></param>
    /// <param name="obj">Serializable object.</param>
    /// <returns></returns>
    public static WWWForm Serialize(this WWWForm form, object obj)
    {
        var myType = obj.GetType();
        var fields = SerializeAppliedPropertyAttributes<FormField>(myType);

        foreach (var field in fields)
        {

            var fieldName = field.Name;
            // ToLover first character for json Name = name
            fieldName = char.ToLower(fieldName[0]) + fieldName.Substring(1);

            form.AddField(fieldName, field.GetValue(obj).ToString());
        }
        return form;
    }
    public static List<FieldInfo> SerializeAppliedPropertyAttributes<T>(Type targetClass) where T : Attribute
    {
        return targetClass.GetFields()
                          .ToList<FieldInfo>()
                          .Where(att => att.GetCustomAttribute<T>() != null)
                          .ToList<FieldInfo>();
    }

}
