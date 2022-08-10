using ProtoBuf;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

public static class Magic
{
    public static byte[] Serialize(object obj)
    {
        if (obj == null) return null;

        try
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize(stream, obj);
                return stream.ToArray();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
        }

        return null;
    }

    public static T Deserialize<T>(byte[] data) where T : class
    {
        if (data == null) return null;

        try
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                return Serializer.Deserialize<T>(stream);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
        }

        return null;
    }

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string randomKey = new string(Enumerable.Repeat(chars, length).Select(s => s[UnityEngine.Random.Range(0, s.Length)]).ToArray());
        return randomKey;
    }
}

