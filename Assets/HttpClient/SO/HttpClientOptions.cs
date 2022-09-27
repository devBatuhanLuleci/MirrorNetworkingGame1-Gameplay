using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class HttpClientOptions : ScriptableObject, ISerializationCallbackReceiver
{
    [TextArea]
    public string info = "Web sunucusuan istek atmak için kullan?lacak ayarlar? içerir.";

    public string api = "localhost";

    public bool Log = false;

    public void OnAfterDeserialize()
    {
    }

    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR 
        if (Application.isPlaying)
            Log = EnvironmentConfigiration.Instance.Log;
#endif
    }
}
