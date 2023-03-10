using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileManager
{

    /// <summary>
    /// Read json file as generic type
    /// <typeparam name="T">Genecric serializable class type</typeparam>
    /// <param name="path">file path. when path is null it will create new file with class name</param>
    /// </summary>
    public static T Read<T>(string path = null) where T : new()
    {
        try
        {
            // if path is null create path with name of class
            if (path == null) path = new T().GetType().Name;
            // if file not exists create a new
            if (!File.Exists(path)) Write(new T(), path);

            using StreamReader reader = new StreamReader(path);
            var file = reader.ReadToEnd();
            return JsonUtility.FromJson<T>(file);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("File rader error: " + ex.Message);
            return new T();
        }
    }
    /// <summary>
    /// Write Generic type to json files
    /// <typeparam name="T">Genecric serializable class type</typeparam>
    /// <param name="value">Genecric serializable class instance</param>
    /// <param name="path">file path</param>
    /// </summary>
    public static void Write<T>(T value, string path)
    {
        try
        {

            using StreamWriter writer = new StreamWriter(path);
            var json = JsonUtility.ToJson(value);
            writer.Write(json);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("File writer error: " + ex.Message);
        }

    }
}
