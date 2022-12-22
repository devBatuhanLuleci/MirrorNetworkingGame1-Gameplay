using System;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

public static class ComponentExtensions
{
    /// <summary>
    ///  Add a new component with data assignets
    /// </summary>
    /// <typeparam name="T">Casting type</typeparam>
    /// <param name="extention">base object</param>
    /// <param name="componentName">Name of component class wil created.</param>
    /// <param name="data">parameters for created compnent</param>
    /// <returns></returns>
    public static T AddComponent<T>(this Component extention, string componentName, object data) where T : Component
    {

        // Create a Type with componentName 
        Type typeOfComponent = Type.GetType(componentName);


        // add a new component on object of called this method
        // component has creating with componentName
        var component = extention.gameObject.AddComponent(typeOfComponent);
        if (data != null)
        {
            component.AddRuntimeFields(data);
            component.AddFields(data);
        }


        return component as T;
    }
    public static void AddFields(this Component extention, object data)
    {
        // get type of data for data fields
        Type typeOfData = data.GetType();

        var typeOfComponent = extention.GetType();
        // Get all runtime fields on  data
        var fieldsOfData = typeOfData.GetFields();


        foreach (var dataField in fieldsOfData)
        {

            // get field of new component with a field of data
            var componentField = typeOfComponent.GetField(dataField.Name);
            // check component has a field like dataField
            if (componentField != null)
            {
                // set data to component field from data object.
                componentField.SetValue(extention, dataField.GetValue(data));
            }
            // if component not have a valid property 
            else
            {
                Debug.LogError(" There is no property called " + dataField.Name + " in " + typeOfComponent.FullName);
            }
        }
    }
    public static void AddRuntimeFields(this Component extention, object data)
    {
        // get type of data for data fields
        Type typeOfData = data.GetType();
        var typeOfComponent = extention.GetType();
        // Get all runtime fields on  data
        var fieldsOfDataRuntime = typeOfData.GetRuntimeFields();


        //a small patern for parsing anonymous types.
        //ex: <name>i_Field pattern will return name
        string pattern = @"<(.*?)>";


        foreach (var dataField in fieldsOfDataRuntime)
        {

            Match match = Regex.Match(dataField.Name, pattern);
            // if field name not contain name you cant assigin any data
            if (!match.Success) continue;
            // get field of new component with a field of data
            var componentField = typeOfComponent.GetField(match.Groups[1].Value);
            // check component has a field like dataField
            if (componentField != null)
            {
                // set data to component field from data object.
                componentField.SetValue(extention, dataField.GetValue(data));
            }
            // if component not have a valid property 
            else
            {
                Debug.LogError(" There is no property called " + dataField.Name + " in " + typeOfComponent.FullName);
            }
        }
    }

    /// <summary>
    ///  Add a new component with data assignets
    /// </summary>
    /// <param name="extention">base object</param>
    /// <param name="componentName">Name of component class wil created.</param>
    /// <param name="data">parameters for created compnent</param>
    /// <returns></returns>
    public static Component AddComponent(this Component extention, string componentName, object data)
    {

        return extention.AddComponent<Component>(componentName, data);
    }

}
